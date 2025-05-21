using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SoupmonManager : MonoBehaviour
{
    public Transform scrollContent; // Donde se instancian los slots
    public GameObject slotPrefab;   // Prefab del SlotSoupMon
    public RectTransform cursor;    // Imagen del cursor
    public GameObject menuPrincipal;

    static public SopamonData[] soupArray = new SopamonData[3]; // Referencia al array claseSoupmon
    private List<GameObject> soupmonSlots = new List<GameObject>();
    private List<int> indexMap = new List<int>(); // Mapea los índices visuales a índices reales
    private int currentIndex = 0;
	
	void Start()
	{
		menuPrincipal.SetActive(false);

		// Asegurar que soupArray referencia `Sopa.sopaPlayer`
		soupArray = Sopa.sopaPlayer;

		// Crear slots solo si hay Soupmons en `sopaPlayer`
		for (int i = 0; i < soupArray.Length; i++)
		{
			if (soupArray[i] != null)
			{
				GameObject slot = Instantiate(slotPrefab, scrollContent);
				slot.GetComponent<SlotSoupMon>().SetSoupMon(soupArray[i]);
				soupmonSlots.Add(slot);
				indexMap.Add(i); // Guardar índice real
			}
		}

		UpdateSelection();
	}
	
    void Awake()
	{
		if (GameManager.instance.currentPlayerData.claseSoupmon != null)
		{
			
			
			soupArray = GameManager.instance.currentPlayerData.claseSoupmon; // Ahora usa directamente `sopaPlayer`
		}
		else
		{
			Debug.LogWarning("sopaPlayer no está inicializado correctamente.");
		}
	}
	


    void Update()
    {
        HandleSoupmonNavigation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarMenu();
        }
    }

    void HandleSoupmonNavigation()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = Mathf.Min(currentIndex + 1, soupmonSlots.Count - 1);
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            int realIndex = indexMap[currentIndex];
            Debug.Log($"Seleccionaste a: {soupArray[realIndex].nombreSopa()}");
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < soupmonSlots.Count; i++)
        {
            soupmonSlots[i].GetComponent<Image>().color = (i == currentIndex) ? Color.green : Color.white;
        }

        if (cursor != null && soupmonSlots.Count > 0)
        {
            cursor.position = soupmonSlots[currentIndex].transform.position;
        }
    }


	
    void CerrarMenu()
    {
        menuPrincipal.SetActive(true);
        gameObject.SetActive(false);
		PanelMenu.currentIndex = 0;
    }
}




//El soupmon es el numero de la imagen
