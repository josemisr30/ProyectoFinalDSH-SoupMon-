using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;

// Clase principal que gestiona la lógica de la pantalla de inicio y los menús
public class PantallaInicio : MonoBehaviour
{
    // Referencias a los distintos menús del juego
    public GameObject canvasInicio;
    public GameObject canvasMenu;
    public GameObject canvasOpciones;
    public GameObject canvasNuevaPartida;

    // Array de botones del menú principal
    public Button[] botonesMenu;
    // Índice del botón actualmente seleccionado
    private int indiceActual = 0;

    // Nombre de la escena donde se guarda la partida (no se utiliza en este script)
    public string nombreEscenaGuardada;

    // Referencia al sistema de eventos de Unity
    private EventSystem eventSystem;
    // Almacena los colores originales de los botones para restaurarlos al deseleccionarlos
    private Color[] coloresOriginales;

    // Fuente de audio para reproducir sonidos al interactuar con los botones
    public AudioSource buttonSound;

    // Método que se ejecuta al iniciar la escena
    void Start() {
        // Obtiene la referencia al sistema de eventos actual
        eventSystem = EventSystem.current;

        // Activa solo el canvas de inicio y desactiva los demás
        canvasInicio.SetActive(true);
        canvasMenu.SetActive(false);
        canvasOpciones.SetActive(false);
        canvasNuevaPartida.SetActive(false);

        // Guarda los colores originales de los botones
        coloresOriginales = new Color[botonesMenu.Length];
        for (int i = 0; i < botonesMenu.Length; i++) {
            coloresOriginales[i] = botonesMenu[i].colors.normalColor;
        }

        // Selecciona el primer botón al iniciar
        indiceActual = 0;
        ActualizarBotonSeleccionado();
    }

    // Método que se ejecuta una vez por frame
    void Update() {
        // Si el menú principal está activo, permite navegar por los botones
        if (canvasMenu.activeSelf) {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                buttonSound.Play(); // Sonido al bajar en el menú
                indiceActual = (indiceActual + 1) % botonesMenu.Length;
                ActualizarBotonSeleccionado();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                buttonSound.Play(); // Sonido al subir en el menú
                indiceActual = (indiceActual - 1 + botonesMenu.Length) % botonesMenu.Length;
                ActualizarBotonSeleccionado();
            }
            else if (Input.GetKeyDown(KeyCode.Return)) {
                buttonSound.Play(); // Sonido al seleccionar un botón
                Debug.Log("Activando boton: " + botonesMenu[indiceActual].name);
                botonesMenu[indiceActual].onClick.Invoke(); // Ejecuta la acción del botón
            }
        }

        // Si estamos en la pantalla de inicio y se presiona Enter, se pasa al menú principal
        if (canvasInicio.activeSelf && Input.GetKeyDown(KeyCode.Return)) {
            buttonSound.Play(); // Sonido al salir del menú de inicio
            Debug.Log("Cambiando al menu principal.");
            PulsarStart();
        }
    }

    // Activa el menú principal al salir de la pantalla de inicio
    public void PulsarStart()
    {
        canvasInicio.SetActive(false);
        canvasMenu.SetActive(true);

        // Reinicia la selección de botones
        indiceActual = 0;
        ActualizarBotonSeleccionado();
    }

    // Lógica al seleccionar "Nueva Partida"
    public void NuevaPartida()
    {
        Debug.Log("Nueva Partida seleccionada.");
        canvasMenu.SetActive(false);
        canvasNuevaPartida.SetActive(true);

        // Inicializa datos del jugador en GameManager
        GameManager.instance.currentPlayerData.posX = -2;
        GameManager.instance.currentPlayerData.posY = 2;
        GameManager.instance.currentPlayerData.posZ = -2;
        GameManager.instance.currentPlayerData.objetos = null;
        GameManager.instance.currentPlayerData.balls = null;
        GameManager.instance.currentPlayerData.claseSoupmon = null;
		GameManager.instance.currentPlayerData.Soupmon1 = null;
		GameManager.instance.currentPlayerData.Soupmon2 = null;
		GameManager.instance.currentPlayerData.Soupmon3 = null;
    }

    // Continúa la partida usando el GameManager
    public void Continuar()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ContinueGame();
        }
        else
        {
            Debug.LogWarning("No se encontro GameManager.");
        }
    }

    // Método para abrir las opciones (comentado)
    public void Opciones()
    {
        Debug.Log("Opciones seleccionadas.");
        //canvasMenu.SetActive(false);
        //canvasOpciones.SetActive(true);
    }

    // Vuelve del submenú al menú principal
    public void VolverAlMenu()
    {
        Debug.Log("Volviendo al menu principal.");
        canvasMenu.SetActive(true);
        canvasOpciones.SetActive(false);
        canvasNuevaPartida.SetActive(false);

        // Restablece el foco al primer botón
        indiceActual = 0;
        ActualizarBotonSeleccionado();
    }

    // Cambia la apariencia del botón seleccionado y restaura los demás
    private void ActualizarBotonSeleccionado() {
        // Restaura los colores originales a todos los botones
        for (int i = 0; i < botonesMenu.Length; i++) {
            var colores = botonesMenu[i].colors;
            colores.normalColor = coloresOriginales[i];
            botonesMenu[i].colors = colores;
        }

        // Selecciona visualmente el botón actual
        eventSystem.SetSelectedGameObject(botonesMenu[indiceActual].gameObject);

        // Cambia el color del botón seleccionado a amarillo
        var coloresSeleccionado = botonesMenu[indiceActual].colors;
        coloresSeleccionado.normalColor = new Color(1, 1, 0, 1);
        botonesMenu[indiceActual].colors = coloresSeleccionado;

        Debug.Log("Boton seleccionado: " + botonesMenu[indiceActual].name);
    }
}
