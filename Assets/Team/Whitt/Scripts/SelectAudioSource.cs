using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SelectAudioSource : MonoBehaviour
{
    AudioClip _clip = null;

    public AudioClip demoSong;

    AudioSource _source = null;

    public bool useMic = false;

    public bool showStartUI = true;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        SetAudioSource();
    }

    public void SetAudioSource()
    {
        if (showStartUI == false && useMic == true)
        {
            _clip = Microphone.Start("Built-In Microphone", true, 10, 44100);
            _source.loop = true;
            
            while(!(Microphone.GetPosition(null) > 0)) { }
            _source.Play();
        }

        if (showStartUI == false && useMic == false)
        {
            _clip = demoSong;
            _source.loop = true;
            _source.Play();
        }
    }

    public void DeactivateAudio()
    {
        _source.Stop();
        if (useMic)
            Microphone.End("Built-In Microphone");
        _clip = null;
        showStartUI = true;
    }
}
