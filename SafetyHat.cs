using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyHat : MonoBehaviour
{
    private void OnEnable()
    {
        var c = GetComponentInParent<Collider2D>();
        Physics2D.IgnoreLayerCollision(6, 7);
    }

    private void OnDisable()
    {
        var c = GetComponentInParent<Collider2D>();
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }
}
