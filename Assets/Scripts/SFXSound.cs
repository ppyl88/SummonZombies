using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSound : MonoBehaviour
{
    
    public AudioSource audioPlayer; // 플레이어 소리 재생기

    void Awake()
    {   
        //DontDestroyOnLoad(gameObject);
        audioPlayer = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        audioPlayer.PlayOneShot(audioClip);
    }

}
