using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    public GameObject inventoryScreen;
    public static bool isOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isOpen == false)
            {
                inventoryScreen.SetActive(true);
                isOpen = true;
            }

            else
            {
                inventoryScreen.SetActive(false);
                isOpen = false;
            }

        }
    }
}

