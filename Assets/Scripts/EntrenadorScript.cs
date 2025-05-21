using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

 [System.Serializable]
public struct sopilla
{
    public string nombre_;
    public int nivel_;
    public double vidaMax_;
    public int atck_;
    public int def_;
    public int speed_;
    public int num_;
    public int tipo_;
};

public class EntrenadorScript : MonoBehaviour
{
    [Header("Managers")]
    public DialogueManager dialogueManager;
    private PlayerInput plIn;
    private bool jugadorEnRango = false;
    public GameObject player; // Referencia al objeto del personaje para desactivar el movimiento

    public string[] dialogoNPC;
    public sopilla SopillaRival1, SopillaRival2, SopillaRival3;
    public Sprite Imagen_Rival_Combate;

    private void Start()
    {
        plIn = GetComponent<PlayerInput>();
        plIn.actions.FindActionMap("ControlMundo").Enable();
        plIn.actions.FindActionMap("Combate").Disable();
        plIn.actions.FindActionMap("Dialogo").Disable();
    }

    void Update()
    {
        if (jugadorEnRango && plIn.actions["B"].ReadValue<float>() == 1)
        {
            InteractuarConNPC();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha entrado en rango del NPC.");
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha salido del rango del NPC.");
            jugadorEnRango = false;
        }
    }

    public void InteractuarConNPC()
    {
        // Desactivar el movimiento del personaje
        if (player != null)
        {
            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = false; // Desactivar movimiento
                plIn.actions.FindActionMap("ControlMundo").Disable();
                plIn.actions.FindActionMap("Dialogo").Enable();
                Debug.Log("Movimiento del personaje desactivado.");
            }
        }

        dialogueManager.StartDialogue(dialogoNPC, FinConversacion);

    }


    void FinConversacion()
    {
        if (player != null)
        {
            var movementScript = player.GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                movementScript.enabled = true; // Activar movimiento
                plIn.actions.FindActionMap("Dialogo").Disable();
                plIn.actions.FindActionMap("ControlMundo").Enable();
                if (SopillaRival1.nombre_ != "") Sopa.sopaRival[0] = new SopamonData(SopillaRival1.nombre_, SopillaRival1.nivel_, SopillaRival1.vidaMax_, SopillaRival1.atck_, SopillaRival1.def_, SopillaRival1.speed_, SopillaRival1.num_, false, true, SopillaRival1.tipo_);
                else Sopa.sopaRival[0] = null;
                if (SopillaRival2.nombre_ != "") Sopa.sopaRival[1] = new SopamonData(SopillaRival2.nombre_, SopillaRival2.nivel_, SopillaRival2.vidaMax_, SopillaRival2.atck_, SopillaRival2.def_, SopillaRival2.speed_, SopillaRival2.num_, false, true, SopillaRival2.tipo_);
                else Sopa.sopaRival[1] = null;
                if (SopillaRival3.nombre_ != "") Sopa.sopaRival[2] = new SopamonData(SopillaRival3.nombre_, SopillaRival3.nivel_, SopillaRival3.vidaMax_, SopillaRival3.atck_, SopillaRival3.def_, SopillaRival3.speed_, SopillaRival3.num_, false, true, SopillaRival3.tipo_);
                else Sopa.sopaRival[2] = null;
                Sopa.trainer = Imagen_Rival_Combate;
                GameManager.instance.currentPlayerData.Scene = SceneManager.GetActiveScene().name;
                GameManager.instance.currentPlayerData.posX = player.transform.position.x;
                GameManager.instance.currentPlayerData.posY = player.transform.position.y+0.1f;
                GameManager.instance.currentPlayerData.posZ = player.transform.position.z;
                SceneManager.LoadScene("PruebasCombate");
                Debug.Log("Movimiento del personaje activado.");
            }
        }
    }


}



    

    

    




    

