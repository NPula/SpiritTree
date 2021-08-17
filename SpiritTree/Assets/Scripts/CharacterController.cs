using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : PhysicsObject
{

    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float jumpPower = 1;

    void Update()
    {

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, 0);

        // if player presses "jump" and were grounded set velocity to jump power value 
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpPower;
        }
    }


}