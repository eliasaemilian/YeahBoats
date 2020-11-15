using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundSetup : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonClickSound = null;

    // Start is called before the first frame update
    void Awake()
    {
        InitButtonSounds();
    }


    private void InitButtonSounds()
    {
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {

            // DEBUG RESET
            //UI_ButtonPlaySound[] butsc = buttons[i].GetComponents<UI_ButtonPlaySound>();
            //for (int j = 0; j < butsc.Length; j++)
            //{
            //    DestroyImmediate(butsc[j], true);
            //}

            if (buttons[i].gameObject.GetComponent<UI_ButtonPlaySound>() == null)
            {
                bool activeStatus = buttons[i].gameObject.activeSelf;
                if (!activeStatus) buttons[i].gameObject.SetActive(true);

                UI_ButtonPlaySound soundScript = buttons[i].gameObject.AddComponent<UI_ButtonPlaySound>();
                soundScript.ButtonSound = _buttonClickSound;
                soundScript.OnStartInit();
                buttons[i].onClick.AddListener(delegate { soundScript.OnButtonClick(); });

                if (!activeStatus) buttons[i].gameObject.SetActive(false);

            }

        }
    }

}
