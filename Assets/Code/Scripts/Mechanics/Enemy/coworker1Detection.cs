using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coworker1Detection : MonoBehaviour
{
    public Vector2 boxSize = new Vector2(1, 1); // size of the box
    public LayerMask layerMask; // which layers to check for collisions with
    public Collider2D hit;

    private Pathfinding.AIPath aiPath;
    private Pathfinding.AIDestinationSetter destinationSetter;
    private coworker1Patrol patrol;
    
    
    private void Start()
    {
        aiPath = GetComponent<Pathfinding.AIPath>();
        aiPath.enabled = false;
        destinationSetter = GetComponent<Pathfinding.AIDestinationSetter>();
        destinationSetter.enabled = false;
        patrol = GetComponent<coworker1Patrol>();
    }

    private void Update()
    {
        hit = Physics2D.OverlapBox(transform.position, boxSize, 0f, layerMask);

        // check if the player is inside the box

        if (hit == null)
        {
            aiPath.enabled = false;
            destinationSetter.enabled = false;
            patrol.enabled = true;
        } else if (hit.CompareTag("Player"))
        {
            aiPath.enabled = true;
            destinationSetter.enabled = true;
            patrol.enabled = false;
        }
        

        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
