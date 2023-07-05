using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coworker1Attack : MonoBehaviour
{
    // Start is called before the first frame update
    public float distance;
    private Transform player;
    private Animator anim;
    private Pathfinding.AIPath aiPath;
    private coworker1Patrol patrol;
    private coworker1Detection detection;

    [SerializeField] private AudioSource caseAttackSound;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        aiPath = GetComponent<Pathfinding.AIPath>();
        anim = GetComponent<Animator>();
        anim.SetBool("isAttacking", false);
        patrol = GetComponent<coworker1Patrol>();
        detection = GetComponent<coworker1Detection>();
    }
    [Header("Player")][SerializeField] private LayerMask collidable;

    // Update is called once per frame
    void Update()
    {
        
        distance = Vector3.Distance(player.position, transform.position);
        if (distance < 1.5f)
        {
            aiPath.enabled = false;
            patrol.enabled = false;
            anim.SetBool("isAttacking", true);
            caseAttackSound.Play();
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2f);
        aiPath.enabled = true;
        patrol.enabled = true;
        anim.SetBool("isAttacking", false);
    }
}
