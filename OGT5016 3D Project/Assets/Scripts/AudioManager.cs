using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; //Singleton
    
    public AudioMixer audioMixer; 

    public Slider volume;

    public bool isMenu; //Script works in both menu and in-game but it works different, this is flag for menu
    
    public AudioSource soundSource;
    
    public AudioClip[] audioList;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Always adjust the volume with saved value
        float vol = PlayerPrefs.GetFloat("Volume");
        audioMixer.SetFloat("newVolume", vol );

        //If it is a menu, script adjust the volume slider to saved value
        if (isMenu)
        {
            volume.value = vol;

            soundSource = GetComponent<AudioSource>();

            //plays music for menu
            soundSource.clip = audioList[0];
            soundSource.Play();

        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //audio mixer parameter change 
    public void SetVolume(float newVolume)
    {
        audioMixer.SetFloat("newVolume", newVolume);
        PlayerPrefs.SetFloat("Volume", newVolume);
    }
}
