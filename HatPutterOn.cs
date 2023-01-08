using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatPutterOn : MonoBehaviour
{
    [SerializeField] bool _putOnHat;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().PutOnHat(_putOnHat);
        GameManager.Instance.IsWearingHat = _putOnHat;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponentInParent<PlayerController>().PutOnHat(_putOnHat);
        GameManager.Instance.IsWearingHat = _putOnHat;

    }
}
