using UnityEngine;
using UnityEngine.InputSystem;

public class EleccionSoupmon : MonoBehaviour
{
    [Header("Managers")]
    public DialogueManager dialogueManager; // Administrador de diálogos

    private bool jugadorEnRango = false; // Variable para detectar si el jugador está en rango
    public GameObject player; // Referencia al personaje para manejar el movimiento

    // Paneles de selección de Soupmon según su tipo
    public GameObject SoupmonF; // Fuego
    public GameObject SoupmonA; // Agua
    public GameObject SoupmonP; // Planta

    // Objetos que indican selección visual
    public GameObject StickF;
    public GameObject StickA;
    public GameObject StickP;

    // Estado global para asegurar que el jugador solo elige un Soupmon inicial
    static public bool SoupmonInicialDisp = true;

    // Líneas de diálogo asociadas
    public string[] Livoo; // Diálogo inicial
    public string[] NoTieneSoupmon; // Diálogo cuando no hay Soupmon disponible
    private PlayerInput plIn;

    private void Start()
    {
        plIn = GetComponent<PlayerInput>();
        plIn.actions.FindActionMap("ControlMundo").Enable();
        plIn.actions.FindActionMap("Combate").Disable();
        plIn.actions.FindActionMap("Dialogo").Disable();
    }
    void Update()
    {
        // Si el jugador está en rango y presiona "E", inicia la interacción
        if (jugadorEnRango && plIn.actions["B"].ReadValue<float>() == 1)
        {
            InteractuarConNPC();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si el jugador entra en el rango del NPC
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha entrado en rango del NPC.");
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detecta si el jugador sale del rango del NPC
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha salido del rango del NPC.");
            jugadorEnRango = false;
        }
    }

    public void InteractuarConNPC()
    {
        // Desactiva el movimiento del jugador antes de iniciar la conversación
        if (player != null)
        {
            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = false;
                plIn.actions.FindActionMap("ControlMundo").Disable();
                plIn.actions.FindActionMap("Combate").Disable();
                plIn.actions.FindActionMap("Dialogo").Enable();
                Debug.Log("Movimiento del personaje desactivado.");
            }
        }

        // Inicia el diálogo y permite la elección de un Soupmon
        dialogueManager.StartDialogue(Livoo, Elegir);
    }

    void Elegir()
    {
        // Reactiva el movimiento del jugador tras la conversación
        if (player != null)
        {
            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = true;
                plIn.actions.FindActionMap("ControlMundo").Enable();
                plIn.actions.FindActionMap("Combate").Disable();
                plIn.actions.FindActionMap("Dialogo").Disable();
                Debug.Log("Movimiento del personaje activado.");
            }
        }

        // Muestra los botones de selección de Soupmon
        StickF.SetActive(true);
        StickA.SetActive(true);
        StickP.SetActive(true);
        Inicial.elegido = false;


    }

    void FinConversacion()
    {
        // Reactiva el movimiento del jugador al finalizar la conversación
        if (player != null)
        {
            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = true;
                plIn.actions.FindActionMap("ControlMundo").Enable();
                plIn.actions.FindActionMap("Combate").Disable();
                plIn.actions.FindActionMap("Dialogo").Disable();
                Debug.Log("Movimiento del personaje activado.");
            }
        }
    }
}