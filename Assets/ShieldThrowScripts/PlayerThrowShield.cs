using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerThrowShield : MonoBehaviour
{
    public Vector2 joystick;
    public GameObject arrowPrefab;
    public GameObject shieldPrefab;
    private Vector3 _arrowPos;
    private GameObject _arrowPointer;
    private PlayerController _controller;
    private SpriteRenderer _spriteRenderer;

   
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.GetStateIsAiming())
        {
            _arrowPointer.transform.position = transform.position;
        }
    }
    
    public void ThrowingShield_OnAim(Vector2 direction)
    {
        if (!_controller.GetStateIsAiming())
        {
            _arrowPointer = Instantiate(arrowPrefab, transform.position, arrowPrefab.transform.rotation);
            _controller.SetStateIsAiming(true);
        }
        
        _arrowPos = GameObject.Find("Throw Direction").transform.position;
        joystick = direction;

        float angle = 0.0F;
        if (direction.y < 0 && direction.x < 0)
        {
            angle = 180;
        }
        else if (direction.y < 0 && direction.x > 0)
        {
            angle = 0;
        }
        else if ( direction.x == 0 && direction.y == 0 )
        {
            Destroy(_arrowPointer.gameObject);
            _controller.SetStateIsAiming(false);
        }
        else
        {
            angle = Vector2.Angle(direction, Vector2.right);
        }

        if (_controller.GetStateIsAiming())
        {
            // angleInDegrees = angle;
            _arrowPointer.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
    
    public void ThrowingShield_OnFire()
    {
        // if (_controller.GetStateLookingRight())
        // {
        //     _spriteRenderer.sprite = noShieldSprite;
        // }
        // else
        // {
        //     _spriteRenderer.sprite = noShieldLeftSprite;
        // }

        var shieldInstance = Instantiate(shieldPrefab, _arrowPos, shieldPrefab.transform.rotation);

        shieldInstance.GetComponent<ShieldController>().forceDir = _arrowPointer.transform.right;
        shieldInstance.GetComponent<ShieldController>().ThrowShield();
        
        _controller.SetStateHasShield(false);
        _controller.SetStateIsAiming(false);
        
        Destroy(_arrowPointer.gameObject);
        // _waitingForShieldThrowCooldown = true;
            
        // StartCoroutine(ActionMapTransitionDelay(0.2F, _playerInput.actions.FindActionMap("Player").name));
        // StartCoroutine(ShieldThrowPickupDelay());
            
        // Destroy(arrowPointer.gameObject);
    }
}
