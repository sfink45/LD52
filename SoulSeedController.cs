using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSeedController : MonoBehaviour
{
    [SerializeField] Color _color;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FindObjectOfType<GameManager>().SeedAcquired(_color, gameObject.name);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Scythe") return;
        transform.parent = collision.gameObject.transform;
        transform.localPosition = Vector3.zero;
    }

    public Color GetColor() => _color;
}
