using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnerController : MonoBehaviour
{
    [SerializeField] float _timeBeforeBossSpawn;
    [SerializeField] float _leafSpawnTime;
    [SerializeField] GameObject _leafPrefab;
    float _currentTime = 0;
    bool _isSpawning = false;
    Rigidbody2D _rb;
    GameManager _gm;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gm = GameManager.Instance;
    }

    private void Update()
    {
        if(!_isSpawning)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _leafSpawnTime)
            {
                SpawnLeaf();
            }
        }
    }

    private void SpawnLeaf()
    {
        _leafPrefab.SetActive(true);
    }

    private void SpawnBoss()
    {
        _gm.TriggerBoss();
        _isSpawning = true;
        Destroy(_leafPrefab.gameObject);
        transform.DOShakeScale(_timeBeforeBossSpawn, .5f, 5,90, true, ShakeRandomnessMode.Full).OnComplete(
            () =>
            {
                _gm.SpawnBoss();
                Destroy(gameObject);
            });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _rb.simulated = false;
        if (collision.gameObject.tag == "Leaf") Destroy(collision.gameObject);
        SpawnBoss();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rb.simulated = false;
        if (collision.gameObject.tag == "Leaf") Destroy(collision.gameObject);
        SpawnBoss();
    }
}
