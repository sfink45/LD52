using Assets;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] HealthChangeEvent _onHealthChanged;
    [SerializeField] SafetyHat _hat;

    public void PutOnHat(bool putOnHat)
    {
        _hat.gameObject.SetActive(putOnHat);
    }

    [SerializeField] int _maxHealth;
    [SerializeField] float _invincibilityTime;
    [SerializeField] int _flashesOnHit;
    int _currentHealth;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _scytheThrowSpeed;
    [SerializeField] float _scytheThrowDistance;
    [SerializeField] GameObject _playerSprite;
    Rigidbody2D _rigidBody;
    
    ScytheController _scythe;
    Camera _mainCam;

    Vector3 _movementVector;

    bool _canBeDamaged = true;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _scythe = GetComponentInChildren<ScytheController>();
        _scythe.IsHeld = true;
        _scythe.ThrowDistance = _scytheThrowDistance;
        _scythe.ThrowSpeed = _scytheThrowSpeed;

        _mainCam = Camera.main;

        _currentHealth = _maxHealth;
        _onHealthChanged.AddListener(GameManager.Instance.HandlePlayerHealthChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementVector != Vector3.zero) transform.position += _movementVector * Time.deltaTime;
    }

    public void OnMove(InputValue val)
    {
        var moveVector = val.Get<Vector2>() * _moveSpeed;
        _movementVector = new Vector3(moveVector.x, moveVector.y, 0);
    }

    public void OnThrow()
    {
        var cursorWorldPos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        cursorWorldPos.z = _mainCam.nearClipPlane;
        if (_scythe.IsHeld) _scythe.Throw(cursorWorldPos);
    }

    public void OnMelee()
    {
        if (!_scythe.IsHeld) TeleportToScythe();
    }

    private void TeleportToScythe()
    {
        transform.position = _scythe.transform.position;
        _playerSprite.transform.localScale = new Vector3(.1f, .1f, 1);
        _playerSprite.transform.DOScale(new Vector3(1, 1, 1), .2f);
        _scythe.GrabReset();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_canBeDamaged && collision.gameObject.tag == "Enemy")
        {
            OnDamage();
            _currentHealth--;
            _onHealthChanged.Invoke(_currentHealth, _maxHealth);
        }
    }

    private void OnDamage()
    {
        _canBeDamaged = false;
        var sr = GetComponentInChildren<SpriteRenderer>();
        var sequence = DOTween.Sequence();
        float timePerFlash = _invincibilityTime / _flashesOnHit;
        for(float i = 0; i < _invincibilityTime; i += timePerFlash * 2)
        {
            sequence.Append(sr.DOFade(0, timePerFlash));
            sequence.Append(sr.DOFade(1, timePerFlash));
        }
        sequence.Play().OnComplete(() => _canBeDamaged = true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_canBeDamaged && collision.gameObject.tag == "Enemy")
        {
            OnDamage();
            _currentHealth--;
            _onHealthChanged.Invoke(_currentHealth, _maxHealth);
        }
    }
}
