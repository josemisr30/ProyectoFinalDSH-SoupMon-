using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class PanelMenu : MonoBehaviour
{
    static public PlayerData playerData = new PlayerData(); // Instancia global de datos del jugador
	public Cursor Cursor; //Referencia al cursor
	
    public GameObject menuPanel; // Panel del menú principal
    public Button[] menuButtons; // Botones del menú en orden (Pokémon, Mochila, etc.)
    public GameObject player; // Referencia al objeto del personaje para desactivar el movimiento
	public TMP_Text Playername; //Referencia al objeto del boton de los datos del jugador
	
	//BotonMenuDataPlayer
	public GameObject playerMenuData; //Referencia al canvas de los datos del jugador
	public TMP_Text InfoName; //Referencia al nombre del jugador
	public TMP_Text InfoId; //Referencia al ID del jugador
	public TMP_Text InfoDinero; //Referencia al Dinero del jugador
	public GameObject ChicoSprite; //Referencia al Sprite del jugador Chico
	public GameObject ChicaSprite; //Referencia al Sprite del jugador Chica
	
	//BotonMenuBagPlayer
	public GameObject playerMenuBag; //Referencia al canvas de la mochila del jugador
	public GameObject playerMenuSoupMon; //Referencia al canvas de los SoupMons del jugador
	
	
    static public int currentIndex = 0; // Índice del botón actualmente seleccionado

    void Start()
    {
        // Asegurarnos de que el menú esté oculto inicialmente
        menuPanel.SetActive(false);

        if (menuButtons.Length > 0)
        {
            menuButtons[currentIndex].Select();
        }
		
		Playername.text = PlayerPrefs.GetString("PlayerName");
		InfoName.text = PlayerPrefs.GetString("PlayerName");
		InfoId.text = PlayerPrefs.GetString("ID");
		//InfoDinero.text = 3000; //Dato por ahora visual y no real (No es porsible operar con el)
		
    }

    void Update()
    {
        // Detectar la tecla para abrir el menú
        if (Input.GetKeyDown(KeyCode.M) && !menuPanel.activeSelf) // Tecla M para abrir el menú
        {
            OpenMenu();
        }

        // Manejar navegación dentro del menú
        if (menuPanel.activeSelf)
        {
            HandleMenuNavigation();
        }
		
    }

    public void OpenMenu()
    {
        // Activar el menú
        menuPanel.SetActive(true);
        menuButtons[0].Select();

        // Desactivar el movimiento del personaje
        if (player != null)
        {
            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = false; // Desactivar movimiento
                Debug.Log("Movimiento del personaje desactivado.");
            }
        }
		Cursor.ResetCursor();
    }
	
	// Cerrar menu
    public void CloseMenu()
	{
		if (playerMenuData.activeSelf)
		{
			playerMenuData.SetActive(false);
			menuButtons[0].Select();
		}
		else if (playerMenuBag.activeSelf)
		{
			playerMenuBag.SetActive(false);
			menuButtons[0].Select();
		}
		else if (playerMenuSoupMon.activeSelf)
		{
			playerMenuSoupMon.SetActive(false);
			menuButtons[0].Select();
		}
		else
		{
			menuPanel.SetActive(false);

			// Reactivar el movimiento del personaje
			if (player != null)
			{
				var movementScript = player.GetComponent<PlayerMovement>();
				if (movementScript != null)
				{
					movementScript.enabled = true; // Activar movimiento
					Debug.Log("Movimiento del personaje activado.");
				}
			}
		}

		// Reseteo del índice y reselección
		currentIndex = 0;
		menuButtons[0].Select();
		Cursor.ResetCursor();
	}


    void HandleMenuNavigation()
    {
        // Detectar navegación con flechas del teclado
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % menuButtons.Length; // Mover hacia abajo
            menuButtons[currentIndex].Select();
            Debug.Log($"Botón seleccionado: {menuButtons[currentIndex].name}");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length; // Mover hacia arriba
            menuButtons[currentIndex].Select();
            Debug.Log($"Botón seleccionado: {menuButtons[currentIndex].name}");
        }

        // Detectar selección con Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log($"Botón ejecutado: {menuButtons[currentIndex].name}");
            menuButtons[currentIndex].onClick.Invoke(); // Invocar la función asociada al botón
        }

        // Salir del menú con tecla Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    // Funciones para cada opción del menú
    public void OnSoupMonButton()
    {
        Debug.Log("Abrir lista de SoupMon.");
        // Aquí iría el código para abrir la lista de SoupMon
		playerMenuSoupMon.SetActive(true);
		menuButtons[0].Select();
		Cursor.ResetCursor();
    }

    public void OnBagButton()
    {
        Debug.Log("Abrir mochila.");
        // Aquí iría el código para abrir la mochila
		playerMenuBag.SetActive(true);
		menuButtons[0].Select();
		Cursor.ResetCursor();
    }

    public void OnPlayerButton()
    {
        //Estadisticas del jugador
		playerMenuData.SetActive(true);
		
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
		menuButtons[0].Select();
		Cursor.ResetCursor();
    }

    public void OnSaveButton()
	{
		Debug.Log("Guardar partida.");

		playerData.playerName = PlayerPrefs.GetString("PlayerName", "DefaultName"); // Nombre del jugador
		playerData.gender = PlayerPrefs.GetString("Aspect", "DefaultGender"); // Género del jugador
		playerData.ID = PlayerPrefs.GetString("ID", "00000"); // ID del jugador
		playerData.dinero = 3000; // Dinero del jugador
		playerData.Scene = SceneManager.GetActiveScene().name;

		if (player != null)
		{
			playerData.posX = player.transform.position.x;
			playerData.posY = player.transform.position.y;
			playerData.posZ = player.transform.position.z;
		}

		// Guardar inventario
		playerData.objetos = new List<Item>(BagManager.objetos);
		playerData.balls = new List<Item>(BagManager.balls);
		playerData.objetosClave = new List<Item>(BagManager.objetosClave);

		// **Guardar la lista de Soupmons**
		playerData.claseSoupmon[0] = SoupmonManager.soupArray[0];
		playerData.claseSoupmon[1] = SoupmonManager.soupArray[1];
		playerData.claseSoupmon[2] = SoupmonManager.soupArray[2];
		playerData.Soupmon1 = Inicial.Soupmon1;
		playerData.Soupmon2 = Inicial.Soupmon1;
		playerData.Soupmon3 = Inicial.Soupmon1;

		SaveGame(playerData); // Guardar en JSON
		menuButtons[0].Select();
		Cursor.ResetCursor();
	}
	

    public void OnOptionsButton()
    {
        Debug.Log("Abrir opciones.");
        // Aquí iría el código para las configuraciones del juego
		menuButtons[0].Select();
		Cursor.ResetCursor();
    }

    public void OnExitButton()
    {
        Debug.Log("Salir del menú.");
        CloseMenu(); // Cerrar el menú
		menuButtons[0].Select();
		Cursor.ResetCursor();
    }
	
	
	private void SaveGame(PlayerData playerData)
	{
		string jsonPath = Application.persistentDataPath + "/gameSave.json";
		string json = JsonUtility.ToJson(playerData, true);
		System.IO.File.WriteAllText(jsonPath, json);

		Debug.Log($"Partida guardada en: {jsonPath}");
	}
	

}
