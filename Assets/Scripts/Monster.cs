using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Monster : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private DamageInfoEffect _damageInfoEffect;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private int _hitPoints = 30;
    [SerializeField] private float _speedMovement = 2f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _attackSpeed = 1f;

    public Character Target;
    private bool _hitRecharged = true;

    public void GetHit(int takenDamage, Vector3 takenVelocity)
    {
        _hitPoints -= takenDamage;
        DamageInfoEffect damageInfoEffect = Instantiate(_damageInfoEffect, transform.position, Quaternion.identity);
        damageInfoEffect.Init(takenDamage);
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
        if (_hitPoints <= 0)
        {
            Die();
        }

        _rigidbody.velocity = takenVelocity;
    }

    private void Update()
    {
        transform.LookAt(Target.transform);
        if (_rigidbody.velocity.magnitude != 0)
        {
            Vector3 deltaPos = transform.forward * _speedMovement * Time.deltaTime;
            transform.position += deltaPos;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(_attackSpeed);
        _hitRecharged = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character) && _hitRecharged)
        {
            character.GetHit(_damage);
            _hitRecharged = false;
            StartCoroutine(HitCooldown());
        }
    }
}
