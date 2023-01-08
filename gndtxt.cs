using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gndtxt : MonoBehaviour
{
    [SerializeField] Color c1 = new Color(1, .67f, 1, 1);
    [SerializeField] Color c2 = new Color(1, .33f, 1, 1);
    [SerializeField] Color c3 = new Color(1, .33f, 1, 1);

    private void Start()
    {
        var bs = GameManager.Instance.BossSeeds;
        Color color = Color.white;
        if (bs == null || bs.Count == 0) return;
        if (bs.Count == 1) color = c1;
        if (bs.Count == 2) color = c2;
        if (bs.Count == 3) color = c3;




        GetComponent<SpriteRenderer>().color = color;
    }
}
