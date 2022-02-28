using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int x_space_between_items;
    public int number_of_column;
    public int y_space_between_items;
    public int x_start;
    public int y_start;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {

            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<Text>().text = inventory.Container[i].amount.ToString();
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(x_start + (x_space_between_items * (i % number_of_column)),
            y_start + (-y_space_between_items * (i / number_of_column)), 0f);
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<Text>().text = inventory.Container[i].amount.ToString();
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<Text>().text = inventory.Container[i].amount.ToString();
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }
    /*
    public void RemoveItem(GameObject item)
    {
        //If you already know what the index of the item is, skip this part
        int index = -1;
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (inventory.Container[i].item == item)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Debug.Log("Item not found in inventory");
            return;
        }

        item = inventory.Container[index].item; //If you don't already have the item reference (this example script does) then grab it now
        inventory.Container[index] = null; //Remove item from inventory
    }*/
}