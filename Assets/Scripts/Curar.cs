using UnityEngine;

public class Curar : MonoBehaviour
{
    [Header("Managers")]
    public DialogueManager dialogueManager; // Referencia al manejador de diálogo
    private bool jugadorEnRango = false; // Variable para verificar si el jugador está en rango

    public string[] Enfermera; // Array de diálogos de la enfermera
    public GameObject player; // Referencia al objeto del jugador para desactivar su movimiento

    void Update() {
        // Si el jugador está en rango y presiona "E", inicia la interacción
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E)) {
            InteractuarConNPC();
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Detecta si el jugador entra en el rango del NPC
        if (other.CompareTag("Player")) {
            Debug.Log("Jugador ha entrado en rango del NPC.");
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        // Detecta si el jugador sale del rango del NPC
        if (other.CompareTag("Player")) {
            Debug.Log("Jugador ha salido del rango del NPC.");
            jugadorEnRango = false;
        }
    }

    public void InteractuarConNPC() {
        // Desactiva el movimiento del jugador al iniciar la conversación
        if (player != null)
        {
            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = false; // Desactivar movimiento
                Debug.Log("Movimiento del personaje desactivado.");
            }
        }

        // Inicia el diálogo con la enfermera y ejecuta la acción de curar
        dialogueManager.StartDialogue(Enfermera, CurarVida);
    }

    public void CurarVida()
    {
        // Revitaliza la vida de los personajes en las listas correspondientes
        if(GameManager.instance.currentPlayerData.claseSoupmon[0]!=null) GameManager.instance.currentPlayerData.claseSoupmon[0].forzarVida(GameManager.instance.currentPlayerData.claseSoupmon[0].vidaMaxSopa());
        if(GameManager.instance.currentPlayerData.claseSoupmon[1]!=null) GameManager.instance.currentPlayerData.claseSoupmon[1].forzarVida(GameManager.instance.currentPlayerData.claseSoupmon[1].vidaMaxSopa());
        if(GameManager.instance.currentPlayerData.claseSoupmon[2]!=null) GameManager.instance.currentPlayerData.claseSoupmon[2].forzarVida(GameManager.instance.currentPlayerData.claseSoupmon[2].vidaMaxSopa());

        if(Sopa.sopaPlayer[0]!=null) Sopa.sopaPlayer[0].forzarVida(Sopa.sopaPlayer[0].vidaMaxSopa());
        if(Sopa.sopaPlayer[1]!=null) Sopa.sopaPlayer[1].forzarVida(Sopa.sopaPlayer[1].vidaMaxSopa());
        if(Sopa.sopaPlayer[2]!=null) Sopa.sopaPlayer[2].forzarVida(Sopa.sopaPlayer[2].vidaMaxSopa());

        // Finaliza la conversación y permite el movimiento nuevamente
        FinConversacion();
    }

    void FinConversacion() {
        // Reactiva el movimiento del jugador una vez finalizada la conversación
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
}