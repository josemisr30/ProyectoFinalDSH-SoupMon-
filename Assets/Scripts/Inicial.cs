using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Inicial : MonoBehaviour
{
	// Referencia al gestor de diálogos
	[Header("Managers")]
	public DialogueManager dialogueManager;

	public SoupmonManager SoupmonManager; // Referencia al manager de Soupmon

	private bool jugadorEnRango = false; // Indica si el jugador está cerca del NPC
	public GameObject player; // Referencia al jugador para desactivar su movimiento

	public GameObject SoupmonF; // Panel para seleccionar Soupmon de Fuego
	public GameObject SoupmonA; // Panel para seleccionar Soupmon de Agua
	public GameObject SoupmonP; // Panel para seleccionar Soupmon de Planta

	public GameObject StickF; // Objeto interactuable de fuego
	public GameObject StickA; // Objeto interactuable de agua
	public GameObject StickP; // Objeto interactuable de planta

	static public string Soupmon1; // Datos del Soupmon seleccionado como string

	public string InicialTipo; // Tipo de Soupmon que representa este NPC
	public string[] SoupmonSeleccionado; // Lista de Soupmon elegibles
	private bool eligiendoFuego = false; // Indica si se está eligiendo el de fuego
	private bool eligiendoAgua = false; // Indica si se está eligiendo el de agua
	private bool eligiendoPlanta = false; // Indica si se está eligiendo el de planta
	private PlayerInput plIn;
	public static bool elegido=true;

	private void Start()
	{
		plIn = GetComponent<PlayerInput>();
		plIn.actions.FindActionMap("ControlMundo").Enable();
		plIn.actions.FindActionMap("Combate").Disable();
		plIn.actions.FindActionMap("Dialogo").Disable();
	}
	void Update()
	{
		gameObject.SetActive(!elegido);
		Debug.Log(elegido);
		// Detecta si el jugador interactúa con el NPC
		if (jugadorEnRango && plIn.actions["A"].ReadValue<float>() == 1)
		{
			plIn.actions.FindActionMap("ControlMundo").Disable();
			plIn.actions.FindActionMap("Combate").Disable();
			plIn.actions.FindActionMap("Dialogo").Enable();
			StartCoroutine(ReactivarDialogo());
			InteractuarStick();
		}

		// Lógica al seleccionar el Soupmon de fuego
		if (eligiendoFuego)
		{
			if (plIn.actions["A"].ReadValue<float>() == 1)
			{
				// Se confirma elección de "Fuegato"
				SopamonData nuevo = new SopamonData("Fuegato", 5, 20, 7, 80, 10, 4, false, false, 2);
				Soupmon1 = "Fuegato, 5, 20, 7, 80, 10, 4, false, false, 2)";
				Sopa.sopaPlayer[0] = nuevo; // Se asigna al jugador

				EleccionSoupmon.SoupmonInicialDisp = false; // Se desactiva la disponibilidad
				SoupmonF.SetActive(false); // Se oculta panel
				eligiendoFuego = false;
				elegido = true;
				FinConversacion(); // Se finaliza conversación
			}
			else if (plIn.actions["B"].ReadValue<float>() == 1)
			{
				SoupmonF.SetActive(false);
				FinConversacion();
				eligiendoFuego = false;
			}
		}
		// Lógica al seleccionar el Soupmon de agua
		else if (eligiendoAgua)
		{
			if (plIn.actions["A"].ReadValue<float>() == 1)
			{
				// Se confirma elección de "Meladusa"
				SopamonData nuevo = new SopamonData("Meladusa", 5, 20, 7, 80, 10, 6, false, false, 2);
				Soupmon1 = "Meladusa, 5, 20, 7, 80, 10, 6, false, false, 2)";
				Sopa.sopaPlayer[0] = nuevo;

				EleccionSoupmon.SoupmonInicialDisp = false;
				SoupmonA.SetActive(false);
				eligiendoAgua = false;
				elegido = true;
				FinConversacion();
			}
			else if (plIn.actions["B"].ReadValue<float>() == 1)
			{
				SoupmonA.SetActive(false);
				FinConversacion();
				eligiendoAgua = false;
			}
		}
		// Lógica al seleccionar el Soupmon de planta
		else if (eligiendoPlanta)
		{
			if (plIn.actions["A"].ReadValue<float>() == 1)
			{
				// Se confirma elección de "Noteveo"
				SopamonData nuevo = new SopamonData("Noteveo", 5, 20, 7, 80, 10, 5, false, false, 2);
				Soupmon1 = "Noteveo, 5, 20, 7, 80, 10, 5, false, false, 2)";
				Sopa.sopaPlayer[0] = nuevo;

				EleccionSoupmon.SoupmonInicialDisp = false;
				SoupmonP.SetActive(false);
				eligiendoPlanta = false;
				elegido = true;
				FinConversacion();
			}
			else if (plIn.actions["B"].ReadValue<float>() == 1)
			{
				SoupmonP.SetActive(false);
				FinConversacion();
				eligiendoPlanta = false;
			}
		}
	}

	// Detecta si el jugador entra en el rango de interacción
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			jugadorEnRango = true;
		}
	}

	// Detecta si el jugador sale del rango de interacción
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			jugadorEnRango = false;
		}
	}

	// Determina qué tipo de Soupmon inicial se ofrece
	void InteractuarStick()
	{
		if (InicialTipo == "Fuego")
		{
			InicialFuego();
		}
		else if (InicialTipo == "Agua")
		{
			InicialAgua();
		}
		else if (InicialTipo == "Planta")
		{
			InicialPlanta();
		}
	}

	// Activa panel y estado de elección de fuego
	void InicialFuego()
	{
		if (EleccionSoupmon.SoupmonInicialDisp)
		{
			// Desactiva movimiento del jugador
			if (player != null)
			{
				var movementScript = player.GetComponent<PlayerMovement>();
				if (movementScript != null)
				{
					movementScript.enabled = false;
				}
			}
			SoupmonF.SetActive(true);
			eligiendoFuego = true;
		}
	}

	// Activa panel y estado de elección de agua
	void InicialAgua()
	{
		if (EleccionSoupmon.SoupmonInicialDisp)
		{
			if (player != null)
			{
				var movementScript = player.GetComponent<PlayerMovement>();
				if (movementScript != null)
				{
					movementScript.enabled = false;
				}
			}
			SoupmonA.SetActive(true);
			eligiendoAgua = true;
		}
	}

	// Activa panel y estado de elección de planta
	void InicialPlanta()
	{
		if (EleccionSoupmon.SoupmonInicialDisp)
		{
			if (player != null)
			{
				var movementScript = player.GetComponent<PlayerMovement>();
				if (movementScript != null)
				{
					movementScript.enabled = false;
				}
			}
			SoupmonP.SetActive(true);
			eligiendoPlanta = true;
		}
	}

	// Restaura el movimiento del jugador al finalizar la elección
	void FinConversacion()
	{
		if (player != null)
		{
			var movementScript = player.GetComponent<PlayerMovement>();
			if (movementScript != null)
			{
				movementScript.enabled = true;
			}
		}
	}
	
	private IEnumerator ReactivarDialogo()
    {
        yield return new WaitForSeconds(0.2f);
        plIn.actions.FindActionMap("Dialogo").Enable();
    }
}
