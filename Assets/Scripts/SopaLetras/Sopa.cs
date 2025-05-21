using System.Collections.Generic;
using System.IO; // Necesario para trabajar con archivos
using System.Linq;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SopaImagenesClass
{
    public Sprite f, b;
}

public class Sopa : MonoBehaviour
{
    private PlayerInput plIn;
    public string filePath = "Assets/Scripts/SopaLetras/Palabras.json"; // Ruta del archivo
    private char[] abc = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'á', 'é', 'í', 'ó', 'ú', 'ü' };
    private const int nABC = 33;
    private Dictionary<string, string> palabras; // Diccionario para almacenar las claves y valores
    private const int nMatrix = 10; //Tamaño de la matriz
    private int nPalabras, combo = 0, estadoCombo = 0;
    private const int palabrasPorSopa = 3; //Número máximo de palabras por sopa
    private char[,] sopa; //Sopa de letras al uso
    private string actual, formando; //Palabra que se está buscando actualmente y la que se está formando
    private Stack<string> ocultas = new Stack<string>(); //Lista de las palabras ocultas en la sopa
    public TMP_Text texto, p1, p2, p3, p4, p5, p6, buscando, com, tiempoRestanteText;
    private bool b1 = true, b2 = true, b3 = true, b4 = true, b5 = true, b6 = true; //Comprobante de si se ha pulsado o no una opción de la sopa de letras
    private int menu; //Opción del menú que se está pulsando. 0: Menú, 1: ataque, 2: mochila, 3: sopamones, 4: huir.
    private Coroutine temporizadorCorrutina; // Para controlar la corrutina del temporizador
    public GameObject me, at, mo, so, es;
    static public SopamonData[] sopaRival = new SopamonData[3];
    static public SopamonData[] sopaPlayer = new SopamonData[3];
    [SerializeField] public SopaImagenesClass[] SopaImagenes;
    [SerializeField] public Sprite[] anim = new Sprite[5];
    public SpriteRenderer sopamon1, sopamon2, entrenador;
    public Slider sld1, sld2, vidaS1, vidaS2, expe;
    public TMP_Text nom1, nom2, cantP, cantS;
    public Image soOp1, soOp2, obj1, obj2, s1, s2, s3;
    public Sprite stick;
    static public Sprite trainer;

    void Start()
    {
        plIn = GetComponent<PlayerInput>();
        plIn.actions.FindActionMap("ControlMundo").Disable();
        plIn.actions.FindActionMap("Combate").Enable();
        menu = 0;
        entrenador.gameObject.SetActive(false);

        /*Borrar desde aquí
        sopaPlayer[0] = new SopamonData("Paco",2,20,7,80,10,0,false,false,2);
        sopaPlayer[1] = new SopamonData("Pepe",1,20,80,80,10,1,false,false,1);
        sopaRival[0] = new SopamonData("Jose",2,10,10,10,5,2,false,true,0);
        sopaRival[1] = new SopamonData("Pakito",4,35,10,10,5,3,false,true,0);
        Hasta aquí*/

        sopamon1.sprite = SopaImagenes[sopaPlayer[0].imagen()].b;
        sopamon2.sprite = SopaImagenes[sopaRival[0].imagen()].f;

        //Declaración y formación de la sopa de letras
        sopa = new char[nMatrix, nMatrix];


        // Verificar si el archivo existe
        if (File.Exists(filePath))
        {
            // Leer todo el contenido del archivo como texto
            string fileContent = File.ReadAllText(filePath);

            // Convertir el texto JSON a un diccionario
            palabras = ConvertJsonToDictionary(fileContent);

            // Mostrar las palabras en la consola
            /*foreach (var palabra in palabras)
            {
                Debug.Log($"Clave: {palabra.Key}, Palabra: {palabra.Value}");
            }*/

        }
        else
        {
            Debug.LogError("El archivo no existe en la ruta especificada: " + filePath);
        }

        nPalabras = palabras.Count;
    }

    private void rellenaSopa()
    {
        bool chachi = false;
        int t = palabrasPorSopa;
        HashSet<int> a = new HashSet<int>();

        combo = 0;
        estadoCombo = 0;

        for (int i = 0; i < nMatrix; i++)
        {
            for (int j = 0; j < nMatrix; j++)
            {
                sopa[i, j] = '?';
            }
        }

        while (a.Count <= t)
        {
            a.Add(UnityEngine.Random.Range(0, nPalabras));
        }

        while (t != 0)
        {
            while (!chachi)
            {
                int tipo = UnityEngine.Random.Range(0, 3), posX = UnityEngine.Random.Range(0, nMatrix), posY = UnityEngine.Random.Range(0, nMatrix);
                switch (tipo)
                {
                    case 0:
                        if (posX - 1 >= 0 && posX + 4 < nMatrix && sopa[posX - 1, posY] == '?' && sopa[posX, posY] == '?' && sopa[posX + 1, posY] == '?' && sopa[posX + 2, posY] == '?' && sopa[posX + 3, posY] == '?' && sopa[posX + 4, posY] == '?')
                        {
                            int cho = UnityEngine.Random.Range(0, 2);
                            if (cho == 0)
                            {
                                sopa[posX - 1, posY] = palabras[a.ElementAt(t - 1).ToString()][0];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX + 1, posY] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX + 2, posY] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX + 3, posY] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX + 4, posY] = palabras[a.ElementAt(t - 1).ToString()][5];
                            }
                            else
                            {
                                sopa[posX - 1, posY] = palabras[a.ElementAt(t - 1).ToString()][5];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX + 1, posY] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX + 2, posY] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX + 3, posY] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX + 4, posY] = palabras[a.ElementAt(t - 1).ToString()][0];
                            }
                            chachi = true;
                        }
                        break;
                    case 1:
                        if (posY - 1 >= 0 && posY + 4 < nMatrix && sopa[posX, posY - 1] == '?' && sopa[posX, posY] == '?' && sopa[posX, posY + 1] == '?' && sopa[posX, posY + 2] == '?' && sopa[posX, posY + 3] == '?' && sopa[posX, posY + 4] == '?')
                        {
                            int cho = UnityEngine.Random.Range(0, 2);
                            if (cho == 0)
                            {
                                sopa[posX, posY - 1] = palabras[a.ElementAt(t - 1).ToString()][0];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX, posY + 1] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX, posY + 2] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX, posY + 3] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX, posY + 4] = palabras[a.ElementAt(t - 1).ToString()][5];
                            }
                            else
                            {
                                sopa[posX, posY - 1] = palabras[a.ElementAt(t - 1).ToString()][5];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX, posY + 1] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX, posY + 2] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX, posY + 3] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX, posY + 4] = palabras[a.ElementAt(t - 1).ToString()][0];
                            }
                            chachi = true;
                        }
                        break;
                    case 2:
                        if (posX - 1 >= 0 && posX + 4 < nMatrix && posY - 1 >= 0 && posY + 4 < nMatrix && sopa[posX - 1, posY - 1] == '?' && sopa[posX, posY] == '?' && sopa[posX + 1, posY + 1] == '?' && sopa[posX + 2, posY + 2] == '?' && sopa[posX + 3, posY + 3] == '?' && sopa[posX + 4, posY + 4] == '?')
                        {
                            int cho = UnityEngine.Random.Range(0, 2);
                            if (cho == 0)
                            {
                                sopa[posX - 1, posY - 1] = palabras[a.ElementAt(t - 1).ToString()][0];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX + 1, posY + 1] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX + 2, posY + 2] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX + 3, posY + 3] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX + 4, posY + 4] = palabras[a.ElementAt(t - 1).ToString()][5];
                            }
                            else
                            {
                                sopa[posX - 1, posY - 1] = palabras[a.ElementAt(t - 1).ToString()][5];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX + 1, posY + 1] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX + 2, posY + 2] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX + 3, posY + 3] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX + 4, posY + 4] = palabras[a.ElementAt(t - 1).ToString()][0];
                            }
                            chachi = true;
                        }
                        break;
                    case 3:
                        if (posX - 1 >= 0 && posX + 4 < nMatrix && posY - 4 >= 0 && posY + 1 < nMatrix && sopa[posX - 1, posY + 1] == '?' && sopa[posX, posY] == '?' && sopa[posX + 1, posY - 1] == '?' && sopa[posX + 2, posY - 2] == '?' && sopa[posX + 3, posY - 3] == '?' && sopa[posX + 4, posY - 4] == '?')
                        {
                            int cho = UnityEngine.Random.Range(0, 2);
                            if (cho == 0)
                            {
                                sopa[posX - 1, posY + 1] = palabras[a.ElementAt(t - 1).ToString()][0];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX + 1, posY - 1] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX + 2, posY - 2] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX + 3, posY - 3] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX + 4, posY - 4] = palabras[a.ElementAt(t - 1).ToString()][5];
                            }
                            else
                            {
                                sopa[posX - 1, posY + 1] = palabras[a.ElementAt(t - 1).ToString()][5];
                                sopa[posX, posY] = palabras[a.ElementAt(t - 1).ToString()][4];
                                sopa[posX + 1, posY - 1] = palabras[a.ElementAt(t - 1).ToString()][3];
                                sopa[posX + 2, posY - 2] = palabras[a.ElementAt(t - 1).ToString()][2];
                                sopa[posX + 3, posY - 3] = palabras[a.ElementAt(t - 1).ToString()][1];
                                sopa[posX + 4, posY - 4] = palabras[a.ElementAt(t - 1).ToString()][0];
                            }
                            chachi = true;
                        }
                        break;
                }
                if (chachi) ocultas.Push(palabras[a.ElementAt(t - 1).ToString()]);
            }
            chachi = false;
            t--;
        }
        for (int i = 0; i < nMatrix; i++)
        {
            for (int j = 0; j < nMatrix; j++)
            {
                if (sopa[i, j] == '?') sopa[i, j] = abc[UnityEngine.Random.Range(0, nABC)];
            }
        }
    }

    private void mostrarSopa()
    {
        string s = "";
        for (int i = 0; i < nMatrix; i++)
        {
            for (int j = 0; j < nMatrix - 1; j++)
            {
                s += sopa[i, j] + " ";
            }
            if (i == (nMatrix - 1)) s += sopa[i, nMatrix - 1];
            else s += sopa[i, nMatrix - 1] + "\n";
        }
        texto.text = s;
    }

    private void mostrarOpciones()
    {
        char[] palabra = new char[6];
        string s = ocultas.Pop();
        actual = s;
        palabra[0] = s[0];
        palabra[1] = s[1];
        palabra[2] = s[2];
        palabra[3] = s[3];
        palabra[4] = s[4];
        palabra[5] = s[5];

        // Barajar el contenido del array palabra
        for (int i = palabra.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            // Intercambiar palabra[i] con palabra[j]
            char temp = palabra[i];
            palabra[i] = palabra[j];
            palabra[j] = temp;
        }

        // Mostrar opciones en la interfaz
        p1.text = palabra[0] + "";
        p2.text = palabra[1] + "";
        p3.text = palabra[2] + "";
        p4.text = palabra[3] + "";
        p5.text = palabra[4] + "";
        p6.text = palabra[5] + "";
    }

    // Método para convertir el JSON en un diccionario
    private Dictionary<string, string> ConvertJsonToDictionary(string json)
    {
        // Eliminar los caracteres innecesarios del JSON
        json = json.TrimStart('{').TrimEnd('}');

        // Crear un diccionario vacío
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        // Dividir el JSON en pares clave-valor
        string[] pairs = json.Split(',');

        foreach (string pair in pairs)
        {
            // Dividir cada par en clave y valor
            string[] keyValue = pair.Split(':');
            string key = keyValue[0].Trim().Trim('"'); // Eliminar espacios y comillas
            string value = keyValue[1].Trim().Trim('"'); // Eliminar espacios y comillas

            // Agregar al diccionario
            dictionary[key] = value;
        }

        return dictionary;
    }

    void lucha() //Aplica los resultados de una lucha
    {
        if (sopaPlayer[0].veloSopa() < sopaRival[0].veloSopa())
        {
            sopaPlayer[0].aplicaDano(sopaRival[0].calculaDano(Random.Range(1, 4), sopaPlayer[0].tipoSopa()));
            sopamon2.gameObject.transform.Translate(0, 0.5f, 0);
            if (sopaPlayer[0].vidaActual() > 0)
            {
                sopaRival[0].aplicaDano(sopaPlayer[0].calculaDano(combo, sopaRival[0].tipoSopa()));
                sopamon1.gameObject.transform.Translate(0, 0.5f, 0);
            }
        }
        else
        {
            sopaRival[0].aplicaDano(sopaPlayer[0].calculaDano(combo, sopaRival[0].tipoSopa()));
            sopamon1.gameObject.transform.Translate(0, 0.5f, 0);
            if (sopaRival[0].vidaActual() > 0)
            {
                sopaPlayer[0].aplicaDano(sopaRival[0].calculaDano(Random.Range(1, 4), sopaPlayer[0].tipoSopa()));
                sopamon2.gameObject.transform.Translate(0, 0.5f, 0);
            }
        }

        if (sopaPlayer[0].vidaActual() <= 0)
        {
            if (sopaPlayer[1] != null && sopaPlayer[1].vidaActual() > 0)
            {
                StartCoroutine(morir(sopamon1.gameObject));
                SopamonData aux = sopaPlayer[0];
                sopaPlayer[0] = sopaPlayer[1];
                sopaPlayer[1] = sopaPlayer[2];
                sopaPlayer[2] = aux;
                sopamon1.sprite = SopaImagenes[sopaPlayer[0].imagen()].b;
                sopamon1.gameObject.transform.position = new Vector3(sopamon1.gameObject.transform.position.x, 2, sopamon1.gameObject.transform.position.z);
            }
            else
            {
                Debug.Log("Gana el rival.");
                GameManager.instance.currentPlayerData.Scene = "CuartoProta";
                GameManager.instance.currentPlayerData.posX = -2;
                GameManager.instance.currentPlayerData.posY = 2;
                GameManager.instance.currentPlayerData.posZ = -2;
                if (sopaPlayer[0] != null) sopaPlayer[0].curar(sopaPlayer[0].vidaMaxSopa());
                if (sopaPlayer[1] != null) sopaPlayer[1].curar(sopaPlayer[1].vidaMaxSopa());
                if (sopaPlayer[2] != null) sopaPlayer[2].curar(sopaPlayer[2].vidaMaxSopa());
                SceneManager.LoadScene("CuartoProta");
            }

        }
        if (sopaRival[0].vidaActual() <= 0)
        {
            if (sopaRival[1] != null && sopaRival[1].vidaActual() > 0)
            {
                StartCoroutine(morir(sopamon2.gameObject));
                sopaPlayer[0].subirNivel(sopaRival[0].nvlSopa());
                SopamonData aux = sopaRival[0];
                sopaRival[0] = sopaRival[1];
                sopaRival[1] = sopaRival[2];
                sopaRival[2] = aux;
                sopamon2.sprite = SopaImagenes[sopaRival[0].imagen()].f;
                sopamon2.gameObject.transform.position = new Vector3(sopamon2.gameObject.transform.position.x, 2, sopamon2.gameObject.transform.position.z);
            }
            else
            {
                sopaPlayer[0].subirNivel(sopaRival[0].nvlSopa());
                sopamon2.gameObject.SetActive(false);
                Debug.Log("Has ganado.");
                SceneManager.LoadScene(GameManager.instance.currentPlayerData.Scene);
            }

        }
    }

    void atacando()
    {
        if (estadoCombo == palabrasPorSopa)
        {
            if (temporizadorCorrutina != null)
            {
                StopCoroutine(temporizadorCorrutina);
                temporizadorCorrutina = null;
                tiempoRestanteText.text = "";

            }
            
            lucha();

            plIn.actions.FindActionMap("Combate").Disable();
            menu = 0;
            at.SetActive(false);
            me.SetActive(true);
            StartCoroutine(ReactivarCombate());
        }
        else if (formando == actual)
        {
            if (temporizadorCorrutina != null)
            {
                StopCoroutine(temporizadorCorrutina);
                temporizadorCorrutina = null;
            }
            plIn.actions.FindActionMap("Combate").Disable();
            combo++;
            estadoCombo++;
            if (estadoCombo != palabrasPorSopa) mostrarOpciones();
            b1 = b2 = b3 = b4 = b5 = b6 = true;
            formando = "";

            temporizadorCorrutina = StartCoroutine(TemporizadorPalabra());

            StartCoroutine(ReactivarCombate());
        }

        if (plIn.actions["Reset"].ReadValue<float>() == 1)
        {
            formando = "";
            b1 = b2 = b3 = b4 = b5 = b6 = true;
        }
        if (b1 && plIn.actions["A"].ReadValue<float>() == 1)
        {
            b1 = false;
            formando += p1.text;
        }
        if (b2 && plIn.actions["B"].ReadValue<float>() == 1)
        {
            b2 = false;
            formando += p2.text;
        }
        if (b3 && plIn.actions["X"].ReadValue<float>() == 1)
        {
            b3 = false;
            formando += p3.text;
        }
        if (b4 && plIn.actions["Y"].ReadValue<float>() == 1)
        {
            b4 = false;
            formando += p4.text;
        }
        if (b5 && plIn.actions["L"].ReadValue<float>() == 1)
        {
            b5 = false;
            formando += p5.text;
        }
        if (b6 && plIn.actions["R"].ReadValue<float>() == 1)
        {
            b6 = false;
            formando += p6.text;
        }
        buscando.text = formando;
        com.text = 'X' + combo.ToString();
    }

    private void atrapar()
    {
        sopamon2.sprite = stick;
        if (100 - sopaRival[0].vidaVisual() * 100 + Random.Range(60, 100) > 150)
        {
            if (sopaPlayer[1] == null) sopaPlayer[1] = sopaRival[0];
            else
            {
                if (sopaPlayer[2] == null) sopaPlayer[2] = sopaRival[0];
                else
                {
                    menu = 2;
                }
            }
            if (menu != 2) SceneManager.LoadScene(GameManager.instance.currentPlayerData.Scene);
        }
        else
        {
            Invoke("escapo", 2f);
        }
    }

    private void escapo()
    {
        sopamon2.sprite = SopaImagenes[sopaRival[0].imagen()].f;
        StartCoroutine(PasarTurno());
    }

    private IEnumerator ReactivarCombate()
    {
        yield return new WaitForSeconds(0.2f);
        plIn.actions.FindActionMap("Combate").Enable();
    }

    private IEnumerator PasarTurno()
    {
        yield return new WaitForSeconds(1f);
        //auxVida = sopaRival[0].vidaActual();
        combo = 0;
        lucha();
        //sopaRival[0].forzarVida(auxVida);
        StartCoroutine(ReactivarCombate());
    }

    // Corrutina para el temporizador de 20 segundos
    private IEnumerator TemporizadorPalabra()
    {
        float tiempoRestante = 20f; // Tiempo inicial del contador

        while (tiempoRestante > 0)
        {
            // Actualizar el texto con el tiempo restante
            tiempoRestanteText.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString() + "s";

            // Esperar un frame y reducir el tiempo restante
            yield return null;
            tiempoRestante -= Time.deltaTime;
        }

        // Cuando el tiempo se agote, forzar la actualización de la palabra
        tiempoRestanteText.text = "Tiempo: 0s"; // Mostrar 0 cuando el tiempo se agote
        Debug.Log("Tiempo agotado. Actualizando palabra...");
        estadoCombo++;
        plIn.actions.FindActionMap("Combate").Disable();
        if (estadoCombo != palabrasPorSopa) mostrarOpciones();
        b1 = b2 = b3 = b4 = b5 = b6 = true;
        formando = "";

        StartCoroutine(ReactivarCombate());

        // Reiniciar el temporizador
        if (estadoCombo != palabrasPorSopa) temporizadorCorrutina = StartCoroutine(TemporizadorPalabra());
    }

    private IEnumerator morir(GameObject obj)
    {
        while (obj.transform.position.y > -2)
        {
            obj.transform.Translate(0, -1, 0);
        }
        yield return null;
    }

    void Update()
    {
        switch (menu)
        {
            case 0: //Menú
                if (plIn.actions["Y"].ReadValue<float>() == 1)
                {
                    plIn.actions.FindActionMap("Combate").Disable();
                    me.SetActive(false);
                    at.SetActive(true);
                    rellenaSopa();
                    mostrarSopa();
                    mostrarOpciones();
                    menu = 1;
                    StartCoroutine(ReactivarCombate());
                }
                if (plIn.actions["X"].ReadValue<float>() == 1 && plIn.actions["B"].ReadValue<float>() != 1)
                {
                    so.SetActive(true);
                    if (sopaPlayer[1] != null && sopaPlayer[1].vidaActual() > 0)
                    {
                        soOp1.gameObject.SetActive(true);
                        soOp1.sprite = SopaImagenes[sopaPlayer[1].imagen()].f;
                        vidaS1.value = (float)sopaPlayer[1].vidaVisual();
                    }
                    else soOp1.gameObject.SetActive(false);
                    if (sopaPlayer[2] != null && sopaPlayer[2].vidaActual() > 0)
                    {
                        soOp2.gameObject.SetActive(true);
                        soOp2.sprite = SopaImagenes[sopaPlayer[2].imagen()].f;
                        vidaS2.value = (float)sopaPlayer[2].vidaVisual();
                    }
                    else soOp2.gameObject.SetActive(false);


                    if (plIn.actions["L"].ReadValue<float>() == 1 && soOp1.gameObject.activeSelf)
                    {
                        plIn.actions.FindActionMap("Combate").Disable();
                        StartCoroutine(morir(sopamon1.gameObject));
                        SopamonData aux = sopaPlayer[0];
                        sopaPlayer[0] = sopaPlayer[1];
                        sopaPlayer[1] = aux;
                        sopamon1.sprite = SopaImagenes[sopaPlayer[0].imagen()].b;
                        sopamon1.gameObject.transform.position = new Vector3(sopamon1.gameObject.transform.position.x, 2, sopamon1.gameObject.transform.position.z);
                        StartCoroutine(ReactivarCombate());
                    }
                    if (plIn.actions["R"].ReadValue<float>() == 1 && soOp2.gameObject.activeSelf)
                    {
                        plIn.actions.FindActionMap("Combate").Disable();
                        StartCoroutine(morir(sopamon1.gameObject));
                        SopamonData aux = sopaPlayer[0];
                        sopaPlayer[0] = sopaPlayer[2];
                        sopaPlayer[2] = aux;
                        sopamon1.sprite = SopaImagenes[sopaPlayer[0].imagen()].b;
                        sopamon1.gameObject.transform.position = new Vector3(sopamon1.gameObject.transform.position.x, 2, sopamon1.gameObject.transform.position.z);
                        StartCoroutine(ReactivarCombate());
                    }
                }
                else so.SetActive(false);
                if (plIn.actions["A"].ReadValue<float>() == 1)
                {
                    plIn.actions.FindActionMap("Combate").Disable();
                    if (sopaPlayer[0].veloSopa() > sopaRival[0].veloSopa() && sopaRival[0].atrapable()) SceneManager.LoadScene(GameManager.instance.currentPlayerData.Scene);
                    else StartCoroutine(PasarTurno());
                }
                if (plIn.actions["B"].ReadValue<float>() == 1 && plIn.actions["X"].ReadValue<float>() != 1)
                {
                    mo.SetActive(true);
                    if (BagManager.objetos[0].quantity > 0)
                    {
                        obj1.gameObject.SetActive(true);
                        cantP.text = BagManager.objetos[0].quantity.ToString();
                    }
                    else obj1.gameObject.SetActive(false);

                    if (BagManager.balls[0].quantity > 0 && sopaRival[0].atrapable())
                    {
                        obj2.gameObject.SetActive(true);
                        cantS.text = BagManager.balls[0].quantity.ToString();
                    }
                    else obj2.gameObject.SetActive(false);

                    if (plIn.actions["L"].ReadValue<float>() == 1 && obj1.gameObject.activeSelf)
                    {
                        BagManager.objetos[0].quantity -= 1;
                        sopaPlayer[0].curar(20);

                        plIn.actions.FindActionMap("Combate").Disable();
                        mo.SetActive(false);

                        StartCoroutine(PasarTurno());
                    }

                    if (plIn.actions["R"].ReadValue<float>() == 1 && obj2.gameObject.activeSelf)
                    {
                        plIn.actions.FindActionMap("Combate").Disable();
                        BagManager.balls[0].quantity -= 1;
                        atrapar();
                    }

                }
                else mo.SetActive(false);
                break;
            case 1: //Atacando
                atacando();
                break;
            case 2:
                plIn.actions.FindActionMap("Combate").Enable();
                me.SetActive(false);
                es.SetActive(true);
                s1.sprite = SopaImagenes[sopaPlayer[0].imagen()].f;
                s2.sprite = SopaImagenes[sopaPlayer[1].imagen()].f;
                s3.sprite = SopaImagenes[sopaPlayer[2].imagen()].f;
                if (plIn.actions["Y"].ReadValue<float>() == 1)
                {
                    sopaPlayer[0] = sopaRival[0];
                    SceneManager.LoadScene(GameManager.instance.currentPlayerData.Scene);
                }
                if (plIn.actions["X"].ReadValue<float>() == 1)
                {
                    sopaPlayer[1] = sopaRival[0];
                    SceneManager.LoadScene(GameManager.instance.currentPlayerData.Scene);
                }
                if (plIn.actions["B"].ReadValue<float>() == 1)
                {
                    sopaPlayer[2] = sopaRival[0];
                    SceneManager.LoadScene(GameManager.instance.currentPlayerData.Scene);
                }
                break;
        }
        nom1.text = sopaPlayer[0].nombreSopa() + "  Nvl " + sopaPlayer[0].nvlSopa();
        nom2.text = "Nvl " + sopaRival[0].nvlSopa() + "  " + sopaRival[0].nombreSopa();
        sld1.value = (float)sopaPlayer[0].vidaVisual();
        sld2.value = (float)sopaRival[0].vidaVisual();
        expe.value = sopaPlayer[0].expBar();
        if (sopaRival[0] != null && sopaRival[0].enemigo())
        {
            entrenador.gameObject.SetActive(true);
            entrenador.sprite = trainer;
        }
        //Debug.Log(sopaPlayer[0].expSop());
        Debug.Log(sopaPlayer[0].expBar());
    }
}
