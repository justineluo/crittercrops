using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seed Object", menuName = "Inventory System/Items/Seed")]
public class SeedObject : ItemObject
{
    public GameObject plantPrefab;
    public void Awake()

    {
        type = ItemType.Seed;
    }
}
