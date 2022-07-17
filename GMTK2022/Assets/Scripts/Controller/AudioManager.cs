using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioManager Instance;

	public AudioSource soundSource;

    public AudioSource dicesoundSource;
    public AudioSource dicesoundSource2;
    public AudioSource musicSource;

	void Awake(){
		if (Instance == null){
			Instance = this;
			DontDestroyOnLoad(this);
		} else {
			GameObject.Destroy(this);
		}
	}
    public void playMusic(AudioClip music)
    {
        musicSource.clip=music;
        musicSource.Play();  

    }
     public void playsound(AudioClip sound)
    {
        musicSource.clip=sound;
        musicSource.Play();  

    }
     public void Diceplaysound(AudioClip sound)
    {
        dicesoundSource.clip=sound;
        dicesoundSource.Play();  

    }
     public void Diceplaysound2(AudioClip sound)
    {
        dicesoundSource2.clip=sound;
        dicesoundSource2.Play();  

    }



}
