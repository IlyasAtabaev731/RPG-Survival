using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Character _targetCharacter;
    [SerializeField] private Monster[] _monsterPrefabs;
    [SerializeField] private int _monstersPerTick = 3;
    [SerializeField] private float _timeInSecondsForTick = 10f;
    [SerializeField] private float _maxRangeToSpawn = 30f;
    [SerializeField] private float _minRangeToSpawn = 10f;

    private float _time;

    private void Spawn()
    {
        int randomXDir = (int)Math.Pow((double)-1, (double)Math.Ceiling(Random.Range(-1f, 1f)));
        int randomZDir = (int)Math.Pow((double)-1, (double)Math.Ceiling(Random.Range(-1f, 1f)));

        Monster _monsterPrefab = _monsterPrefabs[Random.Range(0, _monsterPrefabs.Length)];
        
        Monster monster = Instantiate(
            _monsterPrefab,
            _targetCharacter.transform.position +
            new Vector3(
                randomXDir * Random.Range(_minRangeToSpawn, _maxRangeToSpawn),
                0f,
                randomZDir * Random.Range(_minRangeToSpawn, _maxRangeToSpawn)
            ), Quaternion.identity);
        monster.Target = _targetCharacter;
        // yield return null;
    }
    
    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            for (int i = 0; i < _monstersPerTick; i++)
            {
                Spawn();
            }

            _time = _timeInSecondsForTick;
        }
    }
}
