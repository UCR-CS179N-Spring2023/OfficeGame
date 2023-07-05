using System;
using UnityEngine;
using UnityEngine.Events;

namespace DamageMechanics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Damageable : MonoBehaviour
    {
        public float InitHealth => _initHealth;
        public bool isAlive => _health > 0;
        public float Health { get { return _health; } set { _health = value; } }

        [Header("Health")][SerializeField] private float _initHealth = 100f;
        [SerializeField] private float _health = 100f;
        [SerializeField] private float _invincibleTime = .2f;

        [Header("Event Listners")]
        [SerializeField] public DamageEvent OnHit;
        [SerializeField] public DamageEvent OnDeath;
        private float _lastHit = 0f;


        public bool Damage(float damage, GameObject damager)
        {
            return this.Damage(new Damage(damage, 0f, Vector2.zero, damager));
        }

        public bool Damage(Damage damage)
        {
            if (!isAlive || Time.time < _lastHit + _invincibleTime) return false;
            _lastHit = Time.time;
            _health -= damage.damage;
            OnHit?.Invoke(damage);
            if (!isAlive) OnDeath?.Invoke(damage);
            return true;
        }

    }

    public struct Damage
    {
        public float damage;
        public float immobalDelay;
        public Vector2 knockBack;
        public GameObject damager;

        public Damage(float damage, float immobalDelay, Vector2 knockBack, GameObject damager)
        {
            this.damage = damage;
            this.immobalDelay = immobalDelay;
            this.knockBack = knockBack;
            this.damager = damager;
        }
    }
    [Serializable]
    public class DamageEvent : UnityEvent<Damage> { }
}