using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] Button _retryBtn;
    [SerializeField] Button _quitBtn;
    [SerializeField] Button _continueBtn;
    
    [SerializeField] private InputActionReference _optionAction;

    private void OnEnable()
    {
        _optionAction.action.performed += OnOptionActionTriggered;
    }
    
    private void OnDisable() {
        _optionAction.action.performed -= OnOptionActionTriggered;
    }

    // Start is called before the first frame update
    void Start()
    {
        _retryBtn.onClick.AddListener(Retry);
        _quitBtn.onClick.AddListener(Quit);
        _continueBtn.onClick.AddListener(Continue);
    }

    public void Retry()
    {
        // Reload Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
        #else
            Application.Quit();
        #endif
    }
    
    public void Continue()
    {
        // Continue Game
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    private void OnOptionActionTriggered(InputAction.CallbackContext context) {
        Continue();
    }
}
