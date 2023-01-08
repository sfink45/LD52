using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShotProjectile : BaseProjectileController
{
    public Vector2 ShotDirection { get; set; }
    public float ShotSpeed { get; set; }
    public float Lifetime { get; set; }

    protected float _currentLifetime = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        _currentLifetime += Time.deltaTime;
        if (_currentLifetime >= Lifetime) Destroy(gameObject);

        transform.position += (Vector3)ShotDirection * ShotSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit player!");
            Destroy(gameObject);
        }
    }

}
