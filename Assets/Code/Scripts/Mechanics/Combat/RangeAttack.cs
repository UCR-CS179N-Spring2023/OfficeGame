using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Threading;
using System.Security.Cryptography;

namespace DamageMechanics
{
    public class RangeAttack : MonoBehaviour
    {
        [SerializeField] public GameObject RangeProject;
        [SerializeField] private float _time = 1.5f;
        [SerializeField] private float _speed = 30f;
        [SerializeField] private float _spawn = 3.5f;
        [SerializeField] private GameObject _projectile;

        public void LaunchProjectile(Vector2 direct)
        {
            if (_projectile == null)
            {
                 Vector3 v3 = direct;
                 v3 = v3.normalized;
                _projectile = Instantiate(RangeProject, transform.position+(v3 * _spawn), transform.rotation);
                Rigidbody2D rb = _projectile.GetComponent<Rigidbody2D>();
                Transform trans = _projectile.GetComponent<Transform>();
                rb.velocity = v3 * _speed;
                trans.localScale = new Vector3(rb.velocity.x > 0 ? 1 : -1, 1, 1);
                StartCoroutine(Reset());
            }
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(_time);

            if (_projectile != null) Destroy(_projectile);
        }
    }
}