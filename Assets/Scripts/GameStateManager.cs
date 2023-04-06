using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeLabel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _defeatPanel;
    [SerializeField] private Character _character;
    private float _time = 0f;

    private void OnEnable()
    {
        _character.health.OnDeath += Defeat;
    }

    private void Defeat()
    {
        // Time.timeScale = 0f;
        // _defeatPanel.SetActive(true);
    }

    private void Win()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _timeLabel.text = $"Вам удалось выжить в течении: {(int)_time}";
    }

    private void OnDisable()
    {
        _character.health.OnDeath -= Defeat;
    }
}
