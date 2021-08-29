using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D m_controller; // Making this public for now.
    
    // private Animator m_animator;
    // private RaycastHit2D m_lastControllerColliderHit;
    private Vector3 m_velocity;

    public CharacterController2D Controller
    {
        get
        {
            return m_controller;
        }

        set
        {
            m_controller = value;
        }
    }


    [HideInInspector]
    public List<Interactable> Targets;

    [SerializeField]
    private CharacterCombat m_playerCombat;

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

    public Inventory playerInventory;

    private bool hasControl = true;

    void Awake()
    {
        Controller = GetComponent<CharacterController2D>();

        #region Events Hidden Here
        // listen to some events. (Leaving this here in case we need events later. Probably will)
        //_controller.onControllerCollidedEvent += onControllerCollider;
        //_controller.onTriggerEnterEvent += onTriggerEnterEvent;
        //_controller.onTriggerExitEvent += onTriggerExitEvent;
        #endregion
    }

    void Start()
    {
        healthBarOriginalSize = healthBar.rectTransform.sizeDelta;
        UpdateUI();
    }

    void Update()
    {
        if (hasControl)
        {
            GetInputAndSetVelocity();
            //MovePlayer();
        }
        MovePlayer();

        if (Targets.Capacity > 0)
        {
            foreach (Interactable i in Targets)
            {
                i.Interact();
            }
        }

        if (m_playerCombat != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_playerCombat.Attack();
            }
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void UpdateUI()
    {
        soulText.text = soulsCollected.ToString();
        healthBar.rectTransform.sizeDelta = new Vector2(healthBarOriginalSize.x * ((float)health / (float)maxHealth), healthBar.rectTransform.sizeDelta.y);
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

    public void OnHit(Vector2 enemyPosition)
    {
        hasControl = false;
        float throwForce = 10f;
        float throwHeight = 2f;
        //Vector2 directionToThrow = new Vector2((enemyPosition - (Vector2)transform.position).magnitude * throwForce, throwHeight);
        m_velocity = new Vector2(-(enemyPosition.x - transform.position.x) * throwForce, throwHeight);
        //Controller.move(directionToThrow * Time.deltaTime);

        StartCoroutine(HasControlTimer());
    }

    IEnumerator HasControlTimer()
    {
        yield return new WaitForSeconds(.3f);
        hasControl = true;
    }

    public void Die()
    {
        SceneManager.LoadScene("Playground");
    }

    private void ChangePlayerDirection(Vector3 input)
    {
        float direction = transform.localScale.x;

        if (input.x > 0)
            direction = 1;
        else if (input.x < 0)
            direction = -1;

        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }

    private void GetInputAndSetVelocity()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), (Input.GetKeyDown(KeyCode.Space) ? 1 : 0), 0);

        if (Controller.isGrounded)
            m_velocity.y = 0;

        normalizedHorizontalSpeed = movementInput.x;

        ChangePlayerDirection(movementInput);

        // If grounded we can jump
        if (Controller.isGrounded && movementInput.y > 0)
        {
            // equation to find velocity to jump a particular height is v=sqr(2*H*G)
            m_velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = Controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        m_velocity.x = Mathf.Lerp(m_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        m_velocity.y += gravity * Time.deltaTime;

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (Controller.isGrounded && Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space))
        {
            //m_velocity.y *= 3f;
            Controller.ignoreOneWayPlatformsThisFrame = true;
        }

        //Controller.move(m_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        //m_velocity = Controller.velocity;
    }

    private void MovePlayer()
    {
        Controller.move(m_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        m_velocity = Controller.velocity;
    }
}
