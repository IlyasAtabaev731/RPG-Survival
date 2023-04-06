using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar: MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Camera _camera;
    [SerializeField] private Slider _healthBarSlider;
    
    private void Start()
    {
        _healthBarSlider.maxValue = _character.health.MaxHp;
        _healthBarSlider.minValue = 0;
        _character.health.OnHealthChange += UpdateFilledArea;
        _character.health.OnHealthChange += () => Debug.Log("Got hit");
    }

    private void UpdateFilledArea()
    {
        _healthBarSlider.value = _character.health.Hp;
    }

    private void Update()
    {
        _healthBarSlider.value = _character.health.Hp;
        transform.rotation = _camera.transform.rotation;
    }

    private void OnDisable()
    {
        _character.health.OnHealthChange -= UpdateFilledArea;
    }
}
