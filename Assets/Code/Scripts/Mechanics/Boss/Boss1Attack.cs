using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack : MonoBehaviour
{
    // Start is called before the first frame update
    public float distance;
    private Transform player;
    public Transform enabledChild;
    private bool attacking = false;
    public Vector2 targetPostion;
    private GameObject jumping;

    [SerializeField] private AudioSource jumpAttackSound;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "JumpAttack")
            {
                jumping = transform.GetChild(i).gameObject;
            }
        }

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
        else if (distance > 2f && distance < 12f && attacking == false)
        {
            attacking = true;
            jumping.SetActive(true);
            jumping.GetComponent<Boss1JumpAttack>().enabled = true;
            jumpAttackSound.Play();
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3f);
        attacking = false;
        jumping.SetActive(false);
        enabledChild = null;
    }
}
