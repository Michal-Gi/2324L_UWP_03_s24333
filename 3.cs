using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    //[SerializeField] private Collider2D _platformMovementTrigger;
    [SerializeField] private bool _verticalMovement;
    [SerializeField] private float _negativeRange, _positiveRange, _speed;
    [SerializeField] private Rigidbody2D _body;
    [SerializeField] private Transform _transform;
    private bool _isMoving;
    private float _rangeXRIGHT, _rangeXLEFT, _rangeYUP, _rangeYDOWN;
    private Vector2 _vec;

    private void Start()
    {
        _isMoving = true;
        _rangeXRIGHT = _transform.position.x - _negativeRange;
        _rangeXLEFT = _transform.position.x + _positiveRange;
        _rangeYUP = _transform.position.y + _positiveRange;
        _rangeYDOWN = _transform.position.y - _negativeRange;
        Debug.Log(_rangeXLEFT);
        Debug.Log(_rangeXRIGHT);
        if (_verticalMovement)
            _vec = Vector2.up * _speed;
        else
            _vec = Vector2.left * _speed;
        _body.velocity = _vec;
    }
    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            if (_transform.position.x <= _rangeXRIGHT || _transform.position.y >= _rangeYUP)
            {
                _body.velocity = _vec * -1;
            }
            else if (_transform.position.x >= _rangeXLEFT || _transform.position.y <= _rangeYDOWN)
            {
                _body.velocity = _vec;
            }
        }
        else
            return;
    }

}