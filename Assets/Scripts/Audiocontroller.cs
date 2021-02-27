using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiocontroller : MonoBehaviour
{
    //private static GameObject AudioCreated; // avoid duplicate when change scene

    private void Awake()
    {
        GameObject[] tempGo = GameObject.FindGameObjectsWithTag("AudioSource");

        if(tempGo.Length > 1)
        {
            Destroy(gameObject); // avoid replicate this object and start music again
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void playBackgorundMusic()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void playBackgorundMusicWithDelay(float _delay)
    {
        gameObject.GetComponent<AudioSource>().PlayDelayed(_delay);
    }

    public void stopBackgorundMusic()
    {
        gameObject.GetComponent<AudioSource>().Stop();
    }
    
    public bool BackgorundMusicisPlaying()
    {
        return gameObject.GetComponent<AudioSource>().isPlaying;
    }
}
