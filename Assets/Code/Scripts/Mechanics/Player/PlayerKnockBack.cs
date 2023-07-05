using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DamageMechanics;

namespace PlayerMechanics
{
    [RequireComponent(typeof(IPlayerController))]
    public class PlayerKnockBack : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rd;
        private IPlayerController _controller;
        private Damageable _damageable;
        private void Awake()
        {
            _controller = GetComponent<IPlayerController>();
            rd = GetComponent<Rigidbody2D>();
            _damageable = GetComponent<Damageable>();
            _damageable.OnHit.AddListener(OnPlayerHit);
        }

        public void OnPlayerHit(Damage damage)
        {
            _controller.controlEnabled = false;
            // Vector2 direction = (transform.position - damage.damager.transform.position).normalized;
            _controller.KnockBack(damage.knockBack);
            StartCoroutine(Reset(damage.immobalDelay));

        }

        private IEnumerator Reset(float _delay)
        {
            yield return new WaitForSeconds(_delay);
            // _controller.KnockBack(Vector2.zero);
            _controller.controlEnabled = true;
        }
    }
}