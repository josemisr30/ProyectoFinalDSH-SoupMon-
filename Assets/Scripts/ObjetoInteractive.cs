using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjetoInteractive : MonoBehaviour {
	
    [Header("Managers")]
    public DialogueManager dialogueManager; // Referencia al DialogueManager para gestionar los dialogos



    private bool jugadorEnRango = false; // Booleano que comprueba si esta en el Rango el jugador

 
	public GameObject player; // Referencia al objeto del personaje para desactivar el movimiento


    void Update() {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E)) { //Si el jugador esta en Rango y pulsa E interactua con el objeto
            InteractuarConObjeto();
        }
    }

    private void OnTriggerEnter(Collider other) { //Jugador entra en Rango
        if (other.CompareTag("Player")) {
            Debug.Log("Jugador ha entrado en rango del Objeto.");
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit(Collider other) { //Jugador sale de Rango
        if (other.CompareTag("Player")) {
            Debug.Log("Jugador ha salido del rango del Objeto.");
            jugadorEnRango = false;
        }
    }

    public void InteractuarConObjeto() {
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
		
		BagManager.balls[0].quantity += 10; // Le agrega 10 sticks
		BagManager.objetos[0].quantity += 10; //Le agrega 10 pociones
		//Invoca el dialogo que muestra el mensaje al jugador de que ya ha tomado los objetos
		dialogueManager.StartDialogue(new string[] { "Has tomado los objetos" }, CerrarOpciones);
        
    }


    void CerrarOpciones() { 
		// Activa el movimiento del personaje
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
