using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        private Vector2 _momentumVector = Vector2.zero;

        private bool _grounded = true;
        
        public float gravityMultiplier = 2f;
        public float jumpForce = 150f;
        public float thrust = 100f;
        public float maxVelocity = 10f;

        public float floatHeight;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Physics2D.gravity *= gravityMultiplier;
        }

        private void Update()
        {
            UpdateMovement();
        }

        private void FixedUpdate()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

            if (hit.collider != null && !_grounded)
            {
                _grounded = true;
            }
        }

        public void OnJump()
        {
            if (!_grounded) return;
            HandleJump();
            _grounded = false;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            UpdateMomentumVector(context.ReadValue<Vector2>());
        }

        // public void OnCollisionEnter2D(Collision2D other)
        // {
        //     if (other.gameObject.CompareTag("Ground"))
        //     {
        //         _grounded = true;
        //     }
        // }

        private void UpdateMovement()
        {
            if (_rigidbody2D.velocity.magnitude < maxVelocity)
            {
                _rigidbody2D.AddForce(_momentumVector * thrust, ForceMode2D.Force);
            }
        }

        private void HandleJump()
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void UpdateMomentumVector(Vector2 input)
        {
            _momentumVector = new Vector2(input.x, 0);
        }
        
    }
}