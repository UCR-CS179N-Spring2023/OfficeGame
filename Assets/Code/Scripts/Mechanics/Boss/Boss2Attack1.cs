using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Attack1 : MonoBehaviour
{
    public GameObject LightPrefab;
    public Animator parent;
    private Transform parentTransform;
    private Transform player;
    public Vector2 targetPostion;
    private bool attacking = false;


    private Pathfinding.AIPath aiPath;
    private Pathfinding.AIDestinationSetter destinationSetter;
    private Boss2Detection detection;

    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<Animator>();
        parentTransform = transform.parent.gameObject.GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        detection = transform.parent.gameObject.GetComponent<Boss2Detection>();
        aiPath = transform.parent.gameObject.GetComponent<Pathfinding.AIPath>();
        destinationSetter = transform.parent.gameObject.GetComponent<Pathfinding.AIDestinationSetter>();
    }


    // Update is called once per frame
    void Update()
    {
        targetPostion = new Vector2(player.position.x, player.position.y);
        if (parent.GetBool("isLightning") && attacking == false)
        {
            detection.enabled = false;
            destinationSetter.enabled = false;
            aiPath.enabled = false;
            attacking = true;
            StartCoroutine(waiter(targetPostion));
        }
    }

    IEnumerator waiter(Vector2 targetPostion)
    {
        for (int i = 0; i < 3; i++)
        {
            var position0 = new Vector3(targetPostion.x + Random.Range(-20.0f, 20.0f), targetPostion.y + Random.Range(20.0f, 30.0f), 0);
            var position1 = new Vector3(parentTransform.position.x + Random.Range(-20.0f, 20.0f), targetPostion.y + Random.Range(20.0f, 30.0f), 0);
            var position2 = new Vector3(parentTransform.position.x + Random.Range(-5.0f, 5.0f), targetPostion.y + Random.Range(20.0f, 30.0f), 0);
            Instantiate(LightPrefab, position0, Quaternion.identity);
            Instantiate(LightPrefab, position1, Quaternion.identity);
            Instantiate(LightPrefab, position2, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }

        attacking = false;
        parent.SetBool("isLightning", false);
        detection.enabled = true;
        destinationSetter.enabled = true;
        aiPath.enabled = true;
    }
}
