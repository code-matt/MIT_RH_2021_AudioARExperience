using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RadioControl : MonoBehaviour
{
    private AudioSource radioSource;

    // Start is called before the first frame update
   private void Start()
    {
        radioSource = GetComponent<AudioSource>();

    }

    public void PlayAudio()
    {
        radioSource.Play();
        Debug.Log("PlayMusic");

    }

    public void PauseAudio()
    {
        radioSource.Pause();
    }


}

