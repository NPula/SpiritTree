using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/New Weapon", order = 2)]
public class Weapon_SO : Item_SO
{
    public float damage;

    public override void Use()
    {
        base.Use();
    }
}
