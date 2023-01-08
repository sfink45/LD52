using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image _enemyHealthBar;
    [SerializeField] Image _playerHealthBar;
    [SerializeField] GameObject _playerHud;
    [SerializeField] GameObject _enemyHud;
    [SerializeField] TextMeshProUGUI _bossName;
    [SerializeField] DisableCompletionHandler _completionHandler;
    [SerializeField] public Image FadeToBlack;

    private static UIManager _instance;
    public static UIManager Instance { get => _instance; }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowBattleUI(string bossName)
    {
        _playerHud.SetActive(true);
        _bossName.text = bossName;
        _enemyHud.SetActive(true);
    }

    public void HideBattleUI()
    {
        _playerHud.SetActive(false);
        _enemyHud.SetActive(false);

    }

    public void UpdateEnemyHealth(int current, int max)
    {
        _enemyHealthBar.fillAmount = (float)current / (float)max;
    }

    public void UpdatePlayerHealth(int current, int max)
    {
        _playerHealthBar.fillAmount = (float)current / (float)max;

    }

    public void HandleCompletion(Color color)
    {
        _completionHandler.gameObject.SetActive(true);
        _completionHandler.OnCompletion(color);
    }

    public void ResetStuff()
    {
        _enemyHealthBar.fillAmount = 1;
        _playerHealthBar.fillAmount = 1;
        _bossName.text = "";
    }
}
