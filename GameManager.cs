using Assets.Scripts;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _currentBossIndex;
    [SerializeField] UIManager _uiManager;
    [SerializeField] SoulSeedController[] _soulSeedPrefabs;

    [SerializeField] EnemyBaseController[] _bosses;
    [SerializeField] EnemyBaseController _currentBoss;

    [SerializeField] Image _fadeToBlack;
    private int _playerTimesDamaged = 0;

    private static GameManager _instance;

    public bool IsWearingHat = false;
    public static GameManager Instance { get => _instance; }
    bool _playerFinished = false;

    public List<(int bossIndex, SoulSeedController seed)> BossSeeds { get; private set; }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            BossSeeds = new List<(int, SoulSeedController)>();
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SeedAcquired(Color color, string acquiredSeedName)
    {
        UIManager.Instance.HandleCompletion(color);
        var seq = DOTween.Sequence();
        seq.Append(UIManager.Instance.FadeToBlack.DOFade(0f, 5));
        seq.Append(UIManager.Instance.FadeToBlack.DOFade(1, 3));
        seq.OnComplete(() =>
        SceneManager.LoadScene("GardenScene", LoadSceneMode.Single));
        seq.Play();
        MusicManager.Instance.FadeOut();
    }

    public void HandleEnemyHealthChange(int current, int max)
    {
        if (_playerFinished) return;
        UIManager.Instance.UpdateEnemyHealth(current, max);
        if (current <= 0)
        {
            _playerFinished = true; ;
            var seedToSpawn = GetSeedReward();
            BossSeeds.Add((_currentBossIndex, seedToSpawn));
            Instantiate(seedToSpawn, Vector3.zero, Quaternion.identity);
            UIManager.Instance.HideBattleUI();

        }
    }

    public void HandlePlayerHealthChange(int current, int max)
    {
        if (_playerFinished) return;
        UIManager.Instance.UpdatePlayerHealth(current, max);
        ++_playerTimesDamaged;
        if (current <= 0)
        {
            _playerFinished = true;
            UIManager.Instance.HideBattleUI();
            MusicManager.Instance.FadeOut();
            UIManager.Instance.FadeToBlack.DOFade(1, 4).OnComplete(()=>
            SceneManager.LoadScene(1));
        }
    }

    public void LoadHarvest(int bossIndex)
    {
        _playerFinished = false;
        _playerTimesDamaged = 0;
        _currentBossIndex = bossIndex;
        MusicManager.Instance.FadeOut();
        UIManager.Instance.ResetStuff();
        UIManager.Instance.FadeToBlack.DOFade(1, 6).OnComplete(() =>
        SceneManager.LoadScene(2));
    }

    public void TriggerBoss()
    {
        MusicManager.Instance.PlayTrack(MusicManager.Tracks.BattleTheme);
    }

    public void SpawnBoss()
    {
        var spawnIndex = _currentBossIndex >= _bosses.Length ? UnityEngine.Random.Range(0, _bosses.Length) : _currentBossIndex;
        _currentBoss = Instantiate(_bosses[spawnIndex]);
        UIManager.Instance.ShowBattleUI(_currentBoss.Name);
    }


    private SoulSeedController GetSeedReward()
    {
        if (_playerTimesDamaged == 0) return _soulSeedPrefabs[0];
        if (_playerTimesDamaged == 1) return _soulSeedPrefabs[1];
        return _soulSeedPrefabs[2];
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            IsWearingHat = false;
            MusicManager.Instance.PlayTrack(MusicManager.Tracks.GardenTheme);
            
        }

        if(level == 2)
        {
            FindObjectOfType<SafetyHat>().gameObject.SetActive(IsWearingHat);
        }
        UIManager.Instance.FadeToBlack.DOFade(0, 2);
    }
}
