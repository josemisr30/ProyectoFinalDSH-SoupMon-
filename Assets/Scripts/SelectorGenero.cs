using UnityEngine;
using UnityEngine.UI;

public class SelectorGenero : MonoBehaviour
{
    public Button chicoButton; // Referencia al botón de chico
    public Button chicaButton; // Referencia al botón de chica

    private Button currentButton; // Botón actualmente seleccionado
    private string selectedGender = "Chico"; // Género predeterminado al inicio

    void Start()
    {
        currentButton = chicoButton; // Empezamos seleccionando el botón de chico
        chicoButton.Select(); // Seleccionamos el botón al iniciar
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ToggleSelection(); // Cambia la selección al otro botón
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Enter para confirmar selección
        {
            PlayerPrefs.SetString("Aspect", selectedGender); // Guardar en PlayerPrefs
            currentButton.onClick.Invoke(); // Invocar el botón seleccionado
        }
    }

    void ToggleSelection()
    {
        if (currentButton == chicoButton)
        {
            currentButton = chicaButton;
            selectedGender = "Chica";
        }
        else
        {
            currentButton = chicoButton;
            selectedGender = "Chico";
        }

        currentButton.Select(); // Cambiar la selección visualmente
    }

    public string GetSelectedGender()
    {
        return selectedGender; // Retornar el género seleccionado
    }
}