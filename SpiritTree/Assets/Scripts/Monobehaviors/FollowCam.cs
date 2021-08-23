using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    private Vector3 offset = new Vector3(0, 2f, -10);
    public float smoothSpeed = 0.125f;
    
    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x, offset.y, offset.z);
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //transform.position = smoothedPosition;
        transform.position = desiredPosition;
    }
}
