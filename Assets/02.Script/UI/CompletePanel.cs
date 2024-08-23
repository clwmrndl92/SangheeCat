using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _distanceText;
    [SerializeField] TextMeshProUGUI _carrotText;
    [SerializeField] Button _retryBtn;
    [SerializeField] Button _quitBtn;
    [SerializeField] Animator _panelAnimator;

    void Start()
    {
        _distanceText.text = $": {9999}m";
        _carrotText.text = $": {999}/{999}";

        _retryBtn.onClick.AddListener(OnClickBtn);
        _quitBtn.onClick.AddListener(OnClickBtn);

        gameObject.SetActive(false);
    }

    public void BuildCompletePanel(int diatance, int carrot, int carrotMax) {
        _distanceText.text = $": {diatance}m";
        _carrotText.text = $": {carrot}/{carrotMax}";
    }
    
    private void OnClickBtn(){
        Debug.Log("Click");
    }
}
