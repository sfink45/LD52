using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip[] _tracks;

    AudioSource _source;
    Tracks? _currentTrack;

    public enum Tracks { GardenTheme, BattleTheme }

    private static MusicManager _instance;
    public static MusicManager Instance { get => _instance; }
    private IEnumerator coroutine;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            if (_source == null) _source = GetComponent<AudioSource>();
            if (_source.clip == null) PlayTrack(Tracks.GardenTheme);
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayTrack(Tracks track)
    {
        if (_currentTrack == null || _currentTrack != track) _currentTrack = track;
        else return;
        switch (track)
        {
            case Tracks.BattleTheme:
                coroutine = PlayBattleTheme();
                StartCoroutine(coroutine);
                break;
            default:
                _source.loop = true;
                _source.clip = _tracks[0];
                _source.Play();
                if(_source.volume < 1)
                {
                    _source.DOFade(1, 1);
                }
                return;
        }

    }

    private IEnumerator PlayBattleTheme()
    {
        if (_source.clip != null) yield return null;
        _source.loop = false;
        _source.clip = _tracks[1];
        _source.Play();
        _source.volume = 1;

        yield return new WaitForSeconds(_tracks[1].length + .5f);
        Debug.Log(_currentTrack);
        if (_currentTrack != Tracks.GardenTheme)
        {
            _source.loop = true;
            _source.clip = _tracks[2];
            _source.Play();
        }
    }

    public void FadeOut()
    {
        _source.DOFade(0, 3).OnComplete(() => _source.clip = null);
    }
}
