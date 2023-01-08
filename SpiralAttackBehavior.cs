using Assets.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralAttackBehavior : BaseAttackBehavior
{
    [SerializeField] GameObject _center;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _attackDuration;

    [SerializeField] StraightShotProjectile _projectilePrefab;
    [SerializeField] float _projectileLifeTime;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _shotCooldown;
    float _currentCooldown = 0;
    float _currentDuration = 0;

    public override void HandleUpdate(float timeDelta)
    {
        if (IsDone || _isStartUpPlaying) return;
        _currentCooldown += timeDelta;
        if (_currentCooldown >= _shotCooldown)
        {
            foreach (var p in _spawnPoints)
            {
                var projectile = Instantiate(_projectilePrefab, new Vector3(p.position.x, p.position.y, 0), Quaternion.identity);
                projectile.Lifetime = _projectileLifeTime;
                projectile.ShotSpeed = _projectileSpeed;
                projectile.ShotDirection = (p.position - _center.transform.position).normalized;
            }
            _currentCooldown = 0;
        }
        _center.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);

        _currentDuration += timeDelta;
        if (_currentDuration >= _attackDuration) IsDone = true;
    }

    public override void ResetBehavior()
    {
        _currentCooldown = 0;
        _currentDuration = 0;
        IsDone = false;
    }

    public override void StartupAttack(EnemyBaseController parent)
    {
        _isStartUpPlaying = true;
        parent.transform.DOShakeScale(.2f, randomnessMode: ShakeRandomnessMode.Harmonic).OnComplete(() => _isStartUpPlaying = false);
    }
}
