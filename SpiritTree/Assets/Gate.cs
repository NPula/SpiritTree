using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string requiredInventoryItemString;
    public GameObject keyRequiredText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {

            if (GameObject.Find("Player").GetComponent<CharacterController>().inventory.ContainsKey(requiredInventoryItemString))
            {
                GameObject.Find("Player").GetComponent<CharacterController>().RemoveInventoryItem(requiredInventoryItemString);
                Destroy(gameObject);
                keyRequiredText.SetActive(false);
            }

            else
            {
                keyRequiredText.SetActive(true);
            }
        }
    }

}
