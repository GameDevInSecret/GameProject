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

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            Physics2D.gravity *= gravityMultiplier;
        }

        public void OnJump()
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        }

        private void UpdateMomentumVector(Vector2 input)
        {
            _momentumVector = new Vector2(input.x, 0);
        }
        
    }
}