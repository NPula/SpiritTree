using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public struct SlotItem
{
    //public string name;  // to display in UI
    public Item_SO item; // Item to store
    public int amount;   // Amount of item in slot
}

public class Inventory : MonoBehaviour
{
    public Item_SO tmpItem1;
    public Item_SO tmpItem2;
    public Item_SO tmpItem3;

    [SerializeField] private GameObject m_slotPrefab;
    [SerializeField] private int m_numOfSlots = 5;

    private Image[] m_itemImages;
    private SlotItem[] m_items;
    private GameObject[] inventorySlots;


    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = new Inventory();
        //}
        //else
        //{
        //    Destroy(this);
        //}
    }

    void Start()
    {
        m_itemImages = new Image[m_numOfSlots];
        m_items = new SlotItem[m_numOfSlots];
        inventorySlots = new GameObject[m_numOfSlots];

        CreateSlots();

        //TESTS:
        //AddItem(tmpItem1, 1);
        //AddItem(tmpItem2, 1);
        //AddItem(tmpItem3, 1);
        //AddItem(tmpItem1, 1);
        //AddItem(tmpItem3, 4);
    }

    private void Update()
    {
        /* TESTS
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddItem(tmpItem1, 1);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddItem(tmpItem2, 2);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddItem(tmpItem3, 1);
        }
        */
    }

    private void CreateSlots()
    {
        if (m_slotPrefab != null)
        {
            for (int i = 0; i < m_numOfSlots; i++)
            {
                GameObject newSlot = Instantiate(m_slotPrefab);
                newSlot.name = "ItemSlot_" + i;

                // Must set the second parameter to false so scaling is based on the parent not the world.
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform, false);

                inventorySlots[i] = newSlot;

                m_itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();

                m_items[i].amount = 0;
            }
        }
    }

    public bool AddItem(Item_SO item, int amount)
    {
        // NOTE: We should probably return all available slots in a list. So if we have some amount
        // of items in each slot for example:
        // {4, 0, 0} and then we add 4 more of the same item we would get {5, 3, 0}. As the slot
        // fills up we should handle moving to the next slot. We should probably return an array
        // or list of all available slots and fill each next available slot if there is one with
        // the remainder of the items. If we decide that all items in game are always added 1 at a
        // time (so items on ground shouldn't be stacked in one sprite but instead they would be
        // instantiated one at a time.) then this shouldn't be an issue. Workaround for now is simply
        // added all items one at a time. Not efficient but it works for now.

        for (int i = 0; i < amount; i++)
        {
            int index = GetNextAvailableSlot(item, 1);

            if (index >= 0 /*&& m_items[index].amount + amount <= item.maxAmountPerSlot*/)
            {
                m_items[index].item = item;
                //m_items[index].amount += amount;
                m_items[index].amount += 1; // Use this until function fixed. See NOTE above.

                Slot slotScript = inventorySlots[index].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.qtyText;
                quantityText.enabled = true;
                quantityText.text = m_items[index].amount.ToString();
            
                Sprite itemSprite = item.itemPrefab.GetComponent<SpriteRenderer>().sprite;
                m_itemImages[index].sprite = itemSprite;
                m_itemImages[index].enabled = true;

                return true;
            }
        }

        return false;
    }

    // Removes some amount of an item from first slot found with the item.
    void RemoveItem(Item_SO item, int amount)
    {
        List<int> indices = GetAllSlotsWithItem(item);
        int leftOver = 0;

        // If item in at least one slot.
        if (indices.Count > 0)
        {
            foreach (int index in indices)
            {
                leftOver = m_items[index].amount - amount;
                if (leftOver > 0)
                {
                    m_items[index].amount = leftOver;
                    break;
                }
                else
                {
                    m_items[index].amount = 0;
                    amount = Mathf.Abs(leftOver);
                }
            }
        }
    }

    // Return -1 if no slots are found.
    int GetNextAvailableSlot(Item_SO item, int amount)
    {
        List<int> indices = GetAllSlotsWithItem(item);

        // If item in at least one slot.
        if (indices.Count > 0)
        {
            foreach (int index in indices)
            {
                // if we havent reached max capacity for the item in its slot add item(s)
                if (m_items[index].item != null)
                {
                    if (m_items[index].amount + amount <= m_items[index].item.maxAmountPerSlot)
                    {
                        // return the index of the available slot.
                        return index;
                    }
                }
            }
        }

        // If the item isn't in the inventory or all slots with item
        // are full then find the next empty slot and return the index.
        return GetNextEmptySlot();
    }

    // Should return all available slots. So if we have some amount of items in each slot
    // {4, 0, 0} and then we add 4 more of the same item we would get {5, 3, 0}. As the slot
    // fills up we should handle moving to the next slot. We should probably return an array
    // or list of all available slots and fill each next available slot if there is one with
    // the remainder of the items. If we decide that all items in game are always added 1 at a
    // time (so items on ground shouldn't be stacked in one sprite but instead they would be
    // instantiated one at a time.) then this shouldnt be an issue.

    // Return -1 if no slots are found.
    /*List<int> GetAvailableSlots(Item_SO item, int amount)
    {
        List<int> indices = GetAllSlotsWithItem(item);

        // If item in at least one slot.
        if (indices.Count > 0)
        {
            foreach (int index in indices)
            {
                // if we havent reached max capacity for the item in its slot add item(s)
                if (m_items[index].item != null)
                {
                    if (m_items[index].amount + amount <= m_items[index].item.maxAmountPerSlot)
                    {
                        // return the index of the available slot.
                        return index;
                    }
                }
            }
        }

        // If the item isn't in the inventory or all slots with item
        // are full then find the next empty slot and return the index.
        return GetNextEmptySlot();
    } */

    // returns a list of all indices that contain some item.
    // the list will be empty if none found; 
    List<int> GetAllSlotsWithItem(Item_SO item)
    {
        List<int> indices = new List<int>();

        for (int i = 0; i < m_items.Length; i++)
        {
            if (m_items[i].item != null)
            {
                if (string.Compare(m_items[i].item.itemName, item.itemName) == 0)
                {
                    indices.Add(i);
                }
            }
        }

        return indices;
    }

    // returns the index of the next empty slot
    // returns -1 if all slots are full.
    int GetNextEmptySlot()
    {
        for (int i = 0; i < m_items.Length; i++)
        {
            if (m_items[i].item == null)
            {
                return i;
            }
        }

        return -1;
    }
}
