using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Detection : MonoBehaviour
{

    public Vector2 boxSize = new Vector2(1, 1); // size of the box
    public LayerMask layerMask; // which layers to check for collisions with
    public Collider2D hit;

    public Pathfinding.AIPath aiPath;
    public Pathfinding.AIDestinationSetter destinationSetter;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        aiPath = GetComponent<Pathfinding.AIPath>();
        aiPath.enabled = false;
        destinationSetter = GetComponent<Pathfinding.AIDestinationSetter>();
        destinationSetter.enabled = false;

        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.OverlapBox(transform.position, boxSize, 0f, layerMask);

        
        if (hit) {
            aiPath.enabled = true;
            destinationSetter.enabled = true;
            anim.SetBool("isWalking", true);
        }
        else {
            aiPath.enabled = false;
            destinationSetter.enabled = false;
            anim.SetBool("isWalking", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
