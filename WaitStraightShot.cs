using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitStraightShot : BaseProjectileController
{
    public Vector2 ShotDirection { get; set; }
    public float ShotSpeed { get; set; }
    public float Lifetime { get; set; }
    public float WaitTime { get; set; }
    protected float _currentWaitTime = 0;
    protected float _currentLifetime = 0;
    protected bool _hasWaited = false;

    private void Update()
    {
        if(_hasWaited)
        {
            _currentLifetime += Time.deltaTime;
            if (_currentLifetime >= Lifetime) Destroy(gameObject);

            transform.position += (Vector3)ShotDirection * ShotSpeed * Time.deltaTime;
        }
        else
        {
            _currentWaitTime += Time.deltaTime;
            _hasWaited = _currentWaitTime >= WaitTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit player!");
            Destroy(gameObject);
        }
    }

}
