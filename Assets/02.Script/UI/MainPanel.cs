using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
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
    
    private void Start() {
        _startButton.onClick.AddListener(SetGameUI);
        BuildStartUI();
    }

    public void BuildStartUI() {
        _startButton.gameObject.SetActive(true);
        _carrotPanel.SetActive(true);

        _distanceMeter.gameObject.SetActive(false);
        _pullBtn.gameObject.SetActive(false);
        _jumpBtn.gameObject.SetActive(false);
    }
    public void SetGameUI() {
        _startButton.gameObject.SetActive(false);
        _carrotPanel.SetActive(false);

        _distanceMeter.gameObject.SetActive(true);
        _pullBtn.gameObject.SetActive(true);
        _jumpBtn.gameObject.SetActive(true);

    }

    public void OnClickPullBtn() {
        _pullBtn.image.color = _pullBtn.colors.pressedColor;
    }
    public void OnClickJumpBtn() {
        _pullBtn.image.color = _pullBtn.colors.pressedColor;
    }
}
