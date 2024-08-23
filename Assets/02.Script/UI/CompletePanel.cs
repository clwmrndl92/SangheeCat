using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletePanel : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _distanceText;
    [SerializeField] public TextMeshProUGUI _carrotText;
    [SerializeField] Button _retryBtn;
    [SerializeField] Button _quitBtn;
    [SerializeField] Animator _panelAnimator;
    
    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        _distanceText.text = (int)_player.distance + "m";
        _carrotText.text = "0";

        _retryBtn.onClick.AddListener(OnClickRetryBtn);
        _quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    public void BuildCompletePanel(int diatance, int carrot, int carrotMax) {
        _distanceText.text = $": {diatance}m";
        _carrotText.text = $": {carrot}";
    }
    
    private void OnClickRetryBtn(){
        // Reload Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    private void OnClickQuitBtn(){
        // Quit Game (Editor also)
        Application.Quit();
    }
}
