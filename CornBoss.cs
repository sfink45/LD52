using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornBoss : EnemyBaseController
{
    [SerializeField] BaseAttackBehavior[] _attacks;

    int _currentAttackIndex = -1;
    int _lastAttackIndex = -1;

    private void Update()
    {
        if (_currentAttackIndex < 0)
        {
            _currentAttackIndex = Random.Range(0, _attacks.Length);
            if (_currentAttackIndex == _lastAttackIndex)
                _currentAttackIndex = (_currentAttackIndex + 1) % _attacks.Length;
            _attacks[_currentAttackIndex].StartupAttack(this);
        }
        _attacks[_currentAttackIndex].HandleUpdate(Time.deltaTime);

        if (_attacks[_currentAttackIndex].IsDone)
        {
            _attacks[_currentAttackIndex].ResetBehavior();
            _lastAttackIndex = _currentAttackIndex;
            _currentAttackIndex = -1;
        }
    }

    protected override void HandleDamage()
    {
    }

    protected override void HandleDeath()
    {
        Destroy(gameObject);
    }
}
