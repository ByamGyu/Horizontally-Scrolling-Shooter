using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // �̱���(static ���) ��ü
    public static SoundManager instance;

    public AudioSource _AudioSource_BGM; // ��� �����
    public AudioClip[] _BGMLists; // ��� ����Ʈ
    public StringAudioClip _MyDic_SoundEffects;
    public StringAudioClip _MyDic_BGMs;


    private void Awake()
    {
        if (instance == null) // ����Ŵ����� ������ ���� �����
        {
            instance = this;
            DontDestroyOnLoad(this);

            // ����Ƽ���� �����ϴ� �� �Ŵ��� Ŭ����(OnSceneLoaded�� �ۼ��� �Լ�)
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject); // ������ ������ �ı�?
    }

    // ���� ������ ���� �´� BGM�� ����ȴ�.
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < _BGMLists.Length; i++)
        {
            //���� �̸��� �´� ����� ����ϴ� �Լ�
            //arg0�� ���� �̸���(�� �̸� == ��� �̸� �̿��� ��)
            if (arg0.name == _BGMLists[i].name)
            {
                PlayBGM(_BGMLists[i]);

                Debug.Log("BGM Name: " + _BGMLists[i].name);
                break;
            }
        }
    }

    // ȿ���� ��� �Լ�(����� �ҽ��� �׶� �׶� ����� �ı�)
    public void PlaySoundEffect(string name, AudioClip _clip)
    {
        if(_clip == null)
        {
            Debug.Log("No SoundEffect: " + _clip.name + "File");
            return;
        }
        else
        {
            // name + "Sound"��� �̸��� ������Ʈ�� ������ش�.
            GameObject go = new GameObject(name + "Sound");
            // ����⸦ ������Ʈ�� �ٿ��ش�.
            AudioSource audiosource = go.AddComponent<AudioSource>();
            // ����� ������ �ٿ��ش�.
            // PlayOneShot�� ����ϸ� ȿ���� ��ø ����� �����ϴ�. �� ������ �Ұ���
            audiosource.PlayOneShot(_clip);

            // ������ ����� �ı�
            Destroy(go, _clip.length);
        }
    }

    // ��� ��� �Լ�(���� �Ŵ����� �ִ� ����� ������ ���)
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
