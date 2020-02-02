using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public List<AudioClip> splashClips;
    public List<AudioClip> musicClips;
    private int musicIndex;
    public GameObject tempSoundPrefab;
    AudioSource audio;

	// Use this for initialization
	void Start ()
    {
        audio = GetComponent<AudioSource>();
        musicIndex = 0;
	}

	// Update is called once per frame
	void Update () {
        if (!audio.isPlaying) {
            PlayMusic();
        }
	}

    public void PlayMusic()
    {
        audio.clip = musicClips[musicIndex];
        audio.Play();
        audio.loop = false;

        musicIndex++;
        if (musicIndex >= musicClips.Count) {
            musicIndex = 0;
        }
    }

    public void PlaySplash(GameObject from)
    {
        GameObject splash = (GameObject) Instantiate(tempSoundPrefab, from.transform.position, from.transform.rotation);
        AudioSource audioSource = splash.GetComponent<AudioSource>();
        audioSource.clip = splashClips[Random.Range(0, splashClips.Count)];
        audioSource.Play();
    }
}