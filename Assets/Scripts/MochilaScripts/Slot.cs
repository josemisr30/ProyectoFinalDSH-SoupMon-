using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour {
    public TMP_Text itemText;  // Nombre y cantidad
    private Item item;  // Datos del objeto
    public TMP_Text informacionItem; // Descripción del objeto (Canvas)

    public void SetItem(Item newItem, TMP_Text descripcionBox) {
        item = newItem;
        informacionItem = descripcionBox;
        itemText.text = $"{item.name} x{item.quantity}";  // Mostrar nombre y cantidad
    }

    public void OnPointerEnter() {
        informacionItem.text = item.description;  // Mostrar descripción cuando el cursor pasa encima
    }

    public void OnPointerExit() {
        informacionItem.text = "";  // Vaciar descripción al salir
    }
}