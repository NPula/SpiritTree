using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    //public float inAirDamping = 5f;
    //public float jumpHeight = 3f;
    private CharacterController2D m_controller;
    private Vector3 m_velocity;

    [SerializeField] private float maxSpeed;
    private RaycastHit2D rightLedgeRaycastHit;
    private RaycastHit2D LeftLedgeRaycastHit;
    [SerializeField] private Vector2 rayCastOffset;
    [SerializeField] private float rayCastLength = 2;
    private int direction = 1;

    public GameObject attackBox;

    [SerializeField] private float attackDuration;

    private RaycastHit2D rightWallRaycastHit;
    private RaycastHit2D leftWallRaycastHit;
    [SerializeField] private LayerMask rayCastLayerMask;

    [SerializeField] private int attackPower = 10;

    public int health = 100;
    //private int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        m_controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_velocity.y += gravity * Time.deltaTime;

        //Vector3 targetVelocity = transform.GetComponent<Rigidbody2D>().velocity;
        //Vector3 targetVelocity = new Vector2(maxSpeed * direction * Time.deltaTime, 0);
        m_velocity.x = runSpeed * direction;
        m_controller.move(m_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        m_velocity = m_controller.velocity;
        //transform.Translate(targetVelocity);

        // Check for right ledge
        rightLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y + rayCastOffset.y), Vector2.down, rayCastLength);
        //Debug.DrawRay(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y + rayCastOffset.y), Vector2.down * rayCastLength, Color.blue);

        if (rightLedgeRaycastHit.collider == null)
        {
            direction = -1;
        }
        else
        {
            //Debug.Log("Im Touching:" + rightLedgeRaycastHit.collider.gameObject);
        }

        // Check for Left ledge
        LeftLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y + rayCastOffset.y), Vector2.down, rayCastLength);
        //Debug.DrawRay(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y + rayCastOffset.y), Vector2.down * rayCastLength, Color.green);

        if (LeftLedgeRaycastHit.collider == null)
        {
            direction = 1;
        }
        else
        {
            //Debug.Log("Im Touching:" + LeftLedgeRaycastHit.collider.gameObject);
        }

        rightWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, rayCastLength, rayCastLayerMask);
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * rayCastLength, Color.red);

        if (rightWallRaycastHit.collider != null)
        {
            direction = -1;
        }

        leftWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, rayCastLength, rayCastLayerMask);
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * rayCastLength, Color.magenta);

        if (leftWallRaycastHit.collider != null)
        {
            direction = 1;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (m_velocity.x < -.01)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (m_velocity.x > .01)
        {
            transform.localScale = new Vector2(1, 1);
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            player.health -= attackPower;
            player.UpdateUI();
            
            player.OnHit(transform.position); // make into its own event function later.
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RecieveAttack());
        }
    }

    public IEnumerator RecieveAttack()
    {
        attackBox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackBox.SetActive(false);

    }
}
