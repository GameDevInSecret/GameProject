using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Public variables
        public Sprite spriteGuyWithShieldFacingRight;
        public Sprite spriteGuyWithShieldFacingLeft;
        public Sprite spriteGuyWithoutShieldFacingRight;
        public Sprite spriteGuyWithoutShieldFacingLeft;
        
        // Scripts
        private PlayerMovement _playerMovement;
        private PlayerThrowShield _playerThrowShield;
        
        // Private variables
        private States _states = new States();
        private SpriteRenderer _spriteRenderer;
        private Sprite _currentLeftFacingSprite;
        private Sprite _currentRightFacingSprite;
        
        // Start is called before the first frame update
        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerThrowShield = GetComponent<PlayerThrowShield>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _currentRightFacingSprite = spriteGuyWithShieldFacingRight;
            _currentLeftFacingSprite = spriteGuyWithShieldFacingLeft;
            
            SetStateHasShield(true);
        }

        // Update is called once per frame
        private void Update()
        {
            _playerMovement.UpdateMovement();
        }

        public void IG_Shield_OnAim(InputAction.CallbackContext context)
        {
            if (_states.hasShield)
            {
                _playerThrowShield.ThrowingShield_OnAim(context.ReadValue<Vector2>());
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
            if (_states.isGrounded)
            {
                _playerMovement.OnJump();
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
        }
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
