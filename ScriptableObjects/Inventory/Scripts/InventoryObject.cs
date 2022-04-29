using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory"),]

public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
    }

    void RemoveAnyOneXItem(ItemType type)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.type == type & Container[i].amount != 0)
            {
                Container[i].DecreaseItemCount();
                return;

            }
        }
    }
    void RemoveAnyOneXItemDesc(string descp)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.description == descp & Container[i].amount != 0)
            {
                Container[i].DecreaseItemCount();
                return;

            }
        }
    }

    public void RemoveOneDescpItem(string descp)
    {
        RemoveAnyOneXItemDesc(descp);
    }
    public void RemoveAnyOneSeedItem()
    {
        RemoveAnyOneXItem(ItemType.Seed);
    }
    public void RemoveOneWaterItem()
    {
        RemoveAnyOneXItem(ItemType.Water);
    }

    int GetItemXCount(ItemType type)
    {
        int count = 0;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.type == type)
            {
                count += Container[i].amount;
            }
        }
        return count;
    }
    int GetItemXDescCount(string descp)
    {
        int count = 0;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.description == descp)
            {
                count += Container[i].amount;
            }
        }
        return count;
    }
    public int GetSeedCount(string seed)
    {
        return GetItemXDescCount(seed);
    }
    public int GetWaterCount()
    {
        return GetItemXCount(ItemType.Water);
    }

}

[System.Serializable]
public class InventorySlot
{

    public ItemObject item;
    public int amount;
    /*
    public InventorySlot()
    {
        item = null;
        amount = 0;
    }*/
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
    public void UpdateSlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void DecreaseItemCount()
    {

        amount--;

    }
}
