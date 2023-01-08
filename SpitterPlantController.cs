using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterPlantController : EnemyBaseController
{
    [SerializeField] Transform _projectileSpawnPoint;
    [SerializeField] StraightShotProjectile _projectile;
    [SerializeField] float _shotCooldown;
    [SerializeField] Vector2 _shotDirection;
    protected new bool _shouldInvokeEvent = false;
    float _currentCooldown = 0f;
    private void Update()
    {
        _currentCooldown += Time.deltaTime;
        if(_currentCooldown >= _shotCooldown)
        {
            ShootProjectile();
            _currentCooldown = 0;
        }
    }

    private void ShootProjectile()
    {
        var projectile = Instantiate(_projectile, new Vector3(_projectileSpawnPoint.position.x, _projectileSpawnPoint.position.y, 0), Quaternion.identity);
        projectile.Lifetime = 15f;
        projectile.ShotSpeed = 2.5f;
        projectile.ShotDirection = (-transform.position).normalized;
    }

    protected override void HandleDamage()
    {
        Debug.Log("Damaged!");
    }

    protected override void HandleDeath()
    {
        Debug.Log("Death");
        Destroy(gameObject);
    }
}
