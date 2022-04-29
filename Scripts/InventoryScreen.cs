using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    public GameObject inventoryScreen;
    public GameObject waterInventory;
    public static bool isOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryScreen == null)
        {
            inventoryScreen = GameObject.FindGameObjectWithTag("InventoryScreen");
        }
        if (waterInventory == null)
        {
            waterInventory = GameObject.FindGameObjectWithTag("WaterInventory");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isOpen == false)
            {
                inventoryScreen.SetActive(true);
                waterInventory.SetActive(true);
                isOpen = true;
            }

            else
            {
                inventoryScreen.SetActive(false);
                waterInventory.SetActive(false);
                isOpen = false;
            }

        }
    }
}

