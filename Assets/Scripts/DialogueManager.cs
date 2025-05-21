using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text messageText; // Referencia al texto del diálogo
    public GameObject dialoguePanel; // Panel donde se muestra el diálogo
    public AudioSource textSound; // Sonido que se reproduce al escribir cada letra
    public float textSpeed = 0.05f; // Velocidad con la que aparece el texto en pantalla

    private string[] lines; // Almacena las líneas de diálogo
    private int index; // Índice de la línea actual en el diálogo
    private bool isTyping = false; // Indica si el texto se está escribiendo actualmente
    private System.Action onDialogueComplete; // Acción a ejecutar cuando el diálogo termina
    private PlayerInput plIn;

    public void StartDialogue(string[] dialogueLines, System.Action callback)
    {
        // Activa el panel de diálogo y carga las líneas de conversación
        dialoguePanel.SetActive(true);
        lines = dialogueLines;
        index = 0;
        onDialogueComplete = callback;
        StartCoroutine(TypeText()); // Inicia la animación de escritura del texto
    }

    public void NextLine()
    {
        // Si el texto aún se está escribiendo, detiene la animación y lo muestra completo
        if (isTyping)
        {
            StopAllCoroutines();
            messageText.text = lines[index];
            isTyping = false;
        }
        else
        {
            // Avanza a la siguiente línea de diálogo si quedan más líneas
            index++;
            if (index < lines.Length)
            {
                StartCoroutine(TypeText()); // Escribe la siguiente línea
            }
            else
            {
                // Oculta el panel de diálogo y ejecuta la acción final
                dialoguePanel.SetActive(false);
                onDialogueComplete?.Invoke();
            }
        }
    }

    IEnumerator TypeText()
    {
        // Reinicia el texto antes de escribirlo
        isTyping = true;
        messageText.text = "";

        // Escribe cada letra del diálogo de forma progresiva
        foreach (char letter in lines[index])
        {
            messageText.text += letter;
            textSound.Play(); // Reproduce un sonido con cada letra escrita
            yield return new WaitForSeconds(textSpeed); // Espera antes de escribir la siguiente letra
        }

        isTyping = false;
    }

    private void Start()
    {
        plIn = GetComponent<PlayerInput>();
        plIn.actions.FindActionMap("ControlMundo").Disable();
        plIn.actions.FindActionMap("Combate").Disable();
        plIn.actions.FindActionMap("Dialogo").Enable();
    }

    private void Update()
    {
        // Permite avanzar el diálogo presionando la tecla ESPACIO
        if (dialoguePanel.activeSelf && plIn.actions["A"].ReadValue<float>() == 1)
        {
            plIn.actions.FindActionMap("Dialogo").Disable();
            NextLine();
            StartCoroutine(ReactivarDialogo());
        }
    }

    private IEnumerator ReactivarDialogo()
    {
        yield return new WaitForSeconds(0.2f);
        plIn.actions.FindActionMap("Dialogo").Enable();
    }
}