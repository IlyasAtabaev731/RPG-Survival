using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private const int _damage = 1;
    [SerializeField] private const int _discardingPower = 100;
    
    private bool _hitRecharged = true;
    
    private IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(_character.AttackCooldown);
        _hitRecharged = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Monster monster) && _character.isAttacking && _hitRecharged)
        {
            Vector3 velocity = _character.MoveDirection;
            velocity.y = 1f; 
            monster.GetHit(_damage, _character.MoveDirection * _discardingPower);
            _hitRecharged = false;
            StartCoroutine(HitCooldown());
        }
    }
}
