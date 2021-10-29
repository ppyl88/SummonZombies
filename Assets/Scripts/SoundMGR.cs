using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMGR : MonoBehaviour
{
    //public void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}
    //bg는 배경음악
    //eft는 효과음
    //슬라이더의 value값을 가져올 변수
    public Slider bgVolume;
    public Slider eftVolume;

    //AudioSource의 볼륨값을 가져올 변수
    public AudioSource audio1;
    public AudioSource audio2;
    //슬라이더의 값을 유지시키기 위한 변수
    private float bgVol = 1f;
    private float eftVol = 1f;

    public void SoundSlider()
    {
        audio1.volume = bgVolume.value;
        bgVol = bgVolume.value;
        PlayerPrefs.SetFloat("bgvol", bgVol);

        audio2.volume = eftVolume.value;
        eftVol = eftVolume.value;
        PlayerPrefs.SetFloat("eftvol", eftVol);

    }
    void Start()
    {
        //1f를 넣어준 이유는 bgvol 값이 비어있을때 
        //0이 대입되서 음량이 꺼지지 않도록 하기위함
        bgVol = PlayerPrefs.GetFloat("bgvol", 1f);
        bgVolume.value = bgVol;
        GetComponent<AudioSource>().volume = bgVolume.value;

        eftVol = PlayerPrefs.GetFloat("eftvol", 1f);
        eftVolume.value = eftVol;
        GetComponent<AudioSource>().volume = eftVolume.value;
    }
    void Update()
    {
        SoundSlider();
    }

}
