using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private bool isLoading = false;
    
    //TODO: Connect IntroScene to GameScene
    private TimelineManager timelineManager;

    private void Awake()
    {
        timelineManager = FindObjectOfType<TimelineManager>();
    }

    private void Start()
    {
        /*SoundManager.Instance.PlayMusic("bgm_title2");*/
    }
        
    public void QuitGameClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
                Application.OpenURL(webplayerQuitURL);
        #else
                Application.Quit();
        #endif
    }
}