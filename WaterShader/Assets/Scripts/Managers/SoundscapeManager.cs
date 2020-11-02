using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class MusicEvent : UnityEvent<int> { }
[Serializable] public class SoundEvent : UnityEvent<int> { }
public class SoundscapeManager : MonoBehaviour
{
    public List<AudioClip> Sounds;
    public List<AudioClip> AmbientSounds;
    public List<AudioClip> Music;

    public static MusicEvent QueueNewMusic;
    public static MusicEvent QueueNewAmbientSounds;
    public static SoundEvent PlaySound;

    [SerializeField] private float _fadeDuration = 5f;

    private float VolumeSound
    {
        get
        {
            if (SettingsHandler.RequestSetting(SettingsHandler.SoundVol, out float soundVol))
            {
                return soundVol;
            }
            else return 1f;
        }
    }

    private float VolumeMusic
    {
        get
        {
            if (SettingsHandler.RequestSetting(SettingsHandler.MusicVol, out float musicVol))
            {
                return musicVol;
            }
            else return 1f;
        }
    }

    private AudioSource[] _ambientMusicPlayers;
    private AudioSource _soundsPlayer;

    private IEnumerator[] _crossfaders = new IEnumerator[2];

    private int activePlayer; // Ambient Music Player currently playing


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


    /// <summary>
    /// Setup Audio Sources for Ambient Crossfade and Sounds
    /// </summary>
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

 
    private readonly float volChangesPerSecond = 10;
    /// <summary>
    /// Fades between 2 Audiosources
    /// [ -> https://jwiese.eu/en/blog/2017/06/unity-3d-cross-fade-two-audioclips-with-two-audiosources/ ]
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeAudioSource(AudioSource player, float duration, float targetVolume, Action finishedCallback)
    {
        //Calculate the steps
        int Steps = (int)(volChangesPerSecond * duration);
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

    private void OnQueueNewMusic(int index) => MergeNewToPlay(Music[index]);
    private void OnQueueNewAmbient(int index) => MergeNewToPlay(AmbientSounds[index]);
    private void OnPlaySound(int index)
    {
        if (SettingsHandler.RequestSetting(SettingsHandler.Sound, out bool soundOnOff))
        {
            if (!soundOnOff) return;
        }

        // Play a new Sound
        _soundsPlayer.PlayOneShot(Sounds[index]);
        _soundsPlayer.volume = VolumeSound;
    }

    /// <summary>
    /// Queue new Audioclip to fade with currently playing Ambient
    /// </summary>
    /// <param name="clip"></param>
    public void MergeNewToPlay(AudioClip clip)
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
            _crossfaders[0] = FadeAudioSource(_ambientMusicPlayers[activePlayer], _fadeDuration, 0.0f, () => { _crossfaders[0] = null; });
            StartCoroutine(_crossfaders[0]);
        }

        //Fade-in the new clip
        int NextPlayer = (activePlayer + 1) % _ambientMusicPlayers.Length;
        _ambientMusicPlayers[NextPlayer].clip = clip;
        _ambientMusicPlayers[NextPlayer].Play();
        _crossfaders[1] = FadeAudioSource(_ambientMusicPlayers[NextPlayer], _fadeDuration, VolumeMusic, () => { _crossfaders[1] = null; });
        StartCoroutine(_crossfaders[1]);

        //Register new active player
        activePlayer = NextPlayer;
    }

    public void KillAllMusic() => _ambientMusicPlayers[activePlayer].Stop();
    public void ResumeMusic() => _ambientMusicPlayers[activePlayer].Play();
    public void VolumeChanged() => _ambientMusicPlayers[activePlayer].volume = VolumeMusic;

}

