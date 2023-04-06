using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _bleedHitEffect;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _attackAnimationTime = 0.7f;
    [SerializeField] private float _attackCooldown = 0.8f;
    [SerializeField] private int _maxHitPoints = 100;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _minRangeToAttack = 2f;
    
    
    public Health health;

    public bool isAttacking = false;
    public bool isRecharged = false;
    public bool isCombed = false;
    
    private Vector3 _lastDeltaPos;

    private Transform _attackTarget;
    
    public Vector3 MoveDirection => transform.forward;

    public float AttackCooldown => _attackCooldown;

    private States State
    {
        get => (States)_animator.GetInteger("state");
        set => _animator.SetInteger("state", (int)value);
    }

    public void GetHit(int takenDamage)
    {
        Instantiate(_bleedHitEffect, transform.position + transform.up * 0.3f, Quaternion.identity);
        health.Discard(takenDamage);
    }

    private void Die()
    {
        State = States.DieWithMelee;
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

    private bool AutoClosestTarget(int numberOfRays = 30)
    {
        float angleOfRotate = 360 / numberOfRays;
        float minDist = 1000f;
        Transform targetTransform = transform;
        bool isEnemyFound = false;
        for (int i = 0; i < numberOfRays; i++)
        {
            Vector3 direction = Quaternion.Euler(0, angleOfRotate * i, 0) * transform.forward;
            Ray ray = new Ray(transform.position + transform.up * 0.5f,  direction);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out Monster monster) && hit.distance < minDist)
            {
                targetTransform = monster.transform;
                isEnemyFound = true;
                minDist = hit.distance;
            }
        }

        _attackTarget = targetTransform;
        return isEnemyFound;
    }
    
    private void OnEnable()
    {
        health.OnDeath += Die;
    }

    private void Awake()
    {
        health = new Health(_maxHitPoints);
        isRecharged = true;
    }

    private void Update()
    {
        if (_joystick.Direction.magnitude > 0)
        {
            Vector3 deltaPos = new Vector3();
            deltaPos.x = _joystick.Horizontal;
            deltaPos.z = _joystick.Vertical;
            deltaPos *= _speed * Time.deltaTime;
            
            transform.position += deltaPos;
            
            if (deltaPos.magnitude > 0)
            {
                _lastDeltaPos = deltaPos;
            }
            
            State = States.Run;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_lastDeltaPos), Time.deltaTime * _rotationSpeed);
        }
        else if(AutoClosestTarget() && Vector3.Distance(_attackTarget.position, transform.position) < _minRangeToAttack)
        {
            transform.LookAt(_attackTarget);
            Attack();
        }
        if(!isAttacking && _joystick.Direction.magnitude == 0)
        {
            State = States.Idle;
        }
    }

    private void OnDisable()
    {
        health.OnDeath -= Die;
    }
}

enum States
{
    Idle,
    Run,
    MeleeAttack1,
    MeleeAttack2,
    DieWithMelee
}