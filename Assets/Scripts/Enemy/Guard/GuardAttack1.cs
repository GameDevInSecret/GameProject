using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttack1 : MonoBehaviour
{
    public bool startedAttack;
    public bool performingAttack;
    public bool completedAttack;
    private Damager _damager;
    private bool _canAttack;
    private float _attackTimeElapsed;

    private Transform _spear;
    private Vector2 _attackCheckpoint1; // the position where the spear will be drawn back to before being thrusted
    private Vector2 _attackStartPoint; // the position of the spear tip at the beginning of the attack

    private bool _checkpoint1Reached = false;

    // Start is called before the first frame update
    void Start()
    {
        _damager = GetComponent<Damager>();
        _attackTimeElapsed = 0.0F;
        _spear = transform.Find("Spear");
        _damager.DisableDamager();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startedAttack)
        {
            _attackTimeElapsed += Time.deltaTime;
            // do the attack
            performingAttack = true;

            if (!_checkpoint1Reached)
            {
                _spear.position = Vector2.Lerp(_attackStartPoint, _attackCheckpoint1, _attackTimeElapsed);
                if ((Vector2)_spear.position == _attackCheckpoint1)
                {
                    _checkpoint1Reached = true;
                    _damager.EnableDamager();
                }
            }
            else
            {
                _spear.position = Vector2.Lerp(_attackCheckpoint1, _attackStartPoint, _attackTimeElapsed * 2.5F);
                if ((Vector2)_spear.position == _attackStartPoint)
                {
                    FinishAttack();
                }
            }
            
        }        
    }

    public bool StartAttack()
    {
        _attackStartPoint = transform.Find("Start Position").position;;
        _attackCheckpoint1 = transform.Find("Draw Back Position").position;
        startedAttack = true;
        _canAttack = true;
        return _canAttack;
    }

    public void FinishAttack()
    {
        startedAttack = false;
        performingAttack = false;
        completedAttack = false;
        _attackTimeElapsed = 0.0F;
        _checkpoint1Reached = false;
        _damager.DisableDamager();
    }
}
