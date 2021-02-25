using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Public variables
        public Sprite spriteGuyWithShieldFacingRight;
        public Sprite spriteGuyWithShieldFacingLeft;
        public Sprite spriteGuyWithoutShieldFacingRight;
        public Sprite spriteGuyWithoutShieldFacingLeft;

        [SerializeField] private PlayerHealthEvent healthEvent;
        public PlayerAttributes attributes;
        
        // Scripts
        private PlayerMovement _playerMovement;
        private PlayerThrowShield _playerThrowShield;
        
        // Private variables
        private States _states = new States();
        private SpriteRenderer _spriteRenderer;
        private Sprite _currentLeftFacingSprite;
        private Sprite _currentRightFacingSprite;

        private Damageable _damageable;
        private Rigidbody2D _playerRb;

        private Camera _camera;
        
        // Start is called before the first frame update
        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerThrowShield = GetComponent<PlayerThrowShield>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerRb = GetComponent<Rigidbody2D>();

            _camera = Camera.main;
            
            attributes.healthEvent = healthEvent;
            
            _damageable = GetComponent<Damageable>();
            _damageable.SetHealth(attributes.initialHealth);

            _currentRightFacingSprite = spriteGuyWithShieldFacingRight;
            _currentLeftFacingSprite = spriteGuyWithShieldFacingLeft;
            
            SetStateHasShield(true);
        }
        
        private void FixedUpdate()
        {
            _playerMovement.UpdateMovement();

        }

        public void IG_Shield_OnAim(InputAction.CallbackContext context)
        {
            if (_states.hasShield)
            {
                // if using mouse input
                if (context.control.name.Equals("position"))
                {
                    _playerThrowShield.ThrowingShield_OnAim(
                        _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                }
                else
                {
                    _playerThrowShield.ThrowingShield_OnAim(context.ReadValue<Vector2>());
                }
            }
        }

        public void IG_Player_OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            if (input.x < 0)
            {
                // We are moving left
                _spriteRenderer.sprite = _currentLeftFacingSprite;
            }
            else if (input.x > 0)
            {
                // We are moving right
                _spriteRenderer.sprite = _currentRightFacingSprite;
            }
            _playerMovement.OnMove(input);
        }

        public void IG_Shield_OnFire(InputAction.CallbackContext context)
        {
            if (_states.hasShield && _states.isAiming && context.started)
            {
                _playerThrowShield.ThrowingShield_OnFire();
            }
        }

        public void IG_Player_OnJump(InputAction.CallbackContext context)
        {
            if (_states.isGrounded && context.started)
            {
                _playerMovement.OnJump();
                SetStateIsGrounded(false);
            }
        }
        
        public void SetStateIsAiming( bool s ) { _states.isAiming = s; }
        public bool GetStateIsAiming() { return _states.isAiming; }

        public void SetStateHasShield(bool s)
        {
            _states.hasShield = s;
            if (!s)
            {
                _currentLeftFacingSprite = spriteGuyWithoutShieldFacingLeft;
                _currentRightFacingSprite = spriteGuyWithoutShieldFacingRight;
                
            }
            else
            {
                _currentLeftFacingSprite = spriteGuyWithShieldFacingLeft;
                _currentRightFacingSprite = spriteGuyWithShieldFacingRight;
            }

            if (_states.lookingRight) _spriteRenderer.sprite = _currentRightFacingSprite;
            else _spriteRenderer.sprite = _currentLeftFacingSprite;
        }
        public bool GetStateHasShield() { return _states.hasShield; }
        public void SetStateIsGrounded( bool s ) {_states.isGrounded = s;}
        public bool GetStateIsGrounded() { return _states.isGrounded; }
        public void SetStateLookingRight(bool s) {_states.lookingRight = s;}
        public bool GetStateLookingRight() {return _states.lookingRight;}

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Shield"))
            {
                SetStateHasShield(true);
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                SetStateIsGrounded(true);
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
    }
    
    struct States
    {
        // movement states
        public bool isGrounded;
        public bool lookingRight;

        // Shield states
        public bool hasShield;
        public bool isAiming;
    };
}
