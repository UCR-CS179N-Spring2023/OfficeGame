using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Attacking : MonoBehaviour
{

    public float distance;
    private Transform player;
    private bool attacking = false;
    public Vector2 targetPostion;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        anim = gameObject.GetComponent<Animator>();
    }
    [Header("Player")] [SerializeField] private LayerMask collidable;
    // Update is called once per frame
    void Update()
    {
        targetPostion = new Vector2(player.position.x, player.position.y);
        distance = Vector3.Distance(player.position, transform.position);

        if (distance < 5f)
        {
            // do melee in future
        }
        else if (distance > 16f && distance < 22f && attacking == false)
        {
            attacking = true;
            anim.SetBool("isLightning", true);

            StartCoroutine(waiter());
        }
        else if (distance > 12f)
        {
            // do range attack in future
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(7f);
        attacking = false;
    }
}
