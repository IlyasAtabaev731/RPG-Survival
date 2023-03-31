using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [SerializeField] private const float _speed = 3f;
    [SerializeField] private const float _attackAnimationTime = 0.7f;
    [SerializeField] private const float _attackCooldown = 0.8f;

    [SerializeField] private int _maxHitPoints = 15;

    private int _hitPoints;

    public bool isAttacking = false;
    public bool isRecharged = false;
    public bool isCombed = false;
    
    private Vector3 _lastDeltaPos;

    public event Action OnHealthChange;

    public event Action OnCharacterDeath;

    public int MaxHp => _maxHitPoints;

    public int Hp => _hitPoints;

    public Vector3 MoveDirection => transform.forward;

    public float AttackCooldown => _attackCooldown;

    private States State
    {
        get => (States)_animator.GetInteger("state");
        set => _animator.SetInteger("state", (int)value);
    }

    public void GetHit(int takenDamage)
    {
        _hitPoints -= takenDamage;
        
        OnHealthChange?.Invoke();

        if (_hitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnCharacterDeath?.Invoke();
        Destroy(gameObject);
    }

    private void Attack()
    {
        if (isRecharged)
        {
            if (!isCombed)
            {
                State = States.MeleeAttack1;
            }
            else
            {
                State = States.MeleeAttack2;
            }
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }
    
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(_attackAnimationTime);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        isRecharged = true;
        isCombed = !isCombed;
    }
    
    private void Awake()
    {
        _hitPoints = _maxHitPoints;
        isRecharged = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && isRecharged)
        {
            Attack();
        }
            
        if (!isAttacking)
        {
            Vector3 deltaPos = Input.GetAxis("Vertical") * (new Vector3(0, 0, 1)) * _speed * Time.deltaTime +
                               Input.GetAxis("Horizontal") * (new Vector3(1, 0, 0)) * _speed * Time.deltaTime;
            transform.position += deltaPos;
            if (Mathf.Abs(deltaPos.x) > 0 || Mathf.Abs(deltaPos.z) > 0)
            {
                _lastDeltaPos = deltaPos;
            }
            
            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            {
                State = States.Run;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_lastDeltaPos), Time.deltaTime * 3);
            }
            else
            {
                State = States.Idle;
            }
        }
    }
}

enum States
{
    Idle,
    Run,
    MeleeAttack1,
    MeleeAttack2
}
