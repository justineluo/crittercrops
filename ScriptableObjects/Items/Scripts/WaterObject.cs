using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Water Object", menuName = "Inventory System/Items/Water")]
public class WaterObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Water;
    }

}
