using System.Collections;
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

    private float _counter, _lerpRadius, _outerFinalRadius;
    private float _lerpTime = .8f;

    private Shapes.Rectangle _rect;
    private UI_VectorElementScaler _scaler;
    private List<RectTransform> _uiElements = new List<RectTransform>();
    private TextMeshProUGUI _textfield, _promptText;
    private Rect _rectTrans;

    private bool _openWindow, _movePrompt;
    private int _tutIndexCount = 0;

    // Start is called before the first frame update
    void Start()
    {
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

        _tapAnywherePrompt.SetActive(false);
        List<RectTransform> _promptUI = _tapAnywherePrompt.GetComponent<UI_VectorElementScaler>().PullCanvasUIElements();
        for (int i = 0; i < _promptUI.Count; i++)
        {
            if (_promptUI[i].GetComponent<TextMeshProUGUI>()) _promptText = _promptUI[i].GetComponent<TextMeshProUGUI>();
        }
        _promptText.gameObject.SetActive(false);

        //DEBUG
        OnTutorialTriggered();
    }

    // Update is called once per frame
    void Update()
    {
        if (_openWindow)
        {
            StartCoroutine(OpenTutorialPopUp());

        }

    }

    private void OnTutorialTriggered()
    {

        // put at right position
        _scaler.SetPositionInScreenSpace(tutSet.Instructions[_tutIndexCount].ScreenPos.x, tutSet.Instructions[_tutIndexCount].ScreenPos.y);
        // plop up background
        _counter = 0f;
        _openWindow = true;


        // start arrow movement

    }



    IEnumerator OpenTutorialPopUp()
    {
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

    }


}
