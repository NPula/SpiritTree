using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Transform player;
    protected PlayerController playerController;

    [SerializeField] protected float m_radius = 1f;
    //public float Radius { get; protected set; }

    protected bool IsInteractable { get; private set; }

    protected bool fuck;
    protected virtual void Start()
    {
        //player = GameObject.Find("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
    }

    protected virtual void Update()
    {
        IsInteractable = IsPlayerInRadius(player);
        if (IsInteractable)
        {
            if (!playerController.Targets.Contains(this))
            {
                playerController.Targets.Add(this);
            }

            // playerController.Target = this;
        }
        else
        {
            //playerController.Target = null;

            // Stop interacting as soon as the target object goes null or is outside the range (radius).
            StopInteracting(); 
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting");
    }

    public virtual void StopInteracting()
    {
        //playerController.Target = null;
        playerController.Targets.Remove(this);
    }

    public virtual bool MoreToDo() { return false; }

    public bool IsPlayerInRadius(Transform player)
    {
        float distance = Mathf.Abs((player.position - transform.position).magnitude);
        return (distance <= m_radius);
    }

    public void SetPlayerPosition(Transform position)
    {
        player = position;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_radius);
    }

    public virtual void OnDestroy()
    {
        playerController.Targets.Remove(this);
    }

}
