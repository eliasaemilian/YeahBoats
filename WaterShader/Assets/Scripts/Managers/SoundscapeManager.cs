using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class MusicEvent : UnityEvent<int, float, float> { }
[Serializable] public class SoundEvent : UnityEvent<int> { }
public class SoundscapeManager : MonoBehaviour
{
    public List<AudioClip> Sounds;
    public List<AudioClip> AmbientSounds;
    public List<AudioClip> Music;

    public static MusicEvent QueueNewMusic;
    public static MusicEvent QueueNewAmbientSounds;
    public static SoundEvent PlaySound;

    private AudioSource[] _ambientMusicPlayers;
    private AudioSource _soundsPlayer;

    private int activePlayerIndex;

    private IEnumerator[] _crossfaders = new IEnumerator[2];

    // Start is called before the first frame update
    void Awake()
    {
        QueueNewMusic = new MusicEvent();
        QueueNewAmbientSounds = new MusicEvent();

        PlaySound = new SoundEvent();

        QueueNewMusic.AddListener(OnQueueNewMusic);
        QueueNewAmbientSounds.AddListener(OnQueueNewAmbient);
        PlaySound.AddListener(OnPlaySound);

        InitializeAudioSources();
    }



    private void InitializeAudioSources()
    {
        if (Sounds.Count > 0)
        {
            _soundsPlayer = gameObject.AddComponent<AudioSource>();
            _soundsPlayer.loop = true;
            _soundsPlayer.playOnAwake = false;
            _soundsPlayer.volume = 0.0f;
        }
        if (AmbientSounds.Count > 0 || Music.Count > 0)
        {
            _ambientMusicPlayers = new AudioSource[2];
            for (int i = 0; i < _ambientMusicPlayers.Length; i++)
            {
                _ambientMusicPlayers[i] = gameObject.AddComponent<AudioSource>();

                _ambientMusicPlayers[i].loop = true;
                _ambientMusicPlayers[i].playOnAwake = false;
                _ambientMusicPlayers[i].volume = 0.0f;
            }
        }


    }

    // [ -> https://jwiese.eu/en/blog/2017/06/unity-3d-cross-fade-two-audioclips-with-two-audiosources/ ]
    public float volumeChangesPerSecond = 10;
    IEnumerator FadeAudioSource(AudioSource player, float duration, float targetVolume, Action finishedCallback)
    {
        //Calculate the steps
        int Steps = (int)(volumeChangesPerSecond * duration);
        float StepTime = duration / Steps;
        float StepSize = (targetVolume - player.volume) / Steps;

        //Fade now
        for (int i = 1; i < Steps; i++)
        {
            player.volume += StepSize;
            yield return new WaitForSeconds(StepTime);
        }
        //Make sure the targetVolume is set
        player.volume = targetVolume;

        //Callback
        finishedCallback?.Invoke();

    }

    private void OnQueueNewMusic(int index, float duration, float volume) => MergeNewToPlay(Music[index], duration, volume);
    private void OnQueueNewAmbient(int index, float duration, float volume) => MergeNewToPlay(AmbientSounds[index], duration, volume);


    private void OnPlaySound(int index)
    {
        // Play a new Sound
        _soundsPlayer.PlayOneShot(Sounds[index]);
    }


    private int activePlayer;
    public void MergeNewToPlay(AudioClip clip, float fadeDuration, float volume)
    {
        //Prevent fading the same clip on both players 
        if (clip == _ambientMusicPlayers[activePlayer].clip) return;

        //Kill all playing
        foreach (IEnumerator i in _crossfaders)
        {
            if (i != null)
            {
                StopCoroutine(i);
            }
        }

        //Fade-out the active play, if it is not silent (eg: first start)
        if (_ambientMusicPlayers[activePlayer].volume > 0)
        {
            _crossfaders[0] = FadeAudioSource(_ambientMusicPlayers[activePlayer], fadeDuration, 0.0f, () => { _crossfaders[0] = null; });
            StartCoroutine(_crossfaders[0]);
        }

        //Fade-in the new clip
        int NextPlayer = (activePlayer + 1) % _ambientMusicPlayers.Length;
        _ambientMusicPlayers[NextPlayer].clip = clip;
        _ambientMusicPlayers[NextPlayer].Play();
        _crossfaders[1] = FadeAudioSource(_ambientMusicPlayers[NextPlayer], fadeDuration, volume, () => { _crossfaders[1] = null; });
        StartCoroutine(_crossfaders[1]);

        //Register new active player
        activePlayer = NextPlayer;
    }

    public void DebugMusic()
    {
        Debug.Log("Playing new Music");
        QueueNewMusic.Invoke(0, 10, 1);
    }

    public void DebugAmbient()
    {
        Debug.Log("Playing new Ambient");
        QueueNewAmbientSounds.Invoke(0, 10, 1);
    }

}

