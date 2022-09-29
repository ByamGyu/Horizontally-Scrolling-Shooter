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
    public AudioClip[] _BGMLists; // 브금 리스트
    public StringAudioClip _MyDic_SoundEffects;
    public StringAudioClip _MyDic_BGMs;


    private void Awake()
    {
        if (instance == null) // 사운드매니저가 없으면 새로 만들기
        {
            instance = this;
            DontDestroyOnLoad(this);

            // 유니티에서 제공하는 씬 매니저 클래스(OnSceneLoaded는 작성한 함수)
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject); // 있으면 기존걸 파괴?
    }

    // 씬이 열리면 씬에 맞는 BGM이 재생된다.
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < _BGMLists.Length; i++)
        {
            //씬의 이름에 맞는 브금을 재생하는 함수
            //arg0은 씬의 이름임(씬 이름 == 브금 이름 이여야 함)
            if (arg0.name == _BGMLists[i].name)
            {
                PlayBGM(_BGMLists[i]);

                Debug.Log("BGM Name: " + _BGMLists[i].name);
                break;
            }
        }
    }

    // 효과음 재생 함수(오디오 소스를 그때 그때 만들고 파괴)
    public void PlaySoundEffect(string name, AudioClip _clip)
    {
        if(_clip == null)
        {
            Debug.Log("No SoundEffect: " + _clip.name + "File");
            return;
        }
        else
        {
            // name + "Sound"라는 이름의 오브젝트를 만들어준다.
            GameObject go = new GameObject(name + "Sound");
            // 재생기를 오브젝트에 붙여준다.
            AudioSource audiosource = go.AddComponent<AudioSource>();
            // 재생할 음원을 붙여준다.
            // PlayOneShot을 사용하면 효과음 중첩 재생이 가능하다. 단 정지는 불가능
            audiosource.PlayOneShot(_clip);

            // 끝나면 재생기 파괴
            Destroy(go, _clip.length);
        }
    }

    // 브금 재생 함수(사운드 매니저에 있는 재생기 변수를 사용)
    public void PlayBGM(AudioClip _clip)
    {
        if (_clip == null)
        {
            Debug.Log("No BGM: " + _clip.name + "File");
            return;
        }
        else
        {
            if (_AudioSource_BGM.isPlaying == true) _AudioSource_BGM.Stop();

            _AudioSource_BGM.clip = _clip;
            _AudioSource_BGM.loop = true;
            _AudioSource_BGM.volume = 0.2f;
            _AudioSource_BGM.Play();
        }
    }

    public void StopBGM()
    {
        if (_AudioSource_BGM.isPlaying == true) _AudioSource_BGM.Stop();
    }

    public void ReStartBGM()
    {
        if (_AudioSource_BGM.isPlaying == false) _AudioSource_BGM.Play();
    }
}
