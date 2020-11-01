using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider = null;
    [SerializeField] private Slider _musicSlider = null;

    [SerializeField] private TextMeshProUGUI _soundButtonText = null;
    [SerializeField] private TextMeshProUGUI _musicButtonText = null;
    [SerializeField] private TextMeshProUGUI _notifButtonText = null;
    [SerializeField] private TextMeshProUGUI _reverseInputButtonText = null;

    public static readonly string ReverseInput = "ReverseInput";
    public static readonly string Sound = "Sound";
    public static readonly string SoundVol = "SoundVolume";
    public static readonly string Music = "Music";
    public static readonly string MusicVol = "MusicVolume";

    private readonly string notif = "Notifications";

    private SoundscapeManager _soundscapeManager;


    // Start is called before the first frame update
    void Start()
    {
        _soundSlider.onValueChanged.AddListener(delegate { OnChangedVolumeSliderSound(); });
        _musicSlider.onValueChanged.AddListener(delegate { OnChangedVolumeSliderMusic(); });

        InitPreferences();

        _soundscapeManager = FindObjectOfType<SoundscapeManager>();

    }

    private void InitPreferences()
    {
        if (PlayerPrefs.HasKey(notif)) return;

        PlayerPrefs.SetFloat(notif, 0);
        PlayerPrefs.SetFloat(Music, 0);
        PlayerPrefs.SetFloat(MusicVol, 1);
        PlayerPrefs.SetFloat(Sound, 0);
        PlayerPrefs.SetFloat(SoundVol, 1);
    }

    public void OnChangedVolumeSliderSound()
    {
        ChangePreference(SoundVol, _soundSlider.value);
    }

    public void OnChangedVolumeSliderMusic()
    {
        ChangePreference(MusicVol, _musicSlider.value);
    }

    public void OnClickMusicOnOff()
    {
        if (ChangeBoolPref(Music))
        {
            _musicButtonText.text = "Music is On";
            _soundscapeManager.ResumeMusic();
        }
        else 
        {
            _musicButtonText.text = "Music is Off";
            _soundscapeManager.KillAllMusic();
        }

        UpdateSliderVisibility(Music, _musicSlider);


    }

    public void OnClickSoundOnOff()
    {
        if (ChangeBoolPref(Sound)) _soundButtonText.text = "Sounds are On";
        else _soundButtonText.text = "Sounds are Off";

        UpdateSliderVisibility(Sound, _soundSlider);
    }

    public void OnClickNotificationsOnOff()
    {
        if (ChangeBoolPref(notif)) _notifButtonText.text = "Notifications are On";
        else _notifButtonText.text = "Notifications are Off";

        ChangeBoolPref(notif);
    }

    public void OnClickReverseFishingInput()
    {
        if (ChangeBoolPref(Sound)) _reverseInputButtonText.text = "Reverse Boat Input On";
        else _reverseInputButtonText.text = "Reverse Boat Input Off";

        ChangeBoolPref(ReverseInput);
    }

    private void ChangePreference(string key, float value) => PlayerPrefs.SetFloat(key, value);

    public static bool RequestSetting(string key, out float value)
    {
        value = 0f;
        if (!PlayerPrefs.HasKey(key)) return false;
        else
        {
            value = PlayerPrefs.GetFloat(key);
            Debug.Log(key + " is " + value);
            return true;
        }
    }

    public static bool RequestSetting(string key, out bool value)
    {
        value = false;
        if (!PlayerPrefs.HasKey(key)) return false;
        else
        {
            value = ReadBoolPref(key);
            Debug.Log(key + " is " + value);
            return true;
        }
    }

    private bool ChangeBoolPref(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetFloat(key) > 0)
            {
                ChangePreference(key, 0);
                return true;
            }
            else
            {
                ChangePreference(key, 1);
                return false;
            }
        }
        else return false;
    }

    private static bool ReadBoolPref(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetFloat(key) > 0) return false;
            else return true;
        }
        else return false;
    }

    private void UpdateSliderVisibility(string key, Slider slider)
    {
        if (PlayerPrefs.GetFloat(key) > 0) slider.enabled = true;
        else slider.enabled = false;

    }
}
