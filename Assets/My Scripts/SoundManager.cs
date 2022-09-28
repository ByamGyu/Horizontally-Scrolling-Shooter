using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 음원들
    public AudioClip _audio1;
    public AudioClip _audio2;
    public AudioClip _audio3;
    public AudioClip _audio4;
    public AudioClip _audio5;
    public AudioClip _audio6;
    public AudioClip _audio7;
    public AudioClip _audio8;
    public AudioClip _audio9;
    public AudioClip _audio_BlackHole;

    // 브금들
    public AudioClip _BGM1;
    public AudioClip _BGM2;
    public AudioClip _BGM3;

    // 음원 재생기
    AudioSource _BGMPlayer;
    AudioSource _SoundEffectPlayer;


    private void Awake()
    {
        _BGMPlayer = GetComponent<AudioSource>();
        _SoundEffectPlayer = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(string Name)
    {
        switch(Name)
        {
            case "BlackHole":
                _SoundEffectPlayer.clip = _audio_BlackHole;
                break;
        }

        _SoundEffectPlayer.Play();
    }

    public void PlayerBGM(string Name)
    {
        switch (Name)
        {
            case "BGM1":
                _BGMPlayer.clip = _BGM1;
                break;
            case "BGM2":
                _BGMPlayer.clip = _BGM2;
                break;
            case "BGM3":
                _BGMPlayer.clip = _BGM3;
                break;
        }

        _BGMPlayer.Play();
    }

    public void StopAllSound(string Name)
    {
        if (_SoundEffectPlayer.isPlaying == true) _SoundEffectPlayer.Stop();
        if (_BGMPlayer.isPlaying == true) _BGMPlayer.Stop();
    }
}
