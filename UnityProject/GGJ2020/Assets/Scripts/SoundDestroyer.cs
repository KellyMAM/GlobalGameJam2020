using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundDestroyer : MonoBehaviour
{
    AudioSource audio;
    private bool started = false;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        started = true;
    }
    void Update()
    {
        if (started && !audio.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}