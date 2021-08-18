using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/New Item", order = 1)]
public class Item_SO : ScriptableObject
{
    //public enum ItemType
    //{
    //    Weapon,
    //    Armor,
    //    Food,
    //    Junk,
    //    Quest
    //}

    //public ItemType type;
    public GameObject itemPrefab;
    public string itemName;

    public int maxAmountPerSlot;

    public virtual void Use()
    {

    }
}
