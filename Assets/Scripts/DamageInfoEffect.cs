using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageInfoEffect : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;

    public void Init(int value)
    {
        _text.text = value.ToString();
        Destroy(gameObject, 1f);
    }

}
