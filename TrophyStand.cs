using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrophyStand : MonoBehaviour
{
    [SerializeField] int _bossIndex;

    [SerializeField]GameObject _trophyObject;
    [SerializeField]SpriteRenderer _trophySprite;
    //[SerializeField] ParticleSystem _particles;

    private void Awake()
    {
        var seeds = GameManager.Instance.BossSeeds;
        var tuple = seeds.FirstOrDefault(s => s.bossIndex == _bossIndex);
        if (tuple.seed != null)
        {
            _trophyObject.SetActive(true);
            _trophySprite.color = tuple.seed.GetColor();
            //var p = _particles.main;
            //p.startColor = tuple.seed.GetColor();
        }
        else
        {
            _trophyObject.SetActive(false);
        }
    }
}
