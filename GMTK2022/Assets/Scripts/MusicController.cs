using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource au;

    public AudioSource reminder;
    public AudioClip[] clips;

    public PlayerHealth hp;
    void Start()
    {
        au.loop=false;
        StartCoroutine(MainLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if(hp.health!=0)
        {
            reminder.volume=1/hp.health;
        }
        else
        {
            reminder.volume=1;
        }
        
        
        
    }
    IEnumerator MainLoop()
        {
            au.clip=clips[0];
            au.Play();
            yield return new WaitForSeconds(clips[0].length);
            au.clip=clips[1];
            au.loop=true;
            au.Play();
        }
    
}
