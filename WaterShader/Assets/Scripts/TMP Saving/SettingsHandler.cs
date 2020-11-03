using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider = null;
    [SerializeField] private Image _soundSliderHandle = null;
    [SerializeField] private Slider _musicSlider = null;
    [SerializeField] private Image _musicSliderHandle = null;

    [SerializeField] private TextMeshProUGUI _soundButtonText = null;
    [SerializeField] private TextMeshProUGUI _musicButtonText = null;
    [SerializeField] private TextMeshProUGUI _notifButtonText = null;
    [SerializeField] private TextMeshProUGUI _reverseInputButtonText = null;

    [SerializeField] private Color _disabledElementColor = Color.grey;

    public static readonly string ReverseInput = "ReverseInput";
    public static readonly string Sound = "Sound";
    public static readonly string SoundVol = "SoundVolume";
    public static readonly string Music = "Music";
    public static readonly string MusicVol = "MusicVolume";
    public static readonly string Notif = "Notifications";

    private SoundscapeManager _soundscapeManager;

    private Color _handleDefaultCol;

    // Start is called before the first frame update
    void Start()
    {
        _soundSlider.onValueChanged.AddListener(delegate { OnChangedVolumeSliderSound(); });
        _musicSlider.onValueChanged.AddListener(delegate { OnChangedVolumeSliderMusic(); });

        _handleDefaultCol = _musicSliderHandle.color;

        InitPreferences();

        _soundscapeManager = FindObjectOfType<SoundscapeManager>();

    }

    /// <summary>
    /// Initialize Default Values for PlayerPrefs on first StartUp
    /// </summary>
    private void InitPreferences()
    {

#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
#endif

        if (PlayerPrefs.HasKey(Notif)) return;

        PlayerPrefs.SetFloat(Notif, 0);
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
        _soundscapeManager.VolumeChanged();
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

        UpdateSliderVisibility(Music, _musicSlider, _musicSliderHandle);

    }

    public void OnClickSoundOnOff()
    {
        if (ChangeBoolPref(Sound)) _soundButtonText.text = "Sounds are On";
        else _soundButtonText.text = "Sounds are Off";

        UpdateSliderVisibility(Sound, _soundSlider, _soundSliderHandle);
    }

    public void OnClickNotificationsOnOff()
    {
        if (ChangeBoolPref(Notif)) _notifButtonText.text = "Notifications are On";
        else _notifButtonText.text = "Notifications are Off";

        ChangeBoolPref(Notif);
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

    private void UpdateSliderVisibility(string key, Slider slider, Image handle)
    {
        Image[] sliderComps = slider.GetComponentsInChildren<Image>();
        if (PlayerPrefs.GetFloat(key) > 0)
        {
            slider.enabled = false;
            for (int i = 0; i < sliderComps.Length; i++)
            {
                sliderComps[i].color = _disabledElementColor;
            }
        }
        else
        {
            slider.enabled = true;
            for (int i = 0; i < sliderComps.Length; i++)
            {
                sliderComps[i].color = Color.white;
            }

            handle.color = _handleDefaultCol;
        }

    }

    public void OnClickOpenCloseSettings()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(true);
    }
}
