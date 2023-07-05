using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DamageMechanics;
using PlayerMechanics;


[RequireComponent(typeof(Collider2D))]
public class Boss2Light : Atack
{
    [SerializeField] private AudioSource lightSound;

    public override void OnTriggerStay2D(Collider2D col)
    {
        Damageable damageable = col.GetComponent<Damageable>();
        if (damageable != null)
        {
            lightSound.Play();
            Vector2 direction = col.transform.position.x - transform.position.x < 0 ? Vector2.left : Vector2.right;
            direction += Vector2.up;
            bool is_suc = damageable.Damage(new Damage(_damage, _immobalDelay, direction.normalized * _knockBack, transform.root.gameObject));
            //if (is_suc) Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (transform.position.y < -400f)
        {
            Destroy(gameObject);
        }
    }
}
