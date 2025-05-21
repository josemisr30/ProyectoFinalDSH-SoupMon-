using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NCPManager : MonoBehaviour
{
	[Header("Managers")]
    public DialogueManager dialogueManager;
	
	private bool jugadorEnRango = false;
	public GameObject player; // Referencia al objeto del personaje para desactivar el movimiento
	
	public string[] dialogoNPC;


    void Update() {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E)) {
            InteractuarConNPC();
        }
    }
	
	private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Jugador ha entrado en rango del NPC.");
            jugadorEnRango = true;
        }
    }
	
	private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Jugador ha salido del rango del NPC.");
            jugadorEnRango = false;
        }
    }
	
	public void InteractuarConNPC() {
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
        
        dialogueManager.StartDialogue(dialogoNPC, FinConversacion);

    }
	
	
	void FinConversacion() {
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



    

    

    




    

