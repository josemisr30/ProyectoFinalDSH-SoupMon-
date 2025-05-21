using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Instancia única del GameManager

    public PlayerData currentPlayerData; // Datos del jugador

    void Start()
    {
        // Verifica si hay datos de posición guardados y los aplica al jugador
        if (GameManager.instance != null && GameManager.instance.currentPlayerData != null)
        {
            Vector3 pos = new Vector3(
                GameManager.instance.currentPlayerData.posX,
                GameManager.instance.currentPlayerData.posY,
                GameManager.instance.currentPlayerData.posZ
            );

            // Reposiciona al jugador en la ubicación guardada
            transform.position = pos;
            Debug.Log("Jugador reposicionado en Start() a: " + pos);
        }
    }

    private void Awake()
    {
        // Configuración de Singleton para asegurar que solo haya un GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el GameManager al cambiar de escena
            LoadGame(); // Carga datos guardados al inicio del juego
        }
        else
        {
            Destroy(gameObject); // Si ya existe un GameManager, destruye este duplicado
        }
    }


    // Carga los datos del jugador guardados desde un archivo JSON.
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/gameSave.json";

        // Verifica si el archivo de guardado existe
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentPlayerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Partida cargada con éxito.");
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de guardado.");
        }
    }


    // Carga la última escena guardada y posiciona al jugador.
    public void ContinueGame()
    {
        // Verifica que haya datos guardados antes de continuar el juego
        if (currentPlayerData != null)
        {
            SceneManager.LoadScene(currentPlayerData.Scene);
        }
        else
        {
            Debug.LogWarning("No hay datos cargados para continuar.");
        }
    }
}