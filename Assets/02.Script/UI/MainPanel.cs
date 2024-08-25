using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    Player _player;
    
    [SerializeField] private InputActionReference _optionAction;
    public GameObject pausePanel;

    [SerializeField] private GameObject _playerInfoPanel;
    [SerializeField] DistanceMeter _distanceMeter;
    [SerializeField] TextMeshProUGUI _distanceRecordText;
    [SerializeField] TextMeshProUGUI _carrotRecordText;
    [SerializeField] Button _optionBtn;
    [SerializeField] Button _pullBtn;
    [SerializeField] Button _jumpBtn;

    [Header("Only Start")]
    
    [SerializeField] Button _startButton;
    [SerializeField] GameObject _carrotPanel;
    [SerializeField] TextMeshProUGUI _carrotText;
    
    private void OnEnable() {
        _optionAction.action.performed += OnOptionActionTriggered;
    }

    private void OnDisable() {
        _optionAction.action.performed -= OnOptionActionTriggered;
    }

    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        //TODO: SetRecordText from save data
        SetRecordText(PlayerPrefs.GetInt("highDistance"), PlayerPrefs.GetInt("highCarrot"), PlayerPrefs.GetInt("carrot"));
    }

    private void Start() {
        _startButton.onClick.AddListener(SetGameUI);
        BuildStartUI();
    }
    
    private void FixedUpdate()
    {
        if (_player.isCinema)
        {
            return;
        }
        _distanceRecordText.text = (int)_player.distance + "m";
        _carrotRecordText.text = _player.currentCarrotNum.ToString();
        _carrotText.text = _player.totalCarrotNum.ToString();
    }
    
    public void SetRecordText(float distance, int carrot, int totalCarrot) {
        _distanceRecordText.text = (int)distance + "m";
        _carrotRecordText.text = carrot.ToString();
        _carrotText.text = totalCarrot.ToString();
    }

    public void BuildStartUI() {
        _startButton.gameObject.SetActive(true);
        _playerInfoPanel.SetActive(true);
        _carrotPanel.SetActive(true);

        _distanceMeter.gameObject.SetActive(false);
        _pullBtn.gameObject.SetActive(false);
        _jumpBtn.gameObject.SetActive(false);
    }
    public void SetGameUI() {
        SetRecordText(0, 0, 0);
        _startButton.gameObject.SetActive(false);
        _playerInfoPanel.SetActive(false);
        _carrotPanel.SetActive(false);

        _distanceMeter.gameObject.SetActive(true);
        _pullBtn.gameObject.SetActive(true);
        _jumpBtn.gameObject.SetActive(true);

    }

    public void OnClickPickupBtn() {
        _pullBtn.image.color = _pullBtn.colors.pressedColor;
        // Player코드의 _pullAction.action.triggered가 호출되어야 함
        _player._pickupAction.action.IsPressed();
    }
    public void OnClickJumpBtn() {
        _pullBtn.image.color = _pullBtn.colors.pressedColor;
        // Player코드의 _jumpAction.action.triggered가 호출되어야 함
        _player._jumpAction.action.IsPressed();
    }
    public void OnClickOptionBtn() {
        _pullBtn.image.color = _pullBtn.colors.pressedColor;
        // Pause Game
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    
    private void OnOptionActionTriggered(InputAction.CallbackContext context) {
        OnClickOptionBtn();
    }
}
