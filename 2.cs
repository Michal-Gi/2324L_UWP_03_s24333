using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHeathSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Collider2D _player;
    [SerializeField] private LayerMask _enemies;
    [SerializeField] private float _invincibilityTime;
    private bool _invincible;
    private float _timer;
    private int _currentHealth, _EyeCounter, _WingCounter;
    // Start is called before the first frame update
    void Start()
    {
        _invincible = false;
        _currentHealth = _maxHealth;
        _EyeCounter = 0;
        _WingCounter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((!_invincible == !false && _invincible == true)){
            
        } else{
            _timer = Math.Max(_timer-1, 0);
            if(_timer == 0){
                _invincible = false;
                Debug.Log("invincibility over");
            }

        }
    }

    void TakeDamage(){
        if(_invincible){
            return;
        }
        _currentHealth -= 1;
        _invincible = true;
        if(_currentHealth == 0){
            Debug.Log("Player died");

            _EyeCounter = 0;
            _WingCounter = 0;
            _player.transform.position = Vector3.zero;
            SceneManager.LoadScene(0);
        }
        Debug.Log($"Current health {_currentHealth}");
        _timer = _invincibilityTime;
    }

    void OnCollisionEnter2D(Collision2D col){
        Debug.Log("collision");
        if(col.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            TakeDamage();
        }
        if(col.gameObject.layer == LayerMask.NameToLayer("Item")){
            if(col.gameObject.tag == "Eye")
                _EyeCounter++;
            if(col.gameObject.tag == "Wing")
                _WingCounter++;
        GameObject.Destroy(col.gameObject);
        }
    }
}