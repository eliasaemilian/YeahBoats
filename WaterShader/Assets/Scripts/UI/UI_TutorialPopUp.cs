﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

[RequireComponent(typeof (Shapes.Rectangle))] [RequireComponent(typeof (UI_VectorElementScaler))]
public class UI_TutorialPopUp : MonoBehaviour
{
    [SerializeField] private TutorialSet tutSet = null;
    [SerializeField] [Range (0f, 1f)] private float screenX = .5f;
    [SerializeField] [Range (0f, 1f)] private float screenY = .5f;

    [SerializeField] private GameObject _tapAnywherePrompt = null;
    [SerializeField] private List<GameObject> _uiToDisable = null;

    private float _counter, _lerpRadius, _outerFinalRadius;
    private float _lerpTime = .8f;

    private Shapes.Rectangle _rect;
    private UI_VectorElementScaler _scaler;
    private List<RectTransform> _uiElements = new List<RectTransform>();
    private TextMeshProUGUI _textfield, _promptText;
    private Rect _rectTrans;

    private bool _openWindow, _closeWindow, _movePrompt;
    private int _tutIndexCount = 0;

    // Start is called before the first frame update
    void Start()
    {
      //  ResetTutorial();

        _rect = GetComponent<Shapes.Rectangle>();
        _outerFinalRadius = tutSet.Instructions[_tutIndexCount].PopUpSize.y;
        _rect.Height = 0f;
        _scaler = GetComponent<UI_VectorElementScaler>();
        _uiElements = _scaler.PullCanvasUIElements();

        for (int i = 0; i < _uiElements.Count; i++)
        {
            if (_uiElements[i].GetComponent<TextMeshProUGUI>()) _textfield = _uiElements[i].GetComponent<TextMeshProUGUI>();
        }

        _rectTrans = _textfield.GetComponent<RectTransform>().rect;
        _textfield.text = "";

        _tapAnywherePrompt.SetActive(false);
        List<RectTransform> _promptUI = _tapAnywherePrompt.GetComponent<UI_VectorElementScaler>().PullCanvasUIElements();
        for (int i = 0; i < _promptUI.Count; i++)
        {
            if (_promptUI[i].GetComponent<TextMeshProUGUI>()) _promptText = _promptUI[i].GetComponent<TextMeshProUGUI>();
        }
        _promptText.gameObject.SetActive(false);

        gameObject.SetActive(false);
        _textfield.gameObject.SetActive(false);


        // Check for PlayerPrefs
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            ResetTutorial(); // here bc Github gets freaky with the SO sometimes
            OnTutorialTriggered();
            if (FindObjectOfType<UI_JoystickHandler>() != null) UI_JoystickHandler.BlockJoystickMovement.Invoke();

            // Disable all other UI
            for (int i = 0; i < _uiToDisable.Count; i++)
            {
                _uiToDisable[i].SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_tutIndexCount >= tutSet.Instructions.Count)
        {

        }
        else if (tutSet.Instructions[_tutIndexCount].IsConfirmed)
        {
            // once confirmed move to next tutorial
            _tutIndexCount++;
        }

        if (_closeWindow) StartCoroutine(CloseTutorialPopUp());
        else if (_openWindow) StartCoroutine(OpenTutorialPopUp());

    }

    /// <summary>
    /// If Tutorial is active, listen for touches
    /// If touch lifted is registered move to next Tutorial
    /// </summary>
    float _bufferTimer;
    float _bufferTime = .8f;
    bool waitingForConfirm;
    Touch _touch;
    private void LateUpdate()
    {
        if (!waitingForConfirm) return;

        _bufferTimer += Time.deltaTime;
        if (_bufferTimer < _bufferTime) return;

        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Ended)
            {
                tutSet.Instructions[_tutIndexCount].IsConfirmed = true;
                waitingForConfirm = false;
                _counter = _lerpTime;
                _closeWindow = true;
                _bufferTimer = 0f;
            }
        }
    }

    private void OnTutorialTriggered()
    {

        // put at right position
        _scaler.SetPositionInScreenSpace(tutSet.Instructions[_tutIndexCount].ScreenPos.x, tutSet.Instructions[_tutIndexCount].ScreenPos.y);
        // plop up background
        _counter = 0f;
        _openWindow = true;


        gameObject.SetActive(true);
        _textfield.gameObject.SetActive(true);

    }



    IEnumerator OpenTutorialPopUp()
    {
        if (tutSet.Instructions[_tutIndexCount].Instruction == null) EndTutorial();

        _counter += Time.deltaTime;
        // Lerp outer Radius
        _lerpRadius = Mathf.Lerp(0, _outerFinalRadius, _counter / _lerpTime);
        _rect.Height = _lerpRadius;

        yield return new WaitUntil(() => _counter >= _lerpTime);
        _openWindow = false;

        // enable text
        for (int i = 0; i < _uiElements.Count; i++) _uiElements[i].gameObject.SetActive(true);
        _textfield.text = tutSet.Instructions[_tutIndexCount].Instruction;

        // set height of textfield
        float width = tutSet.Instructions[_tutIndexCount].PopUpSize.x * 17f;
        float height = tutSet.Instructions[_tutIndexCount].PopUpSize.y * 20f;
     //   _rectTrans.size = new Vector2(width, height);
        _rectTrans.height = height;
        _rectTrans.width = width;
        _textfield.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        // trigger Prompt
        _tapAnywherePrompt.SetActive(true);
        _promptText.gameObject.SetActive(true);

        waitingForConfirm = true;

    }

    IEnumerator CloseTutorialPopUp()
    {
        _counter -= Time.deltaTime;
        // Lerp outer Radius
        _lerpRadius = Mathf.Lerp(0, _outerFinalRadius, _counter / _lerpTime);
        _rect.Height = _lerpRadius;

        if (_counter < _lerpTime * .5f) _textfield.gameObject.SetActive(false);


        yield return new WaitUntil(() => _counter <= 0f);

        _textfield.text = "";
        _closeWindow = false;
        for (int i = 0; i < _uiElements.Count; i++) _uiElements[i].gameObject.SetActive(false);

        SetVisibility(false);
        ProceedTutorial();
    }


    private void EndTutorial()
    {
        UI_JoystickHandler.UnblockJoystickMovement.Invoke();
        PlayerPrefs.SetFloat("Tutorial", 1f);

        SetVisibility(false);
        enabled = false;

        // Enable all other UI
        for (int i = 0; i < _uiToDisable.Count; i++)
        {
            _uiToDisable[i].SetActive(true);
        }
    }

    private void ProceedTutorial()
    {
        if (_tutIndexCount < tutSet.Instructions.Count) OnTutorialTriggered();
        else EndTutorial();
    }

    private void SetVisibility(bool val)
    {
        _tapAnywherePrompt.SetActive(val);
        _promptText.gameObject.SetActive(val);
        gameObject.SetActive(val);
        _textfield.gameObject.SetActive(val);
    }

    private void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("Tutorial");
        for (int i = 0; i < tutSet.Instructions.Count; i++)
        {
            tutSet.Instructions[i].IsConfirmed = false;
        }
    }
}
