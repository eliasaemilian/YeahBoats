using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider = null;
    [SerializeField] private Image _soundSliderHandle = null;
    [SerializeField] private Slider _musicSlider = null;
    [SerializeField] private Image _musicSliderHandle = null;

    [SerializeField] private TextMeshProUGUI _soundButtonText = null;
    [SerializeField] private TextMeshProUGUI _soundVolumeText = null;
    [SerializeField] private TextMeshProUGUI _musicButtonText = null;
    [SerializeField] private TextMeshProUGUI _musicVolumeText = null;
    [SerializeField] private TextMeshProUGUI _notifButtonText = null;
    [SerializeField] private TextMeshProUGUI _reverseInputButtonText = null;

    [SerializeField] private Color _disabledElementColor = Color.grey;

    [SerializeField] private List<GameObject> _uiToDisableOnOpen = null;

    public static readonly string ReverseInput = "ReverseInput";
    public static readonly string Sound = "Sound";
    public static readonly string SoundVol = "SoundVolume";
    public static readonly string Music = "Music";
    public static readonly string MusicVol = "MusicVolume";
    public static readonly string Notif = "Notifications";

    private SoundscapeManager _soundscapeManager;

    private Color _handleDefaultCol;


    private string _musicOn = "Music is On";
    private string _musicOff = "Music is Off";
    private string _soundOn = "Sounds are On";
    private string _soundOff = "Sounds are Off";
    private string _notifOn = "Notifications are On";
    private string _notifOff = "Notifications are Off";
    private string _reverseOn = "Reverse Boat Input On";
    private string _reverseOff = "Reverse Boat Input Off";

    // Start is called before the first frame update
    void OnEnable()
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
      //  PlayerPrefs.DeleteAll();
#endif

        if (!PlayerPrefs.HasKey(Notif))
        {
            PlayerPrefs.SetFloat(Notif, 0);
            PlayerPrefs.SetFloat(Music, 0);
            PlayerPrefs.SetFloat(MusicVol, 1);
            PlayerPrefs.SetFloat(Sound, 0);
            PlayerPrefs.SetFloat(SoundVol, 1);
            PlayerPrefs.SetFloat(ReverseInput, 0);
        }

        UpdateAllUIFromPlayerPrefValues();

    }

    private void UpdateAllUIFromPlayerPrefValues()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(MusicVol);
        _soundSlider.value = PlayerPrefs.GetFloat(SoundVol);
        SetBoolToPrefValue(Music);
        SetBoolToPrefValue(Sound);
        SetBoolToPrefValue(Notif);
        SetBoolToPrefValue(ReverseInput);

        UpdateSliderVisibility(Music, _musicSlider, _musicSliderHandle);
        UpdateSliderVisibility(Sound, _soundSlider, _soundSliderHandle);

        UpdateVolumeText(_musicSlider, _musicVolumeText);
        UpdateVolumeText(_soundSlider, _soundVolumeText);

    }

    public void OnChangedVolumeSliderSound()
    {
        ChangePreference(SoundVol, _soundSlider.value);
        UpdateVolumeText(_soundSlider, _soundVolumeText);
    }

    public void OnChangedVolumeSliderMusic()
    {
        ChangePreference(MusicVol, _musicSlider.value);
        if (_soundscapeManager != null) _soundscapeManager.VolumeChanged();
        UpdateVolumeText(_musicSlider, _musicVolumeText);
    }

    public void OnClickMusicOnOff()
    {
        if (ChangeBoolPref(Music))
        {
            _musicButtonText.text = _musicOn;
            if (_soundscapeManager != null) _soundscapeManager.ResumeMusic();
        }
        else 
        {
            _musicButtonText.text = _musicOff;
            if (_soundscapeManager != null) _soundscapeManager.KillAllMusic();
        }

        UpdateSliderVisibility(Music, _musicSlider, _musicSliderHandle);
        UpdateVolumeText(_musicSlider, _musicVolumeText);
    }

    public void OnClickSoundOnOff()
    {
        if (ChangeBoolPref(Sound)) _soundButtonText.text = _soundOn;
        else _soundButtonText.text = _soundOff;

        UpdateSliderVisibility(Sound, _soundSlider, _soundSliderHandle);
        UpdateVolumeText(_soundSlider, _soundVolumeText);
    }

    public void OnClickNotificationsOnOff()
    {
        if (ChangeBoolPref(Notif)) _notifButtonText.text = _notifOn;
        else _notifButtonText.text = _notifOff;

        ChangeBoolPref(Notif);
    }

    public void OnClickReverseFishingInput()
    {
        if (ChangeBoolPref(Sound)) _reverseInputButtonText.text = _reverseOn;
        else _reverseInputButtonText.text = _reverseOff;

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

    private void UpdateVolumeText(Slider slider, TextMeshProUGUI text)
    {
        text.text = $"Set Volume: {Mathf.RoundToInt(slider.value * 100f)} %";

    }

    public void OnClickOpenCloseSettings()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            DisableEnableObstrcutingUI(true);
        }
        else
        {
            gameObject.SetActive(true);
            DisableEnableObstrcutingUI(false);
            UpdateAllUIFromPlayerPrefValues();
        }
    }

    private void DisableEnableObstrcutingUI(bool status)
    {
        if (_uiToDisableOnOpen.Count > 0)
        {
            for (int i = 0; i < _uiToDisableOnOpen.Count; i++)
            {
                _uiToDisableOnOpen[i].SetActive(status);
            }
        }
    }

    private void SetBoolToPrefValue(string key)
    {
        if (key == Music)
        {
            if (ReadBoolPref(key)) _musicButtonText.text = _musicOn;
            else _musicButtonText.text = _musicOff;
        }
        else if (key == Sound)
        {
            if (ReadBoolPref(key)) _soundButtonText.text = _soundOn;
            else _soundButtonText.text = _soundOff;
        }
        else if (key == Notif)
        {
            if (ReadBoolPref(key)) _notifButtonText.text = _notifOn;
            else _notifButtonText.text = _notifOff;
        }
        else if (key == ReverseInput)
        {
            if (ReadBoolPref(key)) _reverseInputButtonText.text = _reverseOn;
            else _reverseInputButtonText.text = _reverseOff;
        }
    }

    /// <summary>
    /// Save Current Scene Index
    /// </summary>
    public void OnApplicationQuit() => SaveCurrentScene();
    public void OnApplicationPause(bool pause)
    {
        if (pause) SaveCurrentScene();
    }

    private void SaveCurrentScene() => PlayerPrefs.SetInt("SceneIndex", SceneManager.GetActiveScene().buildIndex);
}
