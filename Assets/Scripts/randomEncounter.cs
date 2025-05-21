using UnityEngine;
using UnityEngine.SceneManagement;

public class randomEncounter : MonoBehaviour
{
    private SopamonData[] sopamons = new SopamonData[18];

    void Start()
    {
        sopamons[0] = new SopamonData("Sustosano", 2, 12, 5, 7, 10, 2, true, false, 1);
        sopamons[1] = new SopamonData("Oscutón", 3, 10, 7, 5, 12, 0, true, false, 2);
        sopamons[2] = new SopamonData("Oscurroedor", 8, 4, 7, 5, 12, 1, true, false, 2);
        sopamons[3] = new SopamonData("Mospectro", 3, 12, 7, 5, 13, 3, true, false, 2);
        sopamons[4] = new SopamonData("Arbusti", 3, 10, 7, 5, 10, 7, true, false, 1);
        sopamons[5] = new SopamonData("Bolsaconda", 3, 7, 7, 5, 9, 8, true, false, 2);
        sopamons[6] = new SopamonData("Flanbuesa", 3, 6, 6, 9, 4, 9, true, false, 0);
        sopamons[7] = new SopamonData("Huerminator", 3, 7, 8, 5, 9, 10, true, false, 0);
        sopamons[8] = new SopamonData("Hueniquilador", 5, 9, 10, 8, 11, 11, true, false, 0);
        sopamons[9] = new SopamonData("Torcaliza", 3, 13, 10, 11, 16, 12, true, false, 2);
        sopamons[10] = new SopamonData("Calabumza", 3, 12, 7, 7, 8, 13, true, false, 1);
        sopamons[11] = new SopamonData("Peztinho", 3, 11, 5, 6, 13, 14, true, false, 0);
        sopamons[12] = new SopamonData("Pulpátano", 4, 13, 8, 5, 9, 15, true, false, 0);
        sopamons[13] = new SopamonData("Antonio", 7, 10, 7, 5, 15, 16, true, false, 2);
        sopamons[14] = new SopamonData("Esteban", 7, 10, 7, 5, 15, 17, true, false, 1);
        sopamons[15] = new SopamonData("Isidoro", 7, 10, 7, 5, 15, 18, true, false, 2);
        sopamons[16] = new SopamonData("Oberto", 7, 10, 7, 5, 15, 19, true, false, 2);
        sopamons[17] = new SopamonData("Ubaldo", 7, 10, 7, 5, 15, 20, true, false, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Random.Range(0, 100) > 75)
            {
                Sopa.sopaRival[0] = null;
                Sopa.sopaRival[1] = null;
                Sopa.sopaRival[2] = null;
                Sopa.sopaRival[0] = sopamons[Random.Range(0, sopamons.Length)];
                GameManager.instance.currentPlayerData.Scene = SceneManager.GetActiveScene().name;
                GameManager.instance.currentPlayerData.posX = transform.position.x;
                GameManager.instance.currentPlayerData.posY = 1;
                GameManager.instance.currentPlayerData.posZ = transform.position.z;
                SceneManager.LoadScene("PruebasCombate");
            }
        }
    }
}
