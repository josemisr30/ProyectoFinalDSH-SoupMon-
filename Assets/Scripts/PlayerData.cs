using System.Collections.Generic;
[System.Serializable]
public class PlayerData 
{
    public string playerName; // Nombre del jugador
    public string gender;     // Género del jugador
    public string Scene;      // Escena donde se encuentra el jugador
    public float posX;        // Posición X
    public float posY;        // Posición Y
    public float posZ;        // Posición Z
	public float posXC;        // Posición XC
    public float posYC;        // Posición YC
    public float posZC;        // Posición ZC
	public string ID;			  // Id del jugador
	public int dinero;			// Dinero del jugador
	
	
	public List<Item> objetos = new List<Item>(); //Lista de objetos
    public List<Item> balls = new List<Item>(); //Lista de balls
    public List<Item> objetosClave = new List<Item>(); //Lista de objetosClave
	
	public SopamonData[] claseSoupmon = new SopamonData[3]; // Solo 3 Soupmons permitidos
	
	public string Soupmon1;
	public string Soupmon2;
	public string Soupmon3;
}
