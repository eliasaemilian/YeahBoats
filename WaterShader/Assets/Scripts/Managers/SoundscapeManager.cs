using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class MusicEvent : UnityEvent<int> { }
public class SoundscapeManager : MonoBehaviour
{
    public List<AudioClip> Sounds;
    public List<AudioClip> AmbientSounds;
    public List<AudioClip> Music;

    public static MusicEvent QueueNewMusic;
    public static MusicEvent QueueNewAmbientSounds;
    public static MusicEvent PlaySound;

    // Start is called before the first frame update
    void Awake()
    {
        QueueNewMusic = new MusicEvent();
        QueueNewAmbientSounds = new MusicEvent();
        PlaySound = new MusicEvent();

        QueueNewMusic.AddListener(OnQueueNewMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnQueueNewMusic(int index)
    {
        // fetch Music at index in Music List
        // tell player to merge to this music
    }

    private void OnPlaySound(int index)
    {
        // Play a new Sound
    }

    private void MergeNewSoundToPlay(ref List<AudioClip> sounds, int index)
    {

    }
        
    }
