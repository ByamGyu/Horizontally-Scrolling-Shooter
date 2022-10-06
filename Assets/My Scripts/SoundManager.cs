using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // 싱글톤(static 사용) 객체
    public static SoundManager instance;

    public AudioSource _AudioSource_BGM; // 브금 재생기
    public AudioSource _AudioSource_SoundEffect; // 효과음 재생기

    public string[] _BGMNames; // 브금 이름들 저장
    public AudioClip[] _BGMs; // 브금들 저장
    public string[] _SoundEffectNames; // 효과음들 이름 저장
    public AudioClip[] _SoundEffects; // 효과음들 저장

    // 딕셔너리 자료구조
    public Dictionary<string, AudioClip> _DicBGMStorage = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> _DicSoundEffectStorage = new Dictionary<string, AudioClip>();


    private void Awake()
    {
        // 오디오 소스(재생기) 객체를 만들고 사운드 매니저 자식으로 달아주기
        GameObject gotmp1 = new GameObject("AudioSource: BGM");
        _AudioSource_BGM = gotmp1.AddComponent<AudioSource>();
        gotmp1.transform.parent = this.transform;

        // 오디오 소스(재생기) 객체를 만들고 사운드 매니저 자식으로 달아주기
        GameObject gotmp2 = new GameObject("AudioSource: SoundEffect");
        _AudioSource_SoundEffect = gotmp2.AddComponent<AudioSource>();
        gotmp2.transform.parent = this.transform;

        if (instance == null) // 사운드매니저가 없으면 새로 만들기
        {
            instance = this;
            DontDestroyOnLoad(this);

            // 유니티에서 제공하는 씬 매니저 클래스(OnSceneLoaded는 작성한 함수)
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject); // 있으면 기존걸 파괴?


        InitDictionary(); // 배열들 내부 정보를 딕셔너리 형태로 정리
    }

    void InitDictionary() // 딕셔너리 초기화
    {
        // 꼭 이 함수 실행시켜!

        if (_DicBGMStorage.Count != 0) _DicBGMStorage.Clear();
        if (_DicSoundEffectStorage.Count != 0) _DicSoundEffectStorage.Clear();

        for (int i = 0; i < _BGMNames.Length; i++)
        {
            string tmp_name = _BGMNames[i];
            AudioClip tmp_clip = _BGMs[i];

            _DicBGMStorage.Add(tmp_name, tmp_clip);
        }

        for(int i = 0; i < _SoundEffectNames.Length; i++)
        {
            string tmp_name = _SoundEffectNames[i];
            AudioClip tmp_clip = _SoundEffects[i];

            _DicSoundEffectStorage.Add(tmp_name, tmp_clip);
        }
    }

    // 씬이 열리면 씬에 맞는 BGM이 재생된다.
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        string tmp_name = arg0.name;

        Debug.Log(tmp_name);

        PlayBGM(tmp_name, 0.5f);
    }

    // 효과음 재생 함수(오디오 소스를 그때 그때 만들고 파괴)
    // 중첩을 허용함(대신 끊을 수 없음)
    public void PlaySoundEffectOneShot(string name, float _volume = 1.0f)
    {
        if(_DicSoundEffectStorage.ContainsKey(name))
        {
            GameObject go = new GameObject("SoundObject: " + name);
            AudioSource audiosource = go.AddComponent<AudioSource>();            
            audiosource.PlayOneShot(_DicSoundEffectStorage[name], _volume);

            Destroy(go, _DicSoundEffectStorage[name].length);
        }
        else
        {
            Debug.Log("SoundEffect: " + name + "is not exist.");
        }
    }

    // 중첩을 허용하지 않음(대신 끊을 수 있음)
    public void PlaySoundEffectbyAudioSource(string name, bool _loop, float volume)
    {
        if (_DicSoundEffectStorage.ContainsKey(name))
        {
            if (_AudioSource_SoundEffect.isPlaying)
            {
                // 동일한 효과음이 재생중이면 return
                if (_AudioSource_SoundEffect.clip.name == name) return;
                // 다른 효과음이면 기존 효과음을 멈춤
                else _AudioSource_SoundEffect.Stop();
            }

            _AudioSource_SoundEffect.clip = _DicSoundEffectStorage[name];
            _AudioSource_SoundEffect.loop = _loop;
            _AudioSource_SoundEffect.volume = volume;
            _AudioSource_SoundEffect.Play();
        }
        else
        {
            Debug.Log("SoundEffect: " + name + "is not exist.");
        }
    }

    // 브금 재생 함수(사운드 매니저에 있는 재생기 변수를 사용)
    public void PlayBGM(string name, float volume)
    {
        if (_DicBGMStorage.ContainsKey(name))
        {
            if(_AudioSource_BGM.isPlaying == true)
            {
                _AudioSource_BGM.Stop();
            }

            _AudioSource_BGM.clip = _DicBGMStorage[name];
            _AudioSource_BGM.loop = true;
            _AudioSource_BGM.volume = volume;
            _AudioSource_BGM.Play();
        }
        else
        {
            Debug.Log("BGM: " + name + "is not exist.");
        }
    }

    public void StopSoundEffectAudioSource()
    {
        if (_AudioSource_SoundEffect.isPlaying == true) _AudioSource_SoundEffect.Stop();
    }

    public void StopBGM()
    {
        if (_AudioSource_BGM.isPlaying == true) _AudioSource_BGM.Stop();
    }

    public void PauseBGM()
    {
        if (_AudioSource_BGM.isPlaying == true) _AudioSource_BGM.Pause();
    }

    public void RestartBGM()
    {
        if (_AudioSource_BGM.isPlaying == false) _AudioSource_BGM.Play();
    }
}