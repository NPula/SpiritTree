using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject
{

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

    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = new Vector2(maxSpeed * direction, 0);

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

        if (targetVelocity.x < -.01)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (targetVelocity.x > .01)
        {
            transform.localScale = new Vector2(1, 1);
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == CharacterController.Instance.gameObject)
        {
            //Debug.Log("Hurt the player!");
            CharacterController.Instance.health -= attackPower;
            CharacterController.Instance.UpdateUI();
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == CharacterController.Instance.gameObject)
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
