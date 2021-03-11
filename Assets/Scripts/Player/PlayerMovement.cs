using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        private Vector2 _momentumVector = Vector2.zero;
        
        public float gravityMultiplier = 2f;
        public float jumpForce = 150f;
        public float thrust = 100f;
        public float maxVelocity = 10f;
        [Tooltip("How much to slow down moving upwards after the jump button is released")]
        public float jumpUpSlowDownMultiplier = 0.5f;
        [Tooltip("How fast the player should accelerate towards the ground after falling from a jump")]
        public float jumpFallMultiplier = 1.01f;
        [Tooltip("How fast the player can slow down")]
        [Range(0.0F, 1.0F)]
        public float runningTractionMultiplier = 0.85f;
        public float runningChangeDirectionMultiplier = 0.9f;
        public float maxFallVelocity;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            Physics2D.gravity *= gravityMultiplier;
        }

        public void FixedUpdate()
        {
            if (_rigidbody2D.velocity.y < 0 && _rigidbody2D.velocity.y < maxFallVelocity)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * jumpFallMultiplier);
            }
        }

        public void OnJump(bool pressed = true)
        {
            if (pressed)
            {
                _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else if (!pressed && _rigidbody2D.velocity.y > 0)
            {
                // this will get called again when the button is released
                // this will let the player do a small jump or a big jump based on when they release the button
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * jumpUpSlowDownMultiplier);
            }
        }

        public void OnMove(Vector2 input)
        {
            UpdateMomentumVector(input);
        }

        public void UpdateMovement()
        {
            if (_rigidbody2D.velocity.magnitude < maxVelocity)
            {
                _rigidbody2D.AddForce(_momentumVector * thrust, ForceMode2D.Force);
            }

            if (_momentumVector == Vector2.zero)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * runningTractionMultiplier, _rigidbody2D.velocity.y);
            } else if (ChangingDirection())
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * runningChangeDirectionMultiplier, _rigidbody2D.velocity.y);
            }
        }

        private void UpdateMomentumVector(Vector2 input)
        {
            _momentumVector = new Vector2(input.x, 0);
        }

        public void ZeroVelocity()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        public void ZeroYVelocity()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0F);
        }

        private bool ChangingDirection()
        {
            return (_rigidbody2D.velocity.x < 0 && _momentumVector.x > 0) ||
                   (_rigidbody2D.velocity.x > 0 && _momentumVector.x < 0);
        }
        
    }
}