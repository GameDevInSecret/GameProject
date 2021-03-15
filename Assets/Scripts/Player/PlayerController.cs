using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Public variables
        [Header("Sprites")]
        public Sprite spriteGuyWithShieldFacingRight;
        public Sprite spriteGuyWithShieldFacingLeft;
        public Sprite spriteGuyWithoutShieldFacingRight;
        public Sprite spriteGuyWithoutShieldFacingLeft;

        [Header("Attributes")]
        [SerializeField] private PlayerHealthEvent healthEvent;
        public PlayerAttributes attributes;
        
        // Scripts
        private PlayerMovement _playerMovement;

        // Private variables
        private Player.StateController _state;
        private SpriteRenderer _spriteRenderer;
        private Sprite _currentLeftFacingSprite;
        private Sprite _currentRightFacingSprite;

        private Damageable _damageable;
        private Rigidbody2D _playerRb;

        // Aim tools
        private Camera _camera;
        private AimAssist _aimAssist;
        private bool _blockedAim;
        [SerializeField] private LayerMask _shieldBlockableLayers;
        [SerializeField] private GameObject shieldBlockedIndicator;
        private GameObject _blockIndicatorInstance;

        [Header("Shield Variables")]
        public GameObject shield;
        
        // jumping variables
        [Header("Jump Variables")]
        [SerializeField] private LayerMask _jumpBufferLayerMask;
        [SerializeField] private float _minDistanceToBufferJump = 0.1F;
        [SerializeField] private Transform _leftBufferRay;
        [SerializeField] private Transform _rightBufferRay;
        private bool _leftBufferRayHit = false;
        private bool _RightBufferRayHit = false;
        private bool _jumpBuffered = false;
        
        // Is Grounded variables
        [Header("Is Grounded Variables")] 
        [SerializeField] private LayerMask _groundedLayerMask;
        [SerializeField] private Transform _leftGroundedRay;
        [SerializeField] private Transform _midGroundedRay;
        [SerializeField] private Transform _rightGroundedRay;
        private float _groundedDistance = 0.2F;
        [SerializeField] private bool grounded;

        // Start is called before the first frame update
        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerRb = GetComponent<Rigidbody2D>();

            _camera = Camera.main;
            _aimAssist = GetComponentInChildren<AimAssist>();
            
            attributes.healthEvent = healthEvent;
            
            _damageable = GetComponent<Damageable>();
            _damageable.SetHealth(attributes.initialHealth);

            _currentRightFacingSprite = spriteGuyWithShieldFacingRight;
            _currentLeftFacingSprite = spriteGuyWithShieldFacingLeft;

            _blockIndicatorInstance = Instantiate(shieldBlockedIndicator, transform.position,
                shieldBlockedIndicator.transform.rotation);
            _blockIndicatorInstance.SetActive(false);

            _state = new StateController();
            
        }

        private void Update()
        {
            _state.IsGrounded = CheckIsOnGround();
            grounded = _state.IsGrounded;
            
            CloseToGround();
            ShieldThrowBlocked();
            
            if (_state.IsGrounded && _jumpBuffered)
            {
                _jumpBuffered = false;
                
                // we need to stop the player's velocity because the rays extend below the box collider
                // if we don't stop all movement before doing the jump, the downward momentum will consume the
                // jump force and the player won't go back upwards after _state.IsGrounded gets set to true
                _playerMovement.ZeroYVelocity();
                
                _playerMovement.OnJump();
            }
        }

        private void FixedUpdate()
        {
            _playerMovement.UpdateMovement();

            _aimAssist.gameObject.SetActive(_state.IsAiming && !_blockedAim);
            _blockIndicatorInstance.SetActive(_state.IsAiming && _blockedAim);

            UpdateSprite();

        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            // update look state for sprite rendering
            var input = context.ReadValue<Vector2>();
            if (input.x > 0)
                _state.IsLookingRight = true;
            else if (input.x < 0)
                _state.IsLookingRight = false;
            
            _playerMovement.OnMove(input);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // context.started check to prevent second jump on button release
            if (_state.IsGrounded && context.started)
            {
                print("Normal Jump");
                _playerMovement.OnJump();
                // _state.IsGrounded = false;
            }
            else if (!_state.IsGrounded && context.started && CloseToGround())
            {
                print("Buffered");
                _jumpBuffered = true;
            }
            else if (!_state.IsGrounded && context.canceled)
            {
                print("Stopping Jump");
                _playerMovement.OnJump(false);
            }
            else
            {
                print("I NO KNOW WHAT TO DO");
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            //TODO: add deadzone to right stick to prevent snapping opposite dir on quick release
            //TODO: add timer/check for mouse input no-change to disable arrow on no aim with mouse
            //TODO: update sprite based on look?
            
            var input = context.ReadValue<Vector2>();
        
            // adjust input values for mouse input
            if (context.control.name.Equals("position"))
            {
                Vector2 playerPos = _camera.WorldToScreenPoint(transform.position);
                input -= playerPos;
            }
            
            if (_state.HasShield)
            {
                _state.IsAiming = true;

                _aimAssist.OnAim(input);

                ShieldThrowBlocked();

                // update look state for sprite rendering
                // var input = context.ReadValue<Vector2>();
                // UpdateLookingState(input);

            }
            
            if (context.canceled)
            {
                _state.IsAiming = false;
            }

        }

        public void OnFire(InputAction.CallbackContext context)
        {
            // check fire conditions
            if ((!_state.HasShield && !_state.IsAiming) || _blockedAim) return;

            var selfPos = transform.position;
            var spawnPos = selfPos + new Vector3(1.5f, 0f, 0f);

            // instantiate shield
            var shieldInstance = Instantiate(shield, spawnPos, _aimAssist.transform.rotation);
            shieldInstance.transform.RotateAround(selfPos, Vector3.forward, _aimAssist.currentAngle);
            
            // throw shield
            var shieldController = shieldInstance.GetComponent<ShieldController>();
            shieldController.SetForceDirection(shieldInstance.transform.position - selfPos);
            shieldController.ThrowShield();

            // set states and active sprites
            _state.HasShield = false;
            _state.IsAiming = false;
        }

        private void UpdateSprite()
        {
            if (_state.HasShield)
            {
                
                _currentRightFacingSprite = spriteGuyWithShieldFacingRight;
                _currentLeftFacingSprite = spriteGuyWithShieldFacingLeft;
            }
            else
            {
                _currentRightFacingSprite = spriteGuyWithoutShieldFacingRight;
                _currentLeftFacingSprite = spriteGuyWithoutShieldFacingLeft;
            }

            _spriteRenderer.sprite = _state.IsLookingRight ? _currentRightFacingSprite : _currentLeftFacingSprite;
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Shield"))
            {
                _state.HasShield = true;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                _state.IsGrounded = true;
            }
        }

        public void OnHealthSet(Damageable dm)
        {
            print("HEALTH SET TO " + dm.CurrentHealth);
        }

        public void OnTakeDamage(Damager damager, Damageable damageable)
        {
            print("PLAYER HIT FOR " + damager.damage + " DAMAGE!");
            Vector2 damageDir = (transform.position - damager.transform.position + (UnityEngine.Vector3)damager.offset).normalized;
            _playerRb.AddForce(damageDir * 2F, ForceMode2D.Impulse);
            attributes.LowerHealth(damager.damage);
        }

        public void OnDie(Damager damager, Damageable damageable)
        {
            print("You done died, muh boi");
        }

        public void OnGainHealth(int val, Damageable damageable)
        {
            print("Gained " + val + " health");
        }
        
        public void OnDamageableEvent( Damager damager, Damageable damageable) {print("HITTING SOMETHING!");}
        public void OnNonDamageableEvent(Damager damager) {print("HITTING SOMETHING THAT ISN'T DAMAGEABLE");}

        private bool CloseToGround()
        {
            bool leftHit = false;
            bool rightHit = false;
            
            Vector2 leftStartPoint = _leftBufferRay.transform.position;
            RaycastHit2D hitLeft = Physics2D.Raycast(leftStartPoint, Vector2.down, _minDistanceToBufferJump ,_jumpBufferLayerMask);

            if (hitLeft)
            {
                if (hitLeft.transform.CompareTag("Ground"))
                {
                    _leftBufferRayHit = true;
                    leftHit = true;
                }
            }
            else
            {
                _leftBufferRayHit = false;
            }
            
            Vector2 rightStartPoint = _rightBufferRay.transform.position;
            RaycastHit2D hitRight = Physics2D.Raycast(rightStartPoint, Vector2.down, _minDistanceToBufferJump, _jumpBufferLayerMask);

            if (hitRight)
            {
                if (hitRight.transform.CompareTag("Ground"))
                {
                    _RightBufferRayHit = true;
                    rightHit = true;
                }
            }
            else
            {
                _RightBufferRayHit = false;
            }

            return leftHit || rightHit;
        }

        private void OnDrawGizmos()
        {
            // jump buffer gizmo ---------------------------------------------------------------------------------------
            // if (_leftBufferRayHit) Gizmos.color = Color.blue;
            // else Gizmos.color = Color.red;
            // Gizmos.DrawRay(_leftBufferRay.position, Vector2.down * _minDistanceToBufferJump);
            //
            // if (_RightBufferRayHit) Gizmos.color = Color.blue;
            // else Gizmos.color = Color.red;
            // Gizmos.DrawRay(_rightBufferRay.position, Vector2.down * _minDistanceToBufferJump);
            
            // is grounded gizmo ---------------------------------------------------------------------------------------
            // Gizmos.color = Color.red;
            // Gizmos.DrawRay(_leftGroundedRay.position, Vector2.down * _groundedDistance);
            // Gizmos.DrawRay(_midGroundedRay.position, Vector2.down * _groundedDistance);
            // Gizmos.DrawRay(_rightGroundedRay.position, Vector2.down * _groundedDistance);

            // Blocked shield gizmo ------------------------------------------------------------------------------------
            // if (_aimAssist)
            // {
            //     Gizmos.color = Color.magenta;
            //     Vector2 checkPoint = Vector2.right * 1.5f;
            //     checkPoint = Quaternion.Euler(0, 0, _aimAssist.currentAngle) * checkPoint;
            //     Gizmos.DrawRay(transform.position, (checkPoint));
            // }
        }

        private bool CheckIsOnGround()
        {
            RaycastHit2D hit;
            
            Vector2 leftStartPoint = _leftGroundedRay.transform.position;
            hit = Physics2D.Raycast(leftStartPoint, Vector2.down, _groundedDistance ,_groundedLayerMask);
            if (hit && hit.transform.CompareTag("Ground")) return true;
            
            Vector2 midStartPoint = _midGroundedRay.transform.position;
            hit = Physics2D.Raycast(midStartPoint, Vector2.down, _groundedDistance ,_groundedLayerMask);
            if (hit && hit.transform.CompareTag("Ground")) return true;
            
            Vector2 rightStartPoint = _rightGroundedRay.transform.position;
            hit = Physics2D.Raycast(rightStartPoint, Vector2.down, _groundedDistance ,_groundedLayerMask);
            if (hit && hit.transform.CompareTag("Ground")) return true;

            return false;
        }

        private bool ShieldThrowBlocked()
        {
            bool blocked = false;

            // make a vector that points to the right with a length of 1.5 (the distance away that the shield will spawn)
            Vector2 checkPoint = Vector2.right * 1.5f;
            
            // rotate the vector by the angle set in _aimAssist
            checkPoint = Quaternion.Euler(0, 0, _aimAssist.currentAngle) * checkPoint;

            // cast a ray starting at the player center out to the point where the shield will spawn
            // use the spawn distance again, idk if the checkPoint vector needs to be normalized
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (checkPoint), 1.5F, _shieldBlockableLayers);

            // check if there was a collision
            if (hit)
            {
                // there was, so set the position of the block indicator to the point of the collision
                _blockedAim = true;
                _blockIndicatorInstance.transform.position = hit.point;
            }
            else
            {
                // there wasn't a hit, so our aim isnt blocked
                _blockedAim = false;
            }

            return blocked;
        }
    }
}
