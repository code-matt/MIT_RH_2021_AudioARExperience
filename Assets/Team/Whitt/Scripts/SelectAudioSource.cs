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

    bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = demoSong;
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

            while (!(Microphone.GetPosition(null) > 0)) { }
            _source.Play();
            isPlaying = true;
        }

        if (showStartUI == false && useMic == false)
        {
            _clip = demoSong;
            _source.loop = true;
            //  _source.Play();
            isPlaying = true;
        }
    }

    public void DeactivateAudio()
    {
        _source.Stop();
        isPlaying = false;
        if (useMic)
            Microphone.End("Built-In Microphone");
        _clip = null;
        showStartUI = true;
    }

    public void EnterPlaylistMode()
    {
        useMic = false;

        showStartUI = false;
    }

    public void EnterAmbientAudioMode()
    {
        useMic = true;
        showStartUI = false;
    }

    public void ToggleAudio()
    {
        if (isPlaying)
            _source.Pause();

        if (!isPlaying)
            _source.Play();
    }
}