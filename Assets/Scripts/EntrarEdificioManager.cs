using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EntrarEdificioManager : MonoBehaviour
{
    public GameObject pantallaNegra; // Panel de transición para el cambio de escena
    public float duracionPantallaNegra = 2f; // Tiempo que permanece la pantalla negra
    public string escenaDestino; // Nombre de la escena a la que se cambiará
    public Vector3 nuevaPosicion; // Coordenadas destino del jugador tras el cambio de escena

    void OnTriggerEnter(Collider other)
    {
        // Detecta si el jugador entra en la zona de activación
        if (other.CompareTag("Player"))
        {
            // Guarda la nueva posición del jugador
            GameManager.instance.currentPlayerData.posX = nuevaPosicion.x;
            GameManager.instance.currentPlayerData.posY = nuevaPosicion.y;
            GameManager.instance.currentPlayerData.posZ = nuevaPosicion.z;

            // Inicia la transición con pantalla negra y el teletransporte
            StartCoroutine(PantallaNegraYTeletransporte());
        }
    }

    IEnumerator PantallaNegraYTeletransporte()
    {
        // Activa la pantalla negra para la transición
        pantallaNegra.SetActive(true);

        // Espera el tiempo de duración de la pantalla negra antes de cambiar de escena
        yield return new WaitForSeconds(duracionPantallaNegra);

        // Carga la nueva escena especificada
        SceneManager.LoadScene(escenaDestino);
    }
}