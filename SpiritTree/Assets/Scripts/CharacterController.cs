using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : PhysicsObject
{

    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float jumpPower = 1;

    public List<Interactable> Targets;

    public int soulsCollected;

    public Text soulText;


    void Start()
    {
        //soulText = GameObject.Find("Soul").GetComponent<Text>();
        UpdateUI();
    }

    void Update()
    {

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, 0);

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
    }

    public void UpdateUI()
    {
        soulText.text = soulsCollected.ToString();
    }


}

