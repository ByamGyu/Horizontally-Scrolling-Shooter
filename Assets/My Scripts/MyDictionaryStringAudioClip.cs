using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StringAudioClip : SerializableDictionary<string, AudioClip> { }

public class MyDictionaryStringAudioClip : MonoBehaviour
{
    public StringAudioClip _data;


    private void Start()
    {
        Debug.Log("Mydictionasdfkljasldfj: " + _data);
    }
}
