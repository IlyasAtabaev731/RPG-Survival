using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private float _timeToSurvive = 100f;
    [SerializeField] private TMP_Text _timeLabel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _defeatPanel;
    [SerializeField] private Character _character;

    private void OnEnable()
    {
        _character.OnCharacterDeath += Defeat;
    }

    private void Defeat()
    {
        Time.timeScale = 0f;
        _defeatPanel.SetActive(true);
    }

    private void Win()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        _timeToSurvive -= Time.deltaTime;
        _timeLabel.text = $"Осталось времени выжить: {(int)_timeToSurvive}";

        if (_timeToSurvive <= 0)
        {
            Win();
        }
    }

    private void OnDisable()
    {
        _character.OnCharacterDeath -= Defeat;
    }
}
