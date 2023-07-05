using UnityEngine;
using DamageMechanics;

namespace PlayerMechanics
{
    [RequireComponent(typeof(IPlayerController))]
    public class PlayerDeath : MonoBehaviour
    {
        public Vector2 spawn { get { return _spawn; } set { _spawn = value; } }
        private IPlayerController _controller;
        [Header("Spawn")]
        [SerializeField] private Vector2 _spawn;
        [SerializeField] public short lifes = 3;
        private Damageable damageable;

        private void Awake()
        {
            damageable = GetComponent<Damageable>();
            _controller = this.gameObject.GetComponent<IPlayerController>();
            damageable.OnDeath.AddListener(OnPlayerDeath);
        }

        private void Start() {
            _spawn = _controller.position;
        }

        public void OnPlayerDeath(Damage damage)
        {
            if (lifes != 0)
            { // respawn
                damageable.Health = 100f;
                _controller.Teleport(_spawn);
                lifes -= 1;
            }
            else
            { // perma death 
                Debug.Log("Game Over");

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");

                foreach (GameObject player in players)
                {
                    Destroy(player);
                }

                foreach (GameObject entity in entities)
                {
                    Destroy(entity);
                }
                
                GameScript.Instance.ShowDeathMenu();
            }  
        }
    }
}