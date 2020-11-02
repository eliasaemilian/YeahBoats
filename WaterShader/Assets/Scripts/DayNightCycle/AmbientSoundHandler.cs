using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class AmbientSoundHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Time in Minutes")] private int[] _minMaxTimeBetweenAmbient = new int[2];

    [SerializeField] private int[] _indexDayAmbientSounds = null;
    [SerializeField] private int[] _indexNightAmbientSounds = null;

    [SerializeField] private int[] _indexDayMusic = null;
    [SerializeField] private int[] _indexNightMusic = null;

    [SerializeField, Range(0f, 1f)] private float _chanceToPlayMusic = .5f;

    private int newTimeToSwitch => Random.Range(_minMaxTimeBetweenAmbient[0], _minMaxTimeBetweenAmbient[1]) * 60;
    private float _timer;


    void Start()
    {
        // Check for TimeOfDayHandler in Scene
        if (FindObjectOfType<TimeOfDayHandler>() == null) Debug.LogError("Place a Time of Day Handler in the Scene!");

        PlayNewSoundOrMusic();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > newTimeToSwitch)
        {
            _timer = 0f;
            PlayNewSoundOrMusic();
        }

    }

    /// <summary>
    /// Select new Ambient Music or Sounds to play in dependence of Time of Day and queue new clip for the Soundscape Manager
    /// </summary>
    private void PlayNewSoundOrMusic()
    {
        float r = Random.Range(0f, 1f);

        if (r > _chanceToPlayMusic)
        {
            int[] sounds = TimeOfDayHandler.IsNight ? _indexNightAmbientSounds : _indexDayAmbientSounds;
            if (sounds.Length <= 0) return;

            int index = Random.Range(0, sounds.Length);
            SoundscapeManager.QueueNewAmbientSounds.Invoke(sounds[index]);

        }
        else
        {
            int[] music = TimeOfDayHandler.IsNight ? _indexNightMusic : _indexDayMusic;
            if (music.Length <= 0) return;

            int index = Random.Range(0, music.Length);
            SoundscapeManager.QueueNewMusic.Invoke(music[index]);
        }
    }

}
