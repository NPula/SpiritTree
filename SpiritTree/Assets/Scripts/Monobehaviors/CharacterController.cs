using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : PhysicsObject
{

    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float jumpPower = 1;

    [HideInInspector] public List<Interactable> Targets;

    [SerializeField] private CharacterCombat m_playerCombat;
    
    public int soulsCollected;
    public Text soulText;
    public int health = 100;
    private int maxHealth = 100;
    public Image healthBar;
    private Vector2 healthBarOriginalSize;

    public enum DirectionFacing { RIGHT = 1, LEFT = -1}
    [HideInInspector] public DirectionFacing direction;

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
    }

    public void UpdateUI()
    {
        soulText.text = soulsCollected.ToString();

        healthBar.rectTransform.sizeDelta = new Vector2(healthBarOriginalSize.x * ((float)health / (float)maxHealth), healthBar.rectTransform.sizeDelta.y);

        

       // healthBar.rectTransform.sizeDelta 
    }


}

