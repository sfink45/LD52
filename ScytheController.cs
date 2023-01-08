using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheController : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;
    [SerializeField] Transform _centerPoint;
    public float ThrowDistance {get; set;}
    public float ThrowSpeed { get; set; }

    Rigidbody2D _rigidBody;

    Vector2 _initialPos;
    Vector2 _forceVector;
    bool _isReturning = false;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.simulated = false;
    }

    private void Update()
    {
        if (!IsHeld)
        {
            transform.Rotate(0, 0, -_rotationSpeed);
            //Debug.Log(Vector2.Distance(_initialPos, transform.position) < ThrowDistance);
            if (Vector2.Distance(_centerPoint.position, transform.position) < ThrowDistance && !_isReturning)
            {
                transform.position += (Vector3)_forceVector * Time.deltaTime;
            }
            else
            {
                _isReturning = true;
                _forceVector = (_centerPoint.position - transform.position).normalized * ThrowSpeed;
                transform.position += (Vector3)_forceVector * Time.deltaTime;
                if(Vector2.Distance(transform.position, _centerPoint.position) <= 0.5)
                {
                    GrabReset();
                }
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }

    public bool IsHeld { get; set; }

    public void Throw(Vector2 targetPosition)
    {
        IsHeld = false;
        var throwPosition = targetPosition - (Vector2)_centerPoint.position;
        _forceVector =  throwPosition.normalized * ThrowSpeed;
        _rigidBody.simulated = true;
    }

    public void GrabReset()
    {
        IsHeld = true;
        _isReturning = false;
        _forceVector = Vector2.zero;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _rigidBody.simulated = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Scythe Bump");
    }
}
