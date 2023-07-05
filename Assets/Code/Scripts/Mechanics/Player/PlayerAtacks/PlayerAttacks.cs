using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using DamageMechanics;
using System.Threading;

namespace PlayerMechanics
{
    public class PlayerAttacks : MonoBehaviour, IPlayerAttacks
    {
        public bool attacked { get; private set; }
        public bool ranged { get; private set; }
        public bool hit { get; private set; }

        private PlayerInputActionsAsset _inputActions;
        private InputAction _fireAction;
        private InputAction _rangeAction;
        private InputAction _mouseAction;

        private float _rangeTimeFirePressed;
        private float _timeFirePressed;



        private void Awake()
        {
            _inputActions = new PlayerInputActionsAsset();
            _fireAction = _inputActions.Player.Fire;
            _rangeAction = _inputActions.Player.RangeFire;
            _mouseAction = _inputActions.Player.Mouse;
            RangeAttack ra = transform.Find("RangeAttack").gameObject.GetComponent<RangeAttack>();
            _rangeAtackEvent.AddListener(ra.LaunchProjectile);
        }
        private void OnEnable()
        {
            _fireAction.Enable();
            _rangeAction.Enable();
            _mouseAction.Enable();
        }

        private void OnDisable()
        {
            _fireAction.Disable();
            _rangeAction.Disable();
            _mouseAction.Disable();
        }
        [SerializeField] public bool _rangeEnabled = true;


        [SerializeField] private UnityEvent<Vector2> _rangeAtackEvent;

        [Header("Enemy")][SerializeField] private LayerMask collidable;
        private void Update()
        {
            attacked = false;
            ranged = false;
            if (_fireAction.triggered && Time.time >= _timeFirePressed + 1f)
            {
                _timeFirePressed = Time.time;
                // hit = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0f, Vector2.right * transform.localScale.x, 1f, collidable);
                attacked = true;
            }
            else if (_rangeEnabled && _rangeAction.triggered && Time.time >= _rangeTimeFirePressed + 1.5f)
            {
                ranged = true;
                _rangeTimeFirePressed = Time.time;
                var mouse_pos = Camera.main.ScreenToWorldPoint(_mouseAction.ReadValue<Vector2>());
                var delta = (mouse_pos - transform.position).normalized;
                _rangeAtackEvent.Invoke(delta);
            }
        }
    }
}
