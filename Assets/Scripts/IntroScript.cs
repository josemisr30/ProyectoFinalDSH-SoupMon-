using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroScript : MonoBehaviour
{
    [Header("Managers")]
    public DialogueManager dialogueManager; // Para manejar los diálogos

    [Header("UI Elements")]
    public GameObject aspectSelector; // Canvas para elegir género
    public GameObject nameInputPanel; // Canvas para introducir nombre
    public TMP_InputField nameInput; // Campo de texto para el nombre del jugador

	public GameObject ChicoSprite; //Sprite del chico
	public GameObject ChicaSprite; //Sprite del chica
	public GameObject SoupMonSprite; //Sprite del Soupmon
	public GameObject LivooSprite; //Sprite del profesor
    private string playerName; // Variable para almacenar el nombre del jugador
	public string idT; //Variable para almacenar el id temporal del jugador

    private void Start()
    {
        StartIntro(); // Comenzar la introducción
    }

    private void Update()
    {
        // Permitir confirmar el nombre con Enter mientras el panel está activo
        if (nameInputPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmName(); // Confirmar nombre
        }
    }

    private void StartIntro()
    {
        string[] oakDialogue = 
        {
			"¿Eres un chico?",
			"¿O una chica?"
		};

        dialogueManager.StartDialogue(oakDialogue, OnAspectSelection); // Comienza el diálogo inicial
    }

    private void OnAspectSelection()
    {
        aspectSelector.SetActive(true); // Muestra el selector de género
    }

    public void SelectAspect(string aspect)
    {
		// Guardar el género seleccionado
		PlayerPrefs.SetString("Aspect", aspect);

        aspectSelector.SetActive(false); // Oculta el selector de género
		SoupMonSprite.SetActive(true);
        string[] askName = 
		{
            "......................",
            "Hola! ¡Perdona por la espera!",
            "¡Estas en el mundo de los",
			"SOUPMON!",
			"Me llamo Livoo",
            "Pero me llaman PROFESOR SOUPMON",
			//Aqui deberia ir la foto de un sopamon
			"Este mundo está habitado por unas",
			"criaturas llamadas SOUPMON.",
            "la gente y los SOUPMON conviven",
			"ayudándose unos a otros.",
			"Algunos juegan con los SOUPMON, otros",
			"luchan con ellos.",
			//Vuelve el profesor
			"Pero aún hay muchas cosas que",
            "no sabemos.",
			"Quedan muchos misterios por",
			"resolver. Por eso",
			"estudio a diario a los SOUPMON.",
			//Sale tu sprite
			"¿Cómo has dicho que te llamas?"
        };
        dialogueManager.StartDialogue(askName, ShowNameInput); // Pregunta por el nombre
		Debug.Log($"Género seleccionado: {PlayerPrefs.GetString("Aspect")}");
    }

    private void ShowNameInput()
    {
        nameInputPanel.SetActive(true); // Activa el panel de entrada de nombre
        nameInput.text = ""; // Limpia el campo de texto
		nameInput.characterLimit = 8; // Limita el input a 8 caracteres
        nameInput.Select(); // Activa el campo de entrada
    }

    private void ConfirmName()
    {
		
        playerName = nameInput.text.Trim(); // Obtén el nombre ingresado

        if (string.IsNullOrEmpty(playerName)) // Validación del nombre
        {
            Debug.LogWarning("El nombre no puede estar vacío.");
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName); // Guarda el nombre como preferencia
        
		//string gender = PlayerPrefs.GetString("Aspect"); //Obtener genero seleccionado
		
		idT = GenerateRandomID();
		Debug.Log("ID generado: " + idT);
		PlayerPrefs.SetString("ID", idT); // Guarda el id como preferencia

		// Crear instancia de PlayerData
		PlayerData playerData = new PlayerData
		{
			playerName = playerName,
			gender = PlayerPrefs.GetString("Aspect"),
			ID = idT
		};
		//SaveBasicData(playerData); //Guarda datos basicos en txt
		
		nameInputPanel.SetActive(false); // Oculta el panel de entrada de nombre
		LivooSprite.SetActive(false);
		SoupMonSprite.SetActive(false);
		if(PlayerPrefs.GetString("Aspect") == "Chico")
		{
			ChicoSprite.SetActive(true);
			ChicaSprite.SetActive(false);
		}
		else
		{
			ChicaSprite.SetActive(true);
			ChicoSprite.SetActive(false);
		}
		
        string[] finalMessage = 
		{ 
			$"{playerName}, preparate.",
			"Tu propia historia SOUPMON está",
			"a punto de empezar.",
			"Te divertirás y te enfrentarás a",
			"duros desafíos.",
			"¡Te espera un mundo de sueños y",
			"aventuras con SOUPMON! ¡Vamos!",
			"¡Nos vemos!"
		};
        dialogueManager.StartDialogue(finalMessage, StartGame); // Muestra el mensaje final
    }
	

	public string GenerateRandomID()
	{
		int randomNumber = UnityEngine.Random.Range(10000, 100000); // Genera número de 5 dígitos
		return randomNumber.ToString(); // Convierte el número a cadena
	}


	public void SaveBasicData(PlayerData playerData)
	{
		string txtPath = Application.persistentDataPath + "/newGameData.txt";
		string data = $"{playerData.playerName};{playerData.gender}";
		System.IO.File.WriteAllText(txtPath, data);

		Debug.Log($"Datos básicos guardados en: {txtPath}");

	}

    private void StartGame()
    {
        Debug.Log("Iniciando el juego...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Interior"); // Carga la escena principal
    }
}

