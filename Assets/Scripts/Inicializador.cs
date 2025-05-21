using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Inicializador : MonoBehaviour
{
	// Inicializa los datos del BagManager y del SoupmonManager contenidos en el GameManager
    void Start()
    {	
		// Inicializa todos los objetos con cantidad 0

		if (GameManager.instance.currentPlayerData.objetos == null)
		{
			BagManager.objetos.Add(new Item { name = "Poción", quantity = 0, description = "Restaura 20 PS." });
			BagManager.objetos.Add(new Item { name = "Repelente", quantity = 0, description = "Evita encuentros con Soupmons débiles." });
		}

		if (GameManager.instance.currentPlayerData.balls == null)
		{
			BagManager.balls.Add(new Item { name = "SoupStick", quantity = 0, description = "Palo que atrapa Soupmon." });
		}

		if (GameManager.instance.currentPlayerData.objetosClave == null)
		{
			BagManager.objetosClave.Add(new Item { name = "Nota del Profesor", quantity = 0, description = "Una carta con instrucciones." });
			BagManager.objetosClave.Add(new Item { name = "Zapatillas", quantity = 1, description = "Te permite correr más rápido." });
		}



		if (GameManager.instance.currentPlayerData.Soupmon1 != null)
		{
			string datos = GameManager.instance.currentPlayerData.Soupmon1.Trim(')'); // Quitamos el paréntesis final
			string[] valores = datos.Split(','); // Separamos los valores


			SopamonData nuevo = new SopamonData(
			valores[0].Trim().Replace("\"", ""), // Nombre (string)
			int.Parse(valores[1].Trim()),       // Nivel (int)
			int.Parse(valores[2].Trim()),       // Ataque (int)
			int.Parse(valores[3].Trim()),       // Defensa (int)
			int.Parse(valores[4].Trim()),       // Vida (int)
			int.Parse(valores[5].Trim()),       // Velocidad (int)
			int.Parse(valores[6].Trim()),       // Experiencia (int)
			bool.Parse(valores[7].Trim()),      // Legendario (bool)
			bool.Parse(valores[8].Trim()),      // Evolucionable (bool)
			int.Parse(valores[9].Trim())        // Tipo (int)
			);
			GameManager.instance.currentPlayerData.claseSoupmon[0] = nuevo;
			Sopa.sopaPlayer[0] = nuevo;
		}
	}
}
