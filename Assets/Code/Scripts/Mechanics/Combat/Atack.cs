using System.Diagnostics;
using UnityEngine;

namespace DamageMechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class Atack : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] protected float _damage = 30f;
        [Header("Knock Back")]
        [SerializeField] protected Vector2 _knockBack = Vector2.zero;
        [SerializeField] protected float _immobalDelay = 0f;
        
        public virtual void OnTriggerStay2D(Collider2D col)
        {
            Damageable damageable = col.GetComponent<Damageable>();
            if (damageable != null)
            {
                Vector2 direction = (col.transform.position - transform.position).normalized;
                bool is_suc = damageable.Damage(new Damage(_damage, _immobalDelay, direction*_knockBack, transform.root.gameObject));
            }
        }
    }
}