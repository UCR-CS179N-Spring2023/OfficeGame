using UnityEngine;
using PlayerMechanics;

namespace DamageMechanics
{
    public class RespawnPoint : MonoBehaviour
    {
        // Start is called before the first frame update
        private PlayerDeath playerD;
        private SpriteRenderer red;
        private SpriteRenderer green;

        void Start()
        {
            playerD = GameObject.Find("Player").GetComponent<PlayerDeath>();
            red = transform.Find("red").gameObject.GetComponent<SpriteRenderer>();
            green = transform.Find("green").gameObject.GetComponent<SpriteRenderer>();
            red.enabled = true;
            green.enabled = false;
            
        }


        public virtual void OnTriggerStay2D(Collider2D col)
        {
           playerD.spawn = col.transform.position;
           red.enabled = false;
           green.enabled = true;
        }
    }
}