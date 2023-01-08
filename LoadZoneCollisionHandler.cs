using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadZoneCollisionHandler : MonoBehaviour
{
    [SerializeField] int _bossIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.LoadHarvest(_bossIndex);
        }
    }
}
