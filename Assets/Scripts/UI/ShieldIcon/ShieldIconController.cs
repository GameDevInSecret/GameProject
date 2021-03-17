using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldIconController : MonoBehaviour
{
    public RectTransform parent;
    public Camera mainCamera;
    private RectTransform thisRT;
    private float _counter;
    private Vector2 _parentDim;
    private Vector2 _iconDim;
    private Vector2 _iconHalfDim;
    [SerializeField] private bool _shieldActive = false;
    [SerializeField] private float mangle;
    private Transform _playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        _counter = 0;
        _parentDim.x = parent.rect.width;
        _parentDim.y = parent.rect.height;

        thisRT = GetComponent<RectTransform>();
        _iconDim = new Vector2(thisRT.rect.width, thisRT.rect.height);
        _iconHalfDim = _iconDim / 2;
        
        _playerTransform = GameObject.Find("Player Prefab").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        print("ICON");
        _parentDim.x = parent.rect.width;
        _parentDim.y = parent.rect.height;

        GameObject[] shields = GameObject.FindGameObjectsWithTag("Shield");
        if (shields.Length > 0)
        {
            Vector2 shieldScreenSpace = RectTransformUtility.WorldToScreenPoint(mainCamera, shields[0].transform.position);
            Vector2 playerScreenSpace = RectTransformUtility.WorldToScreenPoint(mainCamera, _playerTransform.position);
            Vector2 playerToShield = shieldScreenSpace - playerScreenSpace;

            if (ShieldOffScreen(shieldScreenSpace))
            {
                _shieldActive = true;
                thisRT.position = FindOffscreenPoint(playerScreenSpace, shieldScreenSpace, playerToShield);
                // thisRT.position = FindOffscreenPoint(shieldScreenSpace);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                _shieldActive = false;
                transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else
        {
            print("No shield");
            _shieldActive = false;
            transform.localScale = new Vector3(0, 0, 0);
        }
    }

    // private void OnDrawGizmos()
    // {
    //     if (_shieldActive)
    //     {
    //         GameObject[] shields = GameObject.FindGameObjectsWithTag("Shield");
    //         Vector2 shieldScreenSpace = RectTransformUtility.WorldToScreenPoint(mainCamera, shields[0].transform.position);
    //         Vector2 playerScreenSpace = RectTransformUtility.WorldToScreenPoint(mainCamera, _playerTransform.position);
    //         Vector2 playerToShield = shieldScreenSpace - playerScreenSpace;
    //         
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawRay(_playerTransform.position, playerToShield);
    //     }
    // }

    private bool ShieldOffScreen(Vector2 pos)
    {
        return (pos.x > _parentDim.x || pos.x < 0) || (pos.y > _parentDim.y || pos.y < 0);
    }

    private Vector2 FindOffscreenPoint(Vector2 shield)
    {
        Vector2 screenPos = shield;
        
        // b = 0
        // y = mx + b
        float angle = Mathf.Atan2(shield.y, shield.x);
        angle -= 90 * Mathf.Deg2Rad;

        float cos = Mathf.Cos(angle);
        float sin = -Mathf.Sin(angle);

        // cos = adj / hpy
        // sin = opp / hyp
        // cos / sin
        //      = (adj / hyp) / (opp / hyp)
        //      = (adj*hyp) / (opp*hyp)
        //      = (adj/opp)
        //      = m 
        
        float m = cos / sin;

        // set the bounds to be just inside the edge of the screen
        Vector2 screenBounds = _parentDim * 0.95f;

        if (cos > 0)
        {
            screenPos = new Vector2(screenBounds.y / m, screenBounds.y);
        }
        else
        {
            screenPos = new Vector2(-screenBounds.y / m, -screenBounds.y);
        }

        if (screenPos.x > screenBounds.x)
        {
            screenPos = new Vector2(screenBounds.x, screenBounds.x * m);
        } else if (screenPos.x < -screenBounds.x)
        {
            screenPos = new Vector2(-screenBounds.x, -screenBounds.x * m);
        }
        

        return screenPos;
    }

    private Vector2 FindOffscreenPoint(Vector2 player, Vector2 shield, Vector2 direction)
    {
        Vector2 screenPos = shield; // set the screenPos to the shield's position
        Vector2 screenCenter = _parentDim / 2F; // get the center of the screen
        screenPos -= screenCenter; // move the coordinates to the center so that (0,0) isn't in the bottom left
        Vector2 screenBounds = screenCenter * 0.9F; // set the bounds to just inside the edge of the screen

        // m = (y2 - y1) / (x2 - x1)
        // formula to find the slope between 2 points
        float rise = shield.y - player.y;
        float run = shield.x - player.x;
        float m = rise / run;

        // y = mx
        // x = y / m
        // use these formulas to solve for x and y depending on where the shield is
        
        // the shield is above the bounds
        // screenBounds.y / m = the x value when interception the y bounds
        if (screenPos.y > screenBounds.y) screenPos = new Vector2(screenBounds.y / m, screenBounds.y);
        // the shield is below the bounds
        else if (screenPos.y < -screenBounds.y) screenPos = new Vector2(-screenBounds.y / m, -screenBounds.y);

        // the shield is to the right of the right bound
        // screenBounds.x * m = the y value when intercepting the x bounds
        if (screenPos.x > screenBounds.x) screenPos = new Vector2(screenBounds.x, screenBounds.x * m);
        // the shield is to the left of the left bound
        else if ( screenPos.x <  -screenBounds.x ) screenPos = new Vector2(-screenBounds.x, -screenBounds.x * m);

        return screenPos + screenCenter;
    }
}
