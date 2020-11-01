using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class AmbientSoundHandler : MonoBehaviour
{
    private float _timer;
    [SerializeField, Tooltip("Time in Minutes")] private int[] _minMaxTimeBetweenAmbient = new int[2];

    private int nextSwitch;
    private int newTimeToSwitch => Random.Range(_minMaxTimeBetweenAmbient[0], _minMaxTimeBetweenAmbient[1]) * 60;

    [SerializeField] private int[] _indexDayAmbientSounds = null;
    [SerializeField] private int[] _indexNightAmbientSounds = null;

    [SerializeField] private int[] _indexDayMusic = null;
    [SerializeField] private int[] _indexNightMusic = null;

    [SerializeField, Range(0f, 1f)] private float _chanceToPlayMusic = .5f;


    // Start is called before the first frame update
    void Start()
    {
        nextSwitch = newTimeToSwitch;

        // Check for TimeOfDayHandler in Scene
        if (FindObjectOfType<TimeOfDayHandler>() == null) Debug.LogError("Place a Time of Day Handler in the Scene!");

        PlayNewSoundOrMusic();

    }

    // Update is called once per frame
    void Update()
    {
        // run timer
        _timer += Time.deltaTime;

        if (_timer > newTimeToSwitch)
        {
            nextSwitch = newTimeToSwitch;
            _timer = 0f;
            PlayNewSoundOrMusic();

        }
        // every X interval
        // play new ambient


    }

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
