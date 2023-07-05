using UnityEngine;

namespace DamageMechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundColl : MonoBehaviour
    {
        public virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.name == "Collidable")
                Destroy(this.gameObject);
        }
    }
}