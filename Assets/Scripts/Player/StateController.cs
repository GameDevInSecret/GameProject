//
// Player states
//

namespace Player
{
    public class StateController
    {
         // movement states
         private bool isGrounded;
         private bool isLookingRight;
           
         // shield stated
         private bool hasShield;
         private bool isAiming;

         public StateController()
         {
             this.isGrounded = true;
             this.isLookingRight = true;
             this.hasShield = true;
             this.isAiming = false;
         }

         public bool IsGrounded
         {
             get => isGrounded;
             set => isGrounded = value;
         }

         public bool IsLookingRight
         {
             get => isLookingRight;
             set => isLookingRight = value;
         }

         public bool HasShield
         {
             get => hasShield;
             set => hasShield = value;
         }

         public bool IsAiming
         {
             get => isAiming;
             set => isAiming = value;
         }
    }
}
