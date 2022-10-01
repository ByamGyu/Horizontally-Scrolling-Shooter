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

    public string[] _BGMNames; // ��� �̸��� ����
    public AudioClip[] _BGMs; // ��ݵ� ����
    public string[] _SoundEffectNames; // ȿ������ �̸� ����
    public AudioClip[] _SoundEffects; // ȿ������ ����

    // ��ųʸ� �ڷᱸ��
    public Dictionary<string, AudioClip> _DicBGMStorage = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> _DicSoundEffectStorage = new Dictionary<string, AudioClip>();


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


        InitDictionary(); // �迭�� ���� ������ ��ųʸ� ���·� ����
    }

    void InitDictionary() // ��ųʸ� �ʱ�ȭ
    {
        // �� �� �Լ� �������!

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

    // ���� ������ ���� �´� BGM�� ����ȴ�.
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        string tmp_name = arg0.name;

        Debug.Log(tmp_name);

        PlayBGM(tmp_name, 0.5f);
    }

    // ȿ���� ��� �Լ�(����� �ҽ��� �׶� �׶� ����� �ı�)
    public void PlaySoundEffect(string name)
    {
        if(_DicSoundEffectStorage.ContainsKey(name))
        {
            GameObject go = new GameObject("SoundObject: " + name);
            AudioSource audiosource = go.AddComponent<AudioSource>();
            audiosource.PlayOneShot(_DicSoundEffectStorage[name]);

            Destroy(go, _DicSoundEffectStorage[name].length);
        }
        else
        {
            Debug.Log("SoundEffect: " + name + "is not exist.");
        }
    }

    // ��� ��� �Լ�(���� �Ŵ����� �ִ� ����� ������ ���)
    public void PlayBGM(string name, float volume)
    {
        if (_DicBGMStorage.ContainsKey(name))
        {
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