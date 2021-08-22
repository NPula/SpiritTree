using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController : PhysicsObject
{

    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float jumpPower = 1;

    [HideInInspector] public List<Interactable> Targets;

    [SerializeField] private CharacterCombat m_playerCombat;
    
    //Soul
    public int soulsCollected;
    public Text soulText;
    //Health
    public int health = 100;
    private int maxHealth = 100;
    public Image healthBar;
    private Vector2 healthBarOriginalSize;
    //Key
    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>();
    public Sprite keySprite;
    public Image inventoryItemImage;
    public Sprite inventroyItemblank;

    public enum DirectionFacing { RIGHT = 1, LEFT = -1}
    [HideInInspector] public DirectionFacing direction;

    private static CharacterController instance;
    public static CharacterController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<CharacterController>();
            return instance;
        }
    }

    void Start()
    {
        direction = DirectionFacing.RIGHT;
        healthBarOriginalSize = healthBar.rectTransform.sizeDelta;
        UpdateUI();
    }

    void Update()
    {

        targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * maxSpeed, 0);

        if (targetVelocity.x > 0)
        {
            direction = DirectionFacing.RIGHT;
        }
        else if (targetVelocity.x < 0)
        {
            direction = DirectionFacing.LEFT;
        }

        transform.localScale = new Vector3((int)direction, transform.localScale.y, transform.localScale.z);

        // if player presses "jump" and were grounded set velocity to jump power value 
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpPower;
        }

        if (Targets.Capacity > 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (Interactable i in Targets)
                {
                    i.Interact();
                }
            }
        }

        if (m_playerCombat != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Player Is Attacking");
                m_playerCombat.Attack();
            }
        }

        // UpdateUI();

        if (health <= 0)
        {
            Die();
        }

    }

    public void UpdateUI()
    {
        soulText.text = soulsCollected.ToString();

        healthBar.rectTransform.sizeDelta = new Vector2(healthBarOriginalSize.x * ((float)health / (float)maxHealth), healthBar.rectTransform.sizeDelta.y);

        

       // healthBar.rectTransform.sizeDelta 
    }

    public void AddInventoryItem(string inventoryName, Sprite Image)
    {
        inventory.Add(inventoryName, Image);
        inventoryItemImage.sprite = inventory[inventoryName];
    }

    public void RemoveInventoryItem(string inventoryName)
    {
        inventory.Remove(inventoryName);
        inventoryItemImage.sprite = inventroyItemblank;
    }

    public void Die()
    {
        SceneManager.LoadScene("AndrewSafeSpace");
    }


}

