using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player_ThrowShield : MonoBehaviour
{
    public Vector2 joystick;
    public GameObject arrowPrefab;
    public GameObject shieldPrefab;
    private Vector3 arrowPos;
    private GameObject arrowPointer;
    
    // States
    private bool _hasShieldS = true;
    
    // Start is called before the first frame update
    void Start()
    {
        print("STARTING");
        arrowPointer = Instantiate(arrowPrefab, transform.position, arrowPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        arrowPointer.transform.position = transform.position;
    }
    
    public void ThrowingShield_OnAim(InputAction.CallbackContext context)
    {
        arrowPos = GameObject.Find("Throw Direction").transform.position;
        Vector2 direction = context.ReadValue<Vector2>();
        joystick = direction;

        float angle = 0.0F;
        if (direction.y < 0 && direction.x < 0)
        {
            angle = 180;
        }
        else if ( (direction.y < 0 && direction.x > 0) || (direction.y == 0 && direction.x == 0))
        {
            angle = 0;
        }
        else
        {
            angle = Vector2.Angle(direction, Vector2.right);
        }

        // angleInDegrees = angle;
        arrowPointer.transform.rotation = Quaternion.Euler(0, 0, angle);

    }
    
    public void ThrowingShield_OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && _hasShieldS)
        {
            // if (_lookingRightS)
            // {
            //     spriteRenderer.sprite = noShieldSprite;
            // }
            // else
            // {
            //     spriteRenderer.sprite = noShieldLeftSprite;
            // }

            var shieldInstance = Instantiate(shieldPrefab, arrowPos, shieldPrefab.transform.rotation);

            shieldInstance.GetComponent<ShieldController>().forceDir = arrowPointer.transform.right;
            shieldInstance.GetComponent<ShieldController>().ThrowShield();
            // _hasShieldS = false;
            // _aimingShieldS = false;
            // _waitingForShieldThrowCooldown = true;
            
            // StartCoroutine(ActionMapTransitionDelay(0.2F, _playerInput.actions.FindActionMap("Player").name));
            // StartCoroutine(ShieldThrowPickupDelay());
            
            // Destroy(arrowPointer.gameObject);
        }
    }
}
