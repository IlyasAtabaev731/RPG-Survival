using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar: MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Camera _camera;
    [SerializeField] private Slider _healthBarSlider;

    private void Awake()
    {
        _healthBarSlider.maxValue = _character.MaxHp;
        _healthBarSlider.minValue = 0;
        UpdateFilledArea();
    }

    private void OnEnable()
    {
        _character.OnHealthChange += UpdateFilledArea;
        _character.OnHealthChange += () => Debug.Log("Got hit");
    }

    private void UpdateFilledArea()
    {
        _healthBarSlider.value = _character.Hp;
    }

    private void Update()
    {
        _healthBarSlider.value = _character.Hp;
        transform.rotation = _camera.transform.rotation;
    }

    private void OnDisable()
    {
        _character.OnHealthChange -= UpdateFilledArea;
    }
}
