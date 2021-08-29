using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField] private Item_SO m_itemSO;

    public override void Interact()
    {
        base.Interact();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerController.playerInventory != null)
            {
                playerController.playerInventory.AddItem(m_itemSO, 1);
            }

            Destroy(gameObject);
        }
    }
}
