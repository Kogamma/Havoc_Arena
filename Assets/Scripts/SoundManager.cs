using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
    public AudioClip clickSound; // A click sound
    public AudioClip lazerSound; // A sound for shooting lazers
    public AudioClip hornSound;  // A horn sound
    public AudioClip hitSound;   // A sound for getting hit by projectiles
    public AudioClip deathSound; // A sound for getting killed

    public AudioClip currentAudioClip;

    private AudioSource source;  // An audio source

	public void Start ()
    {
        source = GetComponent<AudioSource>();
    }
    

    public void PlayCurrentAudioClip()
    {
        source.PlayOneShot(currentAudioClip);
    }

    // Plays a sound that indicates that the user presses a button
    public void PlayClickSound ()
    {
        source.PlayOneShot(clickSound);
    }
}
