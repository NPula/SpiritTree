using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{

    enum ItemType {soul, health, key}
    [SerializeField] private ItemType itemType;

    PlayerController playerController;

    [SerializeField] private string inventoryStringName;
    [SerializeField] private Sprite inventorySprite;

    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();

    // Start is called before the first frame update
    void Start()
    {

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        //playerController.Controller.onTriggerEnterEvent += OnPlayerCollision;

    }

    // Update is called once per frame
    void Update()
    {
    }

    /*
    private void OnPlayerCollision(Collider2D collision)
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
        else
        {

        }

        playerController.UpdateUI();
        Destroy(gameObject);  
    } */

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



}
