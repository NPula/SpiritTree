using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public struct Slot
{
    public string name;  // to display in UI
    public Item_SO item; // Item to store
    public int amount;   // Amount of item in slot
}

public class Inventory : MonoBehaviour
{
    static Inventory instance;

    private Slot[] inventory;
    public GameObject[] inventorySlots;
    private GameObject[] m_inventorySlots;
    public GameObject inventoryContainer;

    [SerializeField] private float m_padding; // space around each inventory slot.
    [SerializeField] private int m_amountOfSlotsHorizontal;  
    [SerializeField] private int m_amountOfSlotsVertical;
    [SerializeField] private GameObject m_slotPrefab;

    private Vector2 m_invSize;     // inventory size
    private Vector2 m_invPosition; // anchored position of the inventory.
    private Vector2 m_slotSize;    // size of each inventory slot.
    private int m_numberOfSlots;

    private CharacterStats m_stats;

    private void Awake()
    {
        if (instance == null)
        {
            instance = new Inventory();
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        // Get the players stats script for euipping items
        m_stats = GetComponent<CharacterStats>();

        // Store number of inventory slots at game start.
        m_numberOfSlots = m_amountOfSlotsHorizontal * m_amountOfSlotsVertical;

        // Add slots to inventory
        //CreateInventory();
        CreateInventory2();

        // Initialize inventory array 
        SetupInventory();

        // Update the inventory for the first time
        UpdateInventory();
    }

    private void SetupInventory()
    {
        inventory = new Slot[m_numberOfSlots];
        for (int i = 0; i < m_numberOfSlots; i++)
        {
            inventory[i].item = null;
            inventory[i].amount = 0;
            m_inventorySlots[i].transform.Find("Text").GetComponent<Text>().text = inventory[i].amount.ToString();
        }
    }

    void CreateInventory2()
    {
        m_inventorySlots = new GameObject[m_amountOfSlotsHorizontal * m_amountOfSlotsVertical];

        RectTransform invRect = inventoryContainer.GetComponent<RectTransform>();
        RectTransform invSlot = m_slotPrefab.GetComponent<RectTransform>();

        //m_invPosition = invRect.transform.position;
        m_invPosition = invRect.anchoredPosition;
        m_slotSize = invSlot.sizeDelta;

        // The amount of times to add padding is == amount of slots + 1.
        float extraSizeX = (m_padding * m_amountOfSlotsHorizontal) + m_padding;
        float extraSizeY = (m_padding * m_amountOfSlotsVertical) + m_padding;

        // Set the size of the inventory.
        m_invSize = invRect.sizeDelta = new Vector2(m_slotSize.x * m_amountOfSlotsHorizontal + extraSizeX, m_slotSize.y * m_amountOfSlotsVertical + extraSizeY);
        Debug.Log("Inventory Size: " + m_invSize);

        int k = 0; // For indexing into m_inventorySlots array

        // Loop through all slots to be setup
        for (int i = 0; i < m_amountOfSlotsHorizontal; i++)
        {
            for (int j = 0; j < m_amountOfSlotsVertical; j++)
            {
                // Slot position in the inventory panel. // '-j' flips the y pos since we want to decrease it here.
                //float posOfSlotX = (m_invSize.x / m_amountOfSlotsHorizontal * i);// + m_padding;
                //float posOfSlotY = (m_invSize.y / m_amountOfSlotsVertical * -j);// - m_padding; 
                float posOfSlotX = (m_slotSize.x * i + (m_padding * 2 * (i + 1)));// + m_padding;
                float posOfSlotY = (m_slotSize.y * -j - (m_padding * 2 * (j + 1)));// - m_padding; 
                Vector2 newPosition = new Vector2(posOfSlotX, posOfSlotY);

                //Debug.Log("slot pos: " + k + " : "+ posOfSlotX);

                // Instantiate the next slot needed
                GameObject obj = Instantiate(m_slotPrefab, Vector2.zero, Quaternion.identity);

                obj.GetComponent<Button>().onClick.AddListener(Use);

                // Set the parent of the new slot to the inventory panel object
                // Because a UI Panel is the parent, the coords are scoped within the panel.
                obj.transform.SetParent(inventoryContainer.transform);

                // Finally set the position of each slot.
                obj.GetComponent<RectTransform>().anchoredPosition = newPosition;
                m_inventorySlots[k] = obj;
                k++;
            }
        }
    }

    #region OLD CODE
    void CreateInventory()
    {
        m_inventorySlots = new GameObject[m_amountOfSlotsHorizontal * m_amountOfSlotsVertical];

        RectTransform invRect = inventoryContainer.GetComponent<RectTransform>();
        RectTransform invSlot = m_slotPrefab.GetComponent<RectTransform>();

        //m_invPosition = invRect.transform.position;
        m_invPosition = invRect.anchoredPosition;
        m_slotSize = invSlot.sizeDelta;

        // The amount of times to add padding is == amount of slots + 1.
        float extraSizeX = (m_padding * m_amountOfSlotsHorizontal) + m_padding;
        float extraSizeY = (m_padding * m_amountOfSlotsVertical) + m_padding;

        // Set the size of the inventory.
        m_invSize = invRect.sizeDelta = new Vector2(m_slotSize.x * m_amountOfSlotsHorizontal + extraSizeX, m_slotSize.y * m_amountOfSlotsVertical + extraSizeY);
        Debug.Log("Inventory Size: " + m_invSize);

        int k = 0; // For indexing into m_inventorySlots array

        // Loop through all slots to be setup
        for (int i = 0; i < m_amountOfSlotsHorizontal; i++)
        {
            for (int j = 0; j < m_amountOfSlotsVertical; j++)
            {
                // Slot position in the inventory panel. // '-j' flips the y pos since we want to decrease it here.
                //float posOfSlotX = (m_invSize.x / m_amountOfSlotsHorizontal * i);// + m_padding;
                //float posOfSlotY = (m_invSize.y / m_amountOfSlotsVertical * -j);// - m_padding; 
                float posOfSlotX = (m_slotSize.x * i + (m_padding*2 * (i+1)));// + m_padding;
                float posOfSlotY = (m_slotSize.y * -j - (m_padding*2 * (j+1)));// - m_padding; 
                Vector2 newPosition = new Vector2(posOfSlotX, posOfSlotY);

                //Debug.Log("slot pos: " + k + " : "+ posOfSlotX);

                // Instantiate the next slot needed
                GameObject obj = Instantiate(m_slotPrefab, Vector2.zero, Quaternion.identity);
               
                obj.GetComponent<Button>().onClick.AddListener(Use);

                // Set the parent of the new slot to the inventory panel object
                // Because a UI Panel is the parent, the coords are scoped within the panel.
                obj.transform.SetParent(inventoryContainer.transform);
                
                // Finally set the position of each slot.
                obj.GetComponent<RectTransform>().anchoredPosition = newPosition;
                m_inventorySlots[k] = obj;
                k++;
            }
        }
    }

    #endregion

    void Update()
    {
        foreach (Slot slot in inventory)
        {
            if (slot.item != null)
            {
                //Debug.Log("Inventory: " + slot.item.itemName);
            }
        }
    }

    void Use()
    {
        string itemName = EventSystem.current.currentSelectedGameObject.transform.Find("Name").GetComponent<Text>().text;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item.itemName.Remove(3, inventory[i].item.itemName.Length - 1 - 4) == itemName)
            {
                m_stats.EquipWeapon((Weapon_SO)inventory[i].item);
                inventory[i].amount--;
                if (inventory[i].amount <= 0)
                {
                    inventory[i].item = null;
                }

                UpdateInventory();
                break;
            }
        }
        //m_stats.EquipWeapon(inventory[]); 
    }

    public bool AddItem(Item_SO item, int amount)
    {
        int index = GetNextAvailableSlot(item, amount);
        if (index >= 0)
        {
            inventory[index].item = item;
            inventory[index].amount += amount;

            UpdateInventory();
            return true;
        }

        return false;
    }

    void UpdateInventory()
    {
        for (int i = 0; i < m_numberOfSlots; i++)
        {
            if (inventory[i].item == null)
            {
                m_inventorySlots[i].transform.Find("Name").GetComponent<Text>().text = "none";
                m_inventorySlots[i].transform.Find("Text").GetComponent<Text>().text = "0";
            }
            else
            {
                // TODO: This name needs to be fixed later.
                m_inventorySlots[i].transform.Find("Name").GetComponent<Text>().text = inventory[i].item.itemName.Remove(3, inventory[i].item.itemName.Length-1-4);
                m_inventorySlots[i].transform.Find("Text").GetComponent<Text>().text = inventory[i].amount.ToString();
            }
        }
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
                leftOver = inventory[index].amount - amount;
                if (leftOver > 0)
                {
                    inventory[index].amount = leftOver;
                    break;
                }
                else
                {
                    inventory[index].amount = 0;
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
                if (inventory[index].item != null)
                {
                    if (inventory[index].amount + amount <= inventory[index].item.maxAmountPerSlot)
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

    // returns a list of all indices that contain some item.
    // the list will be empty if none found; 
    List<int> GetAllSlotsWithItem(Item_SO item)
    {
        List<int> indices = new List<int>();

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item != null)
            {
                if (string.Compare(inventory[i].item.itemName, item.itemName) == 0)
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
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item == null)
            {
                return i;
            }
        }

        return -1;
    }
}
