using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AimAssist : MonoBehaviour
    {
        private Camera cam;

        public float currentAngle;
    
        // Start is called before the first frame update
        private void Start()
        {
            cam = Camera.main;
        }

        private void FixedUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
        
            // if mouse input
            if (context.control.name.Equals("position"))
            {
                Vector2 playerPos = cam.WorldToScreenPoint(transform.position);
                input -= playerPos;
            }
        
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
                //angle = Vector2.Angle(input, Vector2.right);
            }
        
            return angle;
        }
    
    }
}
