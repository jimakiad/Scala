using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerTransform;
    public float heightThreshold = 10f;
    public string messageToShow = "You've reached the specified height!";
    private bool hasDisplayedMessage = false;
    public bool destinationReached = false;
    private int _totalCoinsCollected = 0;
    public int totalCoinsCollected
    {
        get { return _totalCoinsCollected; }
        set
        {
            _totalCoinsCollected = value;
            if (value > 0)
            {
                PlayCoinCollectSound();
            }

            // Check if totalCoinsCollected is equal to 7
            if (value == 7)
            {
                PlaySpecialSound();

            }
        }
    }


    public AudioClip destinationReachedSound;
    public AudioClip backgroundMusic;
    public AudioClip coinCollectSound; // New sound for coin collection
    public AudioClip heightReachedSound; // New sound for reaching the specified height
    public AudioClip specialSound; // New sound for when totalCoinsCollected is 7

    void Start()
    {
        // Set up background music audio source
        AudioSource bgAudioSource = gameObject.AddComponent<AudioSource>();
        bgAudioSource.clip = backgroundMusic;
        bgAudioSource.loop = true;
        bgAudioSource.volume = 0.28f;
        bgAudioSource.Play();
    }

    void Update()
    {
        if (playerTransform.position.y >= heightThreshold && !hasDisplayedMessage)
        {
            DisplayMessage(messageToShow);
            PlayHeightReachedSound(); // Play the new sound for reaching the specified height
            hasDisplayedMessage = true;
        }

        // Check if destinationReached is true
        if (destinationReached)
        {
            PlayDestinationReachedSound(); // Play the sound for reaching the destination
            destinationReached = false; // Reset the flag to prevent continuous playing
        }
    }

    void PlayDestinationReachedSound()
    {
        PlaySound(destinationReachedSound);
    }

    void DisplayMessage(string message)
    {
        Debug.Log(message);
    }

    void PlayCoinCollectSound()
    {
        PlaySound(coinCollectSound);
    }

    void PlayHeightReachedSound()
    {
        PlaySound(heightReachedSound);
    }

    void PlaySpecialSound()
    {
        PlaySound(specialSound);
    }



    void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            // Instantiate a new GameObject for playing the sound
            GameObject soundObject = new GameObject("SoundObject");
            AudioSource soundSource = soundObject.AddComponent<AudioSource>();
            soundSource.clip = sound;

            // Play the sound
            soundSource.Play();

            // Destroy the sound object after playing the sound
            Destroy(soundObject, sound.length);
        }
        else
        {
            Debug.LogError("Sound is not assigned in the GameManager.");
        }
    }
}
