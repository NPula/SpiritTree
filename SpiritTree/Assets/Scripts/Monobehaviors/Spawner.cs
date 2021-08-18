using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Item_SO[] itemsToSpawn;

    void Start()
    {
        foreach (Item_SO item in itemsToSpawn)
        {
            Instantiate(item.itemPrefab, (Vector2)item.itemPrefab.transform.position + Random.insideUnitCircle, Quaternion.identity);
        }
    }
}
