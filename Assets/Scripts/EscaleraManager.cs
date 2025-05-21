using System.Collections;
using UnityEngine;

public class EscaleraManager : MonoBehaviour
{
    public Transform posicionSubida;  // Posición destino cuando el jugador sube la escalera
    public Transform posicionBajada;  // Posición destino cuando el jugador baja la escalera
    public GameObject pantallaNegra;  // Canvas de transición con pantalla negra
    public float duracionPantallaNegra = 2f; // Tiempo que dura la pantalla negra antes de teletransportar al jugador

    public GameObject escaleraSubida; // Referencia a la escalera que permite subir
    public GameObject escaleraBajada; // Referencia a la escalera que permite bajar

    void OnTriggerEnter(Collider other)
    {
        // Detecta si el jugador entra en la zona de la escalera
        if (other.CompareTag("Player"))
        {
            // Determina si la escalera es de subida o bajada y mueve al jugador en consecuencia
            if (gameObject == escaleraSubida)
            {
                TeletransportarJugador(other.transform, posicionBajada);
            }
            else if (gameObject == escaleraBajada)
            {
                TeletransportarJugador(other.transform, posicionSubida);
            }
        }
    }

    void TeletransportarJugador(Transform jugador, Transform destino)
    {
        // Inicia la transición de pantalla negra antes de mover al jugador
        StartCoroutine(PantallaNegraYTeletransporte(jugador, destino));
    }

    IEnumerator PantallaNegraYTeletransporte(Transform jugador, Transform destino)
    {
        // Activa la pantalla negra para la transición
        pantallaNegra.SetActive(true);
        
        // Espera el tiempo definido antes de realizar el teletransporte
        yield return new WaitForSeconds(duracionPantallaNegra);

        // Mueve al jugador a la posición destino
        jugador.position = destino.position;

        // Desactiva la pantalla negra tras la transición
        pantallaNegra.SetActive(false);
    }
}