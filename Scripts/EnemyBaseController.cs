using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseController : MonoBehaviour
{
    public string Name { get => _name; }
    [SerializeField] string _name;
    [SerializeField] HealthChangeEvent _onHealthChanged;
    [SerializeField] protected int _maxHealth;
    protected int _currentHealth;
    protected bool _canHitAgain = true;
    public bool ShouldInvokeEvent { get; set; } = true;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        if (ShouldInvokeEvent)
        {
            _onHealthChanged.AddListener(GameManager.Instance.HandleEnemyHealthChange);
        }
    }

    public void Damage(int damage)
    {
        _currentHealth -= damage;
        _onHealthChanged?.Invoke(_currentHealth, _maxHealth);
        if (_currentHealth <= 0)
        {
            HandleDeath();
        }
        else HandleDamage();

    }

    protected abstract void HandleDamage();
    protected abstract void HandleDeath();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scythe")
        {
            if (_canHitAgain)
            {
                Debug.Log("Scythe Hit?");
                _canHitAgain = false;
                Damage(1);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scythe")
        {
            _canHitAgain = true;
        }
    }

}
