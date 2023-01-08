using Assets.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLineAttackBehavior : BaseAttackBehavior
{
    [SerializeField] float _dashSpeed;
    [SerializeField] int _maxDashes;
    [SerializeField] float _dashCooldown;
    [SerializeField] WaitStraightShot _projectilePrefab;
    [SerializeField] float _shotSpeed;
    [SerializeField] float _shotCooldown;
    [SerializeField] float _lifetime;
    [SerializeField] float _waitTime;
    [SerializeField] float _dashDistance;

    EnemyPath[] _paths;
    EnemyBaseController _parent;
    SpriteRenderer _parentRenderer;
    Rigidbody2D _parentRB;
    int _dashCount = 0;
    float _currentDashCooldown;
    float _currentShotCooldown;
    Transform _playerTransform;
    Vector2 _targetDirection;
    Vector2 _originalPos;
    [SerializeField] Vector3[] _startPositions;
    Sequence _activeSequence;

    void Awake()
    {
        _paths = GetComponentsInChildren<EnemyPath>();
        if(_paths?.Length == 0)
        {
            _playerTransform = FindObjectOfType<PlayerController>().transform;
        }
        _parent = GetComponentInParent<EnemyBaseController>();
        _parentRenderer = _parent.GetComponentInChildren<SpriteRenderer>();
        _parentRB = _parent.GetComponent<Rigidbody2D>();
        _currentDashCooldown = _dashCooldown;
    }

    public override void HandleUpdate(float timeDelta)
    {
        if (_isStartUpPlaying || IsDone) return;
        if (_dashCount < _maxDashes)
        {
            if (_projectilePrefab != null)
            {
                _currentShotCooldown += timeDelta;
                if (_currentShotCooldown >= _shotCooldown)
                {
                    var projectile = Instantiate(_projectilePrefab, _parent.transform.position, Quaternion.identity);
                    projectile.Lifetime = _lifetime;
                    projectile.ShotSpeed = _shotSpeed;
                    projectile.ShotDirection = (-_parent.transform.position).normalized;
                    projectile.WaitTime = _waitTime;
                    _currentShotCooldown = 0;
                }
            }

            if (_targetDirection == Vector2.zero)
            {
                if (_playerTransform != null)
                {
                    _targetDirection = (_playerTransform.position - _parent.transform.position).normalized;
                    _originalPos = _parent.transform.position;
                }
                else
                {

                }
            }
            if (Vector2.Distance(_parent.transform.position, _originalPos) <= _dashDistance)
            {
                var distanceAdded = _targetDirection * timeDelta * _dashSpeed;
                _parent.transform.position += (Vector3)distanceAdded;
            }
            else
            {
                _dashCount++;
                _targetDirection = Vector2.zero;
                StartupAttack(_parent);
            }

        }
        else
        {
            IsDone = true;
            _parentRB.simulated = false;
            _parentRenderer.color = Color.clear;
            var spawnPos = new Vector2(Random.Range(_startPositions[0].x, _startPositions[1].x), Random.Range(_startPositions[0].y, _startPositions[1].y));
            _parent.transform.position = (Vector3)spawnPos;
            _parentRenderer.DOColor(new Color(1, 1, 1, 1), 1f).OnComplete(() => {
                IsDone = true;
                _parentRB.simulated = true;
                });
        }
    }

    public override void ResetBehavior()
    {
        _dashCount = 0;
        IsDone = false;
        //_parentRenderer.color = new Color(1, 1, 1, 1);
        _activeSequence.Complete();
    }

    public override void StartupAttack(EnemyBaseController parent)
    {
        if (_isStartUpPlaying) return;
        if (_dashCount >= _maxDashes) return;
        _isStartUpPlaying = true;
        var spriteRender = parent.GetComponentInChildren<SpriteRenderer>();
        _parentRB.simulated = false;
        var _activeSequence = DOTween.Sequence();
        _activeSequence.Append(spriteRender.DOFade(0, 1f));
        var spawnPos = new Vector2(Random.Range(_startPositions[0].x, _startPositions[1].x), Random.Range(_startPositions[0].y, _startPositions[1].y));
        _activeSequence.Append(parent.transform.DOMove(spawnPos, 0f));
        _activeSequence.Append(spriteRender.DOFade(1, .5f));

        _activeSequence.Play().OnComplete(() =>
        {
            _isStartUpPlaying = false;
            _parentRB.simulated = true;
        });
    }
}
