using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Character _targetCharacter;
    [SerializeField] private Monster _monsterPrefab;
    [SerializeField] private const int _monstersPerTick = 3;
    [SerializeField] private const float _timeInSecondsForTick = 10f;
    [SerializeField] private const float _maxRangeToSpawn = 30f;
    [SerializeField] private const float _minRangeToSpawn = 10f;

    private float _time;

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            for (int i = 0; i < _monstersPerTick; i++)
            {
                int randomXDir = (int)Math.Pow((double)-1, (double)Math.Ceiling(Random.Range(-1f, 1f)));
                int randomZDir = (int)Math.Pow((double)-1, (double)Math.Ceiling(Random.Range(-1f, 1f)));

                Monster monster = Instantiate(
                    _monsterPrefab,
                    _targetCharacter.transform.position +
                                                                new Vector3(
                                                                  randomXDir * Random.Range(_minRangeToSpawn, _maxRangeToSpawn),
                                                                  0f,
                                                                  randomZDir * Random.Range(_minRangeToSpawn, _maxRangeToSpawn)
                                                                  ), Quaternion.identity);
                monster.Target = _targetCharacter;
            }

            _time = _timeInSecondsForTick;
        }
    }
}
