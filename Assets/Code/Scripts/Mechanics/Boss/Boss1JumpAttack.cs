using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageMechanics;
using PlayerMechanics;

public class Boss1JumpAttack : Atack
{
    public float strength = 30f;
    private Animator anim;
    private Rigidbody2D rb;

    private Pathfinding.AIPath aiPath;
    private Pathfinding.AIDestinationSetter destinationSetter;
    private BossDetection detection;

    public Vector2 target;
    // bool detected = false;
    Vector2 zero = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start()
    {
        detection = GameObject.Find("Boss1").GetComponent<BossDetection>();
        aiPath = GameObject.Find("Boss1").GetComponent<Pathfinding.AIPath>();
        destinationSetter = GameObject.Find("Boss1").GetComponent<Pathfinding.AIDestinationSetter>();

        anim = GameObject.Find("Boss1").GetComponent<Animator>();
        rb = GameObject.Find("Boss1").GetComponent<Rigidbody2D>();
        target = GameObject.Find("Boss1").GetComponent<Boss1Attack>().targetPostion;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.Equals(zero))
        {
            target = GameObject.Find("Boss1").GetComponent<Boss1Attack>().targetPostion;
        }
        aiPath.enabled = false;
        detection.enabled = false;
        destinationSetter.enabled = false;

        anim.SetBool("isJumping", true);

        Vector2 newPos = Vector2.MoveTowards(rb.position, target, 5 * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2f);

        aiPath.enabled = true;
        detection.enabled = true;
        destinationSetter.enabled = true;

        anim.SetBool("isJumping", false);
        gameObject.SetActive(false);
        gameObject.GetComponent<Boss1JumpAttack>().enabled = false;
        target = zero;
    }
}
