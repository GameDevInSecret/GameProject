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

        // Private variables
        private StateController _state;
        private SpriteRenderer _spriteRenderer;
        private Sprite _currentLeftFacingSprite;
        private Sprite _currentRightFacingSprite;

        private Damageable _damageable;
        private Rigidbody2D _playerRb;

        // Aim tools
        private Camera _camera;
        private AimAssist _aimAssist;

        public GameObject shield;
        
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

            _state = new StateController();
            
        }
        
        private void FixedUpdate()
        {
            _playerMovement.UpdateMovement();

            _aimAssist.gameObject.SetActive(_state.IsAiming);
            
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
                _playerMovement.OnJump();
                _state.IsGrounded = false;
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
            if (!_state.HasShield && !_state.IsAiming && !context.started) return;
            
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
    }
}
