using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : Interactable
{

    enum ItemType {soul, health, key}
    [SerializeField] private ItemType itemType;

    [SerializeField] private string inventoryStringName;
    [SerializeField] private Sprite inventorySprite;

    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();

    [SerializeField] private float m_moveTowardsEntitySpeed = 1f;
    [SerializeField] private float m_collectDistance = 1f;

    private Vector3 velocity = Vector3.zero;

    public override void Interact()
    {
        base.Interact();

        Vector3 position = Vector3.SmoothDamp(transform.position, playerController.transform.position,ref velocity, m_moveTowardsEntitySpeed);
        transform.position = position;

        if (IsPlayerInRadius(playerController.transform, m_collectDistance))
        {
            CollectObject();
        }
    }

    private void CollectObject()
    {
        if (itemType == ItemType.soul)
        {
            playerController.soulsCollected += 1;
        }
        else if (itemType == ItemType.health)
        {
            if (playerController.health < 100)
            {
                playerController.health += 25;
            }

        }
        else if (itemType == ItemType.key)
        {
            playerController.AddInventoryItem(inventoryStringName, inventorySprite);
        }

        playerController.UpdateUI();
        Destroy(gameObject);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger: " + collision.tag);
        if (collision.CompareTag("Player"))
        {
            if (itemType == ItemType.soul)
            {
                playerController.soulsCollected += 1;
            }
            else if (itemType == ItemType.health)
            {
                if(playerController.health < 100) 
                {
                    playerController.health += 25;
                }
                
            }
            else if (itemType == ItemType.key)
            {
                playerController.AddInventoryItem(inventoryStringName, inventorySprite);
            }
            else
            {

            }

            playerController.UpdateUI();
            Destroy(gameObject);
        }
        
    }
    */
}
