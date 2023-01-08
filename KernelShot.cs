using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KernelShot : WaitStraightShot
{
    [SerializeField] StraightShotProjectile _explodedShotPrefab;
    [SerializeField] int _explodeShots;
    [SerializeField] float _explodeShotSpeed;
    [SerializeField] float _explodeShotLifetime;

    private void Update()
    {
        _currentLifetime += Time.deltaTime;
        if(_currentLifetime >= Lifetime)
        {
            _currentWaitTime += Time.deltaTime;
            if(!_hasWaited && _currentWaitTime >= WaitTime)
            {
                _hasWaited = true;
                var d = 0;
                var v = Vector2.right;
                var toSpawn = _explodeShots; /*Random.Range(_explodeShots / 2, _explodeShots * 2);*/
                var dt = 360 / toSpawn;
                for(int i = 0; i <= toSpawn; ++i)
                {
                    var p = Instantiate(_explodedShotPrefab, transform.position, Quaternion.identity);
                    p.Lifetime = _explodeShotLifetime;
                    p.ShotSpeed = _explodeShotSpeed;

                    p.ShotDirection = Quaternion.AngleAxis(d, Vector3.forward) * v;
                    d += dt;
                }
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += (Vector3)ShotDirection * ShotSpeed * Time.deltaTime;
        }
    }
}
