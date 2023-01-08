using Assets.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProjectile : StraightShotProjectile
{
    [SerializeField] EnemyBaseController _spawnedEnemy;
    public float PlantLifetime { get; set; }

    bool _isPlanted = false;

    Tweener _activeTweener;

    private void Update()
    {
        _currentLifetime += Time.deltaTime + Random.Range(0, Time.deltaTime);
        if (_currentLifetime >= Lifetime)
        {
            if(!_isPlanted)
            {
                _isPlanted = true;
                _activeTweener = transform.DOShakePosition(Random.Range(1, 3))
                        .OnComplete(() => {
                            var s = Instantiate(_spawnedEnemy, transform.position, Quaternion.AngleAxis(Random.Range(0, 361), Vector3.forward));
                            s.ShouldInvokeEvent = false;
                            Destroy(gameObject);
                        });
            }
        }
        else
        {
            transform.position += (Vector3)ShotDirection * ShotSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit player!");
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Scythe")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(_activeTweener?.active ?? false)
        {
            _activeTweener.Kill();
        }
    }
}
