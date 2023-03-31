using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _cameraSpeed = 2f;

    private Vector3 _pos;
    private Vector3 _relateToTargetPos;
    
    private void Awake()
    {
        _relateToTargetPos = transform.position;
    }

    private void LateUpdate()
    {
        _pos = _target.position;
        _pos += _relateToTargetPos;
        transform.position = Vector3.Lerp(transform.position, _pos, Time.deltaTime * _cameraSpeed);
    }
}
