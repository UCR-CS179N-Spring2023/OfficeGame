using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMechanics
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public Vector2 velocity => _velocity;
        public Vector2 position => _body.position; 
        public bool jumpingThisFrame { get; private set; }
        public bool landingThisFrame { get; private set; }
        public bool grounded => _colDown;
        public bool controlEnabled { get { return _controlEnabled; } set { _controlEnabled = value; } }
        public bool moveEnabled { get { return _moveEnabled; } set { _moveEnabled = value; } }

        #region SetUp
        private Rigidbody2D _body;
        private CapsuleCollider2D _coll;
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _body.isKinematic = true;
            _coll = GetComponent<CapsuleCollider2D>();
            _inputActions = new PlayerInputActionsAsset();
            _moveAction = _inputActions.Player.Move;
            _jumpAction = _inputActions.Player.Jump;
        }
        private void OnEnable()
        {
            _velocity = Vector2.zero;
            _moveAction.Enable();
            _jumpAction.Enable();
        }

        private void OnDisable()
        {
            _moveAction.Disable();
            _jumpAction.Disable();
        }
        #endregion

        #region Update
        [Header("Enable")]
        [SerializeField] private bool _controlEnabled = true;
        [SerializeField] private bool _moveEnabled = true;

        private void Update()
        {
            if (_moveEnabled)
            {
                _velocity += _body.velocity;
                _body.velocity = Vector2.zero;
            }
            else
            {
                _body.velocity = Vector2.zero;
                _velocity = Vector2.zero;
                return;
            }
            if (_controlEnabled) ParseInput();
            CheckColisons();
            CalculateJumpApex();
            CalculateHorizontal();
            CalculateJump();
            CalculateGravity();
            Move();
        }
        #endregion

        #region Input
        private PlayerInputActionsAsset _inputActions;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private float _timeJumpPressed;
        private float _moveDirection;
        private void ParseInput()
        {
            if (_jumpAction.triggered) _timeJumpPressed = Time.time;
            _moveDirection = _moveAction.ReadValue<Vector2>().x;
        }
        #endregion

        #region Collisions
        [Header("COLLISION")][SerializeField] private LayerMask collidable;
        [SerializeField] private float detectionMaxLength = 0.1f;
        // [SerializeField] private short tryStep = 3;

        [SerializeField] private bool _colUp, _colRight, _colDown, _colLeft;
        private float _timeLeftGrounded;
        private void CheckColisons()
        {
            Vector2 dist = _deltaPosition;

            dist.x = Mathf.Max(Mathf.Abs(dist.x), detectionMaxLength);
            dist.y = Mathf.Max(Mathf.Abs(dist.y), detectionMaxLength);
            var _colDown_new = Physics2D.CapsuleCast(_coll.bounds.center, _coll.bounds.size, _coll.direction, 0f, Vector2.down, dist.y, collidable);

            landingThisFrame = false;
            if (_colDown && !_colDown_new)
            {
                _timeLeftGrounded = Time.time;
            }
            else if (!_colDown && _colDown_new)
            {
                _coyoteUsable = true;
                landingThisFrame = true;
            }
            _colDown = _colDown_new;
            _colUp = Physics2D.CapsuleCast(_coll.bounds.center, _coll.bounds.size, _coll.direction, 0f, Vector2.up, dist.y, collidable);
            _colRight = Physics2D.CapsuleCast(_coll.bounds.center, _coll.bounds.size, _coll.direction, 0f, Vector2.right, dist.x, collidable);
            _colLeft = Physics2D.CapsuleCast(_coll.bounds.center, _coll.bounds.size, _coll.direction, 0f, Vector2.left, dist.x, collidable);
        }
        #endregion

        #region Horizontal
        [Header("WALKING")][SerializeField] private float _acceleration = 60f;
        [SerializeField] private float _deAcceleration = 150f;
        [SerializeField] private float _opositeCoeficient = 2.8f;
        [SerializeField] private float _maxSpeed = 13f;
        [Header("AIR CONTROLL")][SerializeField] private float _apexBonus = 2f;
        [SerializeField] private float _airAcceleration = 70f;
        [SerializeField] private float _airDeAcceleration = 90f;
        private void CalculateHorizontal()
        {
            if (_moveDirection != 0)
            {
                var accel = (_colDown ? _acceleration : _airAcceleration) * (_velocity.x * _moveDirection > 0 ? 1f : _opositeCoeficient);
                _velocity.x = Mathf.Clamp(_velocity.x + _moveDirection * _acceleration * Time.deltaTime, -_maxSpeed, _maxSpeed);
                _velocity.x += Mathf.Sign(_moveDirection) * _apexBonus * _apexPoint * Time.deltaTime;
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0, (_colDown ? _deAcceleration : _airDeAcceleration) * Time.deltaTime); //try second order
            }
        }
        #endregion

        #region Jump
        [Header("JUMP")][SerializeField] private float _jumpHeight = 30f;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3f;

        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint;
        private bool _isCoyoteUsable => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool _isJumpBuffered => _timeJumpPressed + jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!_colDown && _controlEnabled)
            {
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                _gravity = Mathf.Lerp(minGravity, maxGravity, _apexPoint); //try second order
            }
            else
            {
                _apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            if ((_isCoyoteUsable || _colDown) && _isJumpBuffered)
            {
                _velocity.y = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                jumpingThisFrame = true;
            }
            else
            {
                jumpingThisFrame = false;
            }

            if (!_colDown && _jumpAction.ReadValue<float>() == 0 && !_endedJumpEarly && velocity.y > 0)
            {
                _endedJumpEarly = true;
            }
        }
        #endregion

        #region Gravity
        [Header("GRAVITY")][SerializeField] private float maxFallSpeed = 35f;
        [SerializeField] private float minGravity = 70f;
        [SerializeField] private float maxGravity = 100f;
        private float _gravity;
        private void CalculateGravity()
        {
            if (!_colDown)
            {
                var acceleration = _endedJumpEarly && _velocity.y > 0 ? _gravity * _jumpEndEarlyGravityModifier : _gravity;
                _velocity.y -= acceleration * Time.deltaTime;
                _velocity.y = Mathf.Max(-maxFallSpeed, _velocity.y);
            }
        }
        #endregion

        #region Move
        private Vector2 _deltaPosition;
        private Vector2 _velocity;
        // private Vector2 _enviroment_move;

        // public void AddEnvMove(Vector2 move) {
        //     _enviroment_move = move;
        // }

        // private void EnvMove() {
        //     _body.position += _enviroment_move;
        //     _enviroment_move = Vector2.zero;
        // }

        private void Move()
        {
            if ((_colDown && _velocity.y < 0) || (_colUp && _velocity.y > 0)) _velocity.y = 0;


            if ((_velocity.x > 0 && _colRight) || (_velocity.x < 0 && _colLeft)) _velocity.x = 0;

            _deltaPosition = _velocity * Time.deltaTime;
            var hit = Physics2D.CapsuleCast(_coll.bounds.center, _coll.bounds.size, _coll.direction, 0f, _deltaPosition, _deltaPosition.magnitude, collidable);
            if (hit)
            {

                //     var move = Vector2.zero;
                //     if (_deltaPosition.x >= detectionMaxLength)
                //     {

                //         move.x = _deltaPosition.x;
                //         move.y = 0;
                //         _deltaPosition.x = 0f;
                //         // Debug.Log("move:" + _deltaPosition);
                //         // Debug.Log(hit.distance);

                //         move.x = move.x / 2;
                //         // Debug.Log("move:" + move);
                //         // Debug.Log("delta + move" + (_deltaPosition + move));
                //         hit = Physics2D.CapsuleCast(_coll.bounds.center, _coll.bounds.size, _coll.direction, 0f, move,   _deltaPosition.x + move.x, collidable);
                //         if (!hit)
                //         {
                //             Debug.Log("not hit");
                //             _deltaPosition.x += move.x;
                //         }
                //         Debug.Log("new delta" + _deltaPosition);
                //     }
                //     move.x = _deltaPosition.x;
                //     _body.position += move;

                //     if (_deltaPosition.y >= detectionMaxLength)
                //     {

                //         move.y = _deltaPosition.y;
                //         move.x = 0;
                //         _deltaPosition.y = 0;
                //         move.y = move.y / 2;
                //         // Debug.Log("move:" + move);
                //         // Debug.Log("delta + move" + (_deltaPosition + move));
                //         hit = Physics2D.CapsuleCast(_coll.bounds.center, _coll.bounds.size, _coll.direction, 0f, move,  _deltaPosition.y + move.y, collidable);
                //         if (!hit)
                //         {
                //             Debug.Log("not hit");
                //             _deltaPosition.y += move.y;

                //         }
                //             Debug.Log("new delta" + _deltaPosition);

                //         // controlEnabled = false;
                //     }
                //     move.y = _deltaPosition.y;
                //     _body.position += move;
            }
            else
            {
                _body.position += _deltaPosition;
            }



            if (_body.position.y < -300f)
                _body.position = new Vector2(-437f, -258f);

        }


        public void Teleport(Vector2 moveTo)
        {
            _body.velocity = Vector2.zero;
            _velocity = Vector2.zero;
            _body.position = moveTo;
        }

        public void KnockBack(Vector2 knockBack)
        {
            _controlEnabled = false;
            _moveDirection = 0f;
            _velocity = knockBack;
        }

        #endregion
    }
}