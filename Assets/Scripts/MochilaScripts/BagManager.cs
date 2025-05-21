using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BagManager : MonoBehaviour {
    // Listas públicas estáticas que contienen los distintos tipos de ítems
    static public List<Item> objetos = new List<Item>();
    static public List<Item> balls = new List<Item>();
    static public List<Item> objetosClave = new List<Item>();

    // Referencias al contenido del scroll y a los prefabs de los slots
    public Transform scrollContent;
    public GameObject slotPrefab;
	
	
    // Texto donde se muestra la descripción del ítem seleccionado
    public TMP_Text descripcionItem;

    // Lista de objetos gráficos (slots) mostrados actualmente
    static public List<GameObject> itemSlots = new List<GameObject>();

    // Referencias a los distintos canvas del inventario
    public GameObject canvasObjetos, canvasBalls, canvasObjetoClave, menuPrincipal;

    // Cursor gráfico que señala el objeto actualmente seleccionado
    public RectTransform cursor;

    // Índice actual del ítem seleccionado en la lista activa
    private int currentIndex = 0;

    // Lista que representa la categoría activa actualmente mostrada
    static private List<Item> activeInventory = new List<Item>();

    // Índice de categoría (0 = objetos, 1 = balls, 2 = objetos clave)
    private int categoriaIndex = 0;

    // Método llamado al inicio del juego
    void Start() {
        menuPrincipal.SetActive(false); // Desactiva el menú principal
        CambiarCategoria(0); // Inicia en la categoría de "Objetos"
    }

    // Método llamado al despertar el objeto, antes del Start
    void Awake() {
        // Recupera los datos del inventario desde el GameManager si existen
        if(GameManager.instance.currentPlayerData.objetos != null) {
            objetos = GameManager.instance.currentPlayerData.objetos;
        }
        else if(GameManager.instance.currentPlayerData.balls != null) {
            balls = GameManager.instance.currentPlayerData.balls;
        }
        else if(GameManager.instance.currentPlayerData.objetosClave != null) {
            objetosClave = GameManager.instance.currentPlayerData.objetosClave;
        }
    }

    // Cambia la categoría activa y actualiza los slots mostrados
    void CambiarCategoria(int nuevoIndex) {
        categoriaIndex = nuevoIndex;

        // Elimina todos los slots anteriores
        foreach (GameObject slot in itemSlots) {
            Destroy(slot);
        }
        itemSlots.Clear();

        // Asigna la lista activa en función de la categoría
        if (categoriaIndex == 0) {
            activeInventory = objetos.FindAll(item => item.quantity > 0);
            ActivarCanvas(canvasObjetos);
        } else if (categoriaIndex == 1) {
            activeInventory = balls.FindAll(item => item.quantity > 0);
            ActivarCanvas(canvasBalls);
        } else if (categoriaIndex == 2) {
            activeInventory = objetosClave.FindAll(item => item.quantity > 0);
            ActivarCanvas(canvasObjetoClave);
        }

        // Crea los nuevos slots según la lista activa
        foreach (Item item in activeInventory) {
            GameObject slot = Instantiate(slotPrefab, scrollContent);
            slot.GetComponent<Slot>().SetItem(item, descripcionItem);
            itemSlots.Add(slot);
        }

        // Reinicia el índice y actualiza la selección
        currentIndex = 0;
        UpdateSelection();
    }

    // Activa el canvas de la categoría seleccionada y desactiva los demás
    void ActivarCanvas(GameObject canvasActivo) {
        canvasObjetos.SetActive(canvasActivo == canvasObjetos);
        canvasBalls.SetActive(canvasActivo == canvasBalls);
        canvasObjetoClave.SetActive(canvasActivo == canvasObjetoClave);
    }

    // Lógica de actualización por frame
    void Update() {
        HandleBagNavigation(); // Navegación con teclado

        if (Input.GetKeyDown(KeyCode.Escape)) {
            CerrarMochila(); // Cierra la mochila si se pulsa Escape
        }
    }

    // Maneja la navegación dentro de la mochila
    void HandleBagNavigation() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentIndex = Mathf.Min(currentIndex + 1, itemSlots.Count - 1);
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            categoriaIndex = (categoriaIndex - 1 + 3) % 3;
            CambiarCategoria(categoriaIndex);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            categoriaIndex = (categoriaIndex + 1) % 3;
            CambiarCategoria(categoriaIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log($"Objeto seleccionado: {activeInventory[currentIndex].name}");
        }
    }

    // Actualiza la selección visual y la descripción del ítem
    void UpdateSelection() {
        if (activeInventory.Count > 0 && currentIndex >= 0 && currentIndex < activeInventory.Count) {
            descripcionItem.text = activeInventory[currentIndex].description;
            cursor.position = itemSlots[currentIndex].transform.position;
        } else {
            descripcionItem.text = "No hay objetos en esta categoría.";
        }
    }

    // Añade un objeto a la lista correspondiente
    public void AddItem(string nombreObjeto, string categoria) {
        List<Item> listaCategoria = ObtenerListaCategoria(categoria);
        Item itemExistente = listaCategoria.Find(item => item.name == nombreObjeto);

        if (itemExistente != null) {
            itemExistente.quantity++;
        } else {
            listaCategoria.Add(new Item { name = nombreObjeto, quantity = 1, description = "Descripción pendiente." });
        }

        CambiarCategoria(categoriaIndex); // Refresca la mochila si está abierta
        Debug.Log($"Se ha añadido {nombreObjeto}. Ahora tienes {itemExistente?.quantity ?? 1}.");
    }

    // Elimina un objeto (o reduce su cantidad)
    public void DeleteItem(string nombreObjeto, string categoria) {
        List<Item> listaCategoria = ObtenerListaCategoria(categoria);
        Item itemExistente = listaCategoria.Find(item => item.name == nombreObjeto);

        if (itemExistente != null) {
            itemExistente.quantity--;

            if (itemExistente.quantity <= 0) {
                listaCategoria.Remove(itemExistente);
                Debug.Log($"{nombreObjeto} eliminado de la mochila.");
            } else {
                Debug.Log($"Has usado {nombreObjeto}. Ahora tienes {itemExistente.quantity}.");
            }

            CambiarCategoria(categoriaIndex);

            // Limpia la descripción si ya no hay ítems
            if (activeInventory.Count == 0) {
                descripcionItem.text = "";
            }
        }
    }

    // Devuelve la lista correspondiente según la categoría indicada
    private List<Item> ObtenerListaCategoria(string categoria) {
        switch (categoria.ToLower()) {
            case "objetos": return objetos;
            case "balls": return balls;
            case "objetosclave": return objetosClave;
            default:
                Debug.LogError($"Categoría {categoria} no reconocida.");
                return new List<Item>(); // Retorna lista vacía para evitar errores
        }
    }

    // Devuelve la cantidad que se tiene de un objeto específico
    public int CuantoTengo(string nombreObjeto, string categoria) {
        List<Item> listaCategoria = ObtenerListaCategoria(categoria);
        Item itemExistente = listaCategoria.Find(item => item.name == nombreObjeto);
        if (itemExistente != null) {
            if (itemExistente.quantity > 0) {
                return itemExistente.quantity;
            }
            return 0;
        }
        return 0;
    }

    // Cierra la mochila y reactiva el menú principal
    void CerrarMochila() {
        menuPrincipal.SetActive(true); // Activa el menú principal
        gameObject.SetActive(false); // Desactiva la mochila
		PanelMenu.currentIndex = 0;
    }
}
