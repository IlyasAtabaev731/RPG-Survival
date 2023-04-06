using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _discardingPower = 10;
    
    private bool _hitRecharged = true;
    
    private List<Monster> _currentTargets;
    
    private IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(_character.AttackCooldown);
        _hitRecharged = true;
    }

    private void Awake()
    {
        _currentTargets = new List<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Monster monster) && _character.isAttacking && !_currentTargets.Contains(monster))
        {
            Vector3 velocity = _character.MoveDirection;
            monster.GetHit(_damage, _character.MoveDirection * _discardingPower);
            _currentTargets.Add(monster);
            // _hitRecharged = false;
            // StartCoroutine(HitCooldown());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Monster monster) && _character.isAttacking && _currentTargets.Contains(monster))
        {
            _currentTargets.Remove(monster);
        }
    }
}
