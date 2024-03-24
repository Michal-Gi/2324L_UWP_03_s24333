using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWalker : MonoBehaviour
{

    private Transform[] waypoints;
    [SerializeField] public Transform player;
    private float walkspeed = 1;
    private float chasespeed = 3;
    private float spotradius = 3;
    private float chaseradius = 3;
    [SerializeField] public AudioClip[] _chaseClips;
    [SerializeField] public AudioClip _startChaseClip;
    private float _chaseClipsBaseCooldown = 10f;
    private float _chaseClipsCooldownVariance = 5f;
    private float _chaseClipsCooldown = 0f;

    private Rigidbody2D _rb;
    private SimpleHpSystem _hpSystem;

    private int _waypointChosen = 0;

    private bool _performJump;
    private bool _isGrounded;
    private float _jumpForce = 5;

    private bool _ischasing = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hpSystem = GetComponent<SimpleHpSystem>();
    }

    void Start()
    {

        transform.position = waypoints[_waypointChosen].transform.position;
    }

    void Update()
    {
        if (_hpSystem.Alive)
        {
            Walk();
            if (_rb.velocity.x > 0)
            {
                var scale = _rb.transform.localScale.y;
                
                
                //TODO: ?
                
                _rb.transform.localScale = new Vector3(scale * -1, scale, scale);
            }
            else
            {
                var scale = _rb.transform.localScale.y;
                _rb.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }

    private void FixedUpdate()
    {
        if(_chaseClipsCooldown > 0f)
        {
            _chaseClipsCooldown -= Time.fixedDeltaTime;
        }
        else if (_ischasing && _chaseClipsCooldown <= 0f)
        {
            if (_chaseClips.Any())
            {
                SoundFXManager.Instance.PlaySound(_chaseClips[UnityEngine.Random.Range(0, _chaseClips.Length)], transform, 0.8f);
                _chaseClipsCooldown = _chaseClipsBaseCooldown + UnityEngine.Random.Range(-_chaseClipsCooldownVariance, _chaseClipsCooldownVariance);
            }
        }
        else if (_performJump & _isGrounded)
        {
            _performJump = false;
            _isGrounded = false;
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
        _isGrounded = true;

    private void Walk()
    {

        if (Vector3.Distance(player.position, transform.position) < spotradius ||
            (_ischasing & Vector3.Distance(player.position, transform.position) < spotradius + chaseradius))
        {
            if (!_ischasing && _startChaseClip != null)
            {
                SoundFXManager.Instance.PlaySound(_startChaseClip, transform, 0.8f);
                _chaseClipsCooldown = _chaseClipsBaseCooldown + UnityEngine.Random.Range(-_chaseClipsCooldownVariance, _chaseClipsCooldownVariance);
            }
            _ischasing = true;
            _rb.velocity = new Vector2(
                ((transform.position.x - player.position.x <= 0) ? 1 : (-1)) * chasespeed,
                _rb.velocity.y);
        }
        else
        {
            _ischasing = false;

            _rb.velocity = new Vector2(
                (transform.position.x - waypoints[_waypointChosen].transform.position.x <= 0) ? 1 : (-1) * walkspeed,
                _rb.velocity.y);


            _performJump = false;
            if (Math.Abs(transform.position.x - waypoints[_waypointChosen].transform.position.x) < 0.01)
            {

                _waypointChosen++;
                if (_waypointChosen == waypoints.Length)
                {
                    _waypointChosen = 0;
                }
            }
            else
            {

                if (waypoints[_waypointChosen].transform.position.y - transform.position.y > 0.5)
                {
                    _performJump = true;
                }
            }
        }
    }
}