using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AimAssist : MonoBehaviour
    {

        public float currentAngle = 0.0F;

        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        public void OnAim(Vector2 input)
        {
            currentAngle = CalculateAngle(input);
        }

        private float CalculateAngle(Vector2 input)
        {
            float angle;
        
            if (input.y < 0 && input.x < 0)
            {
                angle = 180;
            }
            else if (input.y < 0 && input.x > 0)
            {
                angle = 0;
            }
            else
            {
                angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            }
        
            return angle;
        }
    
    }
}
