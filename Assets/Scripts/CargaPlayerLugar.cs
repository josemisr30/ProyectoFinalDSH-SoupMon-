using UnityEngine;

public class CargaPlayerLugar : MonoBehaviour
{
	// Recoloca al Jugador en la escena de forma automatica
	static public bool muelto = false; // Booleano para saber si el jugador pierde un combate
	void Start()
	{
		if (GameManager.instance != null && GameManager.instance.currentPlayerData != null)
		{
			Vector3 pos = new Vector3(
				GameManager.instance.currentPlayerData.posX,
				GameManager.instance.currentPlayerData.posY,
				GameManager.instance.currentPlayerData.posZ
			);

			transform.position = pos;
			Debug.Log("Jugador reposicionado en Start() a: " + pos);
		}
		else if (muelto) //Si pierde un combate se cambia a la posicion donde curo la ultima vez
		{
			muelto = false;
			Vector3 pos = new Vector3(
				GameManager.instance.currentPlayerData.posXC,
				GameManager.instance.currentPlayerData.posYC,
				GameManager.instance.currentPlayerData.posZC
			);

			transform.position = pos;
			Debug.Log("Jugador reposicionado en Start() a: " + pos);
		}
	}

}
