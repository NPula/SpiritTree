using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{

    enum ItemType {soul, health, other}
    [SerializeField] private ItemType itemType;

    // Start is called before the first frame update
    void Start()
    {
        
        if (itemType == ItemType.soul)
        {
            Debug.Log("Im a Soul");
        }
        else if(itemType == ItemType.health)
        {
            Debug.Log("Im Health");
        }
        else if (itemType == ItemType.other)
        {
            Debug.Log("Im other");
        }
        else
        {
            Debug.Log("Im none of them");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<CharacterController>().soulsCollected += 1;
            GameObject.Find("Player").GetComponent<CharacterController>().UpdateUI();
            Destroy(gameObject);

        }
    }
}
