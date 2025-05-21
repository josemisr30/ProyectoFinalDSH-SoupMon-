using UnityEngine;

public class Cursor : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform[] options;   // Array de botones de la interfaz
    public RectTransform arrow;       // Indicador visual de la selección
    public float offsetX = -240f;     // Distancia horizontal entre la flecha y los botones

    static public int selectedIndex = 0; // Índice del botón seleccionado

    void Start() {
        // Asegúrate de que la flecha esté en la posición inicial
        UpdateArrow();
    }

    void Update() {
        // Flecha abajo
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            selectedIndex = (selectedIndex + 1) % options.Length;
            UpdateArrow();
        }

        // Flecha arriba
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
            UpdateArrow();
        }
    }

    void UpdateArrow() {
        // Mueve la flecha a la posición del botón seleccionado
        var target = options[selectedIndex];
        arrow.position = new Vector3(
            target.position.x + offsetX,
            target.position.y,
            target.position.z
        );
    }

    // Llamar desde fuera para reiniciar el índice al abrir el menú
    public void ResetCursor() 
	{
        selectedIndex = 0;
        UpdateArrow();
    }
}
