using UnityEngine;
using TMPro;

public class SlotSoupMon : MonoBehaviour {
    public TMP_Text infoText; // Texto que muestra la info del SoupMon
    private SopamonData sopa;


    // Asignar Sopamon al slot
    public void SetSoupMon(SopamonData newSopa) {
        sopa = newSopa;

        // Mostrar datos clave: Nombre, Nivel, Exp, Vida actual/max
		infoText.text = $"{sopa.nombreSopa()} Nv {sopa.nvlSopa()} Exp {sopa.expSop()} Vida {(int)sopa.vidaActual()}/{(int)(sopa.vidaActual() / sopa.vidaVisual())}";
	}

}


