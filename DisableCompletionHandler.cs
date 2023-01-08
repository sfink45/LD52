using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisableCompletionHandler : MonoBehaviour
{
    TextMeshProUGUI[] _childTexts;

    private void Awake()
    {
        if(_childTexts == null)
        {
            _childTexts = GetComponentsInChildren<TextMeshProUGUI>();
            gameObject.SetActive(false);
        }
    }

    public void OnAnimationDone()
    {
        gameObject.SetActive(false);
    }

    public void OnCompletion(Color color)
    {
        gameObject.SetActive(true);
        foreach(var c in _childTexts)
        {
            c.color = color;
        }
    }
}
