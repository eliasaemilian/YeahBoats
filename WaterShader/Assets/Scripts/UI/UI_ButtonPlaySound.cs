using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonPlaySound : MonoBehaviour
{
    private SoundscapeManager _soundscapeManager;

    public AudioClip ButtonSound;

    // Start is called before the first frame update
    void Start() => OnStartInit();


    public void OnButtonClick()
    {
        // _audio.Play();
      //  Debug.Log("Click");
        if (_soundscapeManager != null) SoundscapeManager.PlaySoundWithClip.Invoke(ButtonSound);
    }

    public void OnStartInit()
    {
        _soundscapeManager = FindObjectOfType<SoundscapeManager>();
     //   Debug.Log("Setup Button");
    }
}
