using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{

    enum ItemType {soul, health, key}
    [SerializeField] private ItemType itemType;

    CharacterController characterController;

    [SerializeField] private string inventoryStringName;
    [SerializeField] private Sprite inventorySprite;

    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();

    // Start is called before the first frame update
    void Start()
    {
    
        characterController = GameObject.Find("Player").GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (itemType == ItemType.soul)
            {
                characterController.soulsCollected += 1;
            }
            else if (itemType == ItemType.health)
            {
                if(characterController.health < 100) 
                {
                    characterController.health += 25;
                }
                
            }
            else if (itemType == ItemType.key)
            {
                characterController.AddInventoryItem(inventoryStringName, inventorySprite);
            }
            else
            {

            }

            characterController.UpdateUI();
            Destroy(gameObject);

        }
    }



}
