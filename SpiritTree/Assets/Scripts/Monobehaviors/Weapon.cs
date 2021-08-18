using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable
{
    //private Inventory playerInventory;
    //[SerializeField] private Weapon_SO so;

    protected override void Start()
    {
        base.Start();
        //playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Interact()
    {
        base.Interact();

        //Inventory.instance.AddItem(so, 1);
        
        Destroy(gameObject);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();   
    }
}
