using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] Button _retryBtn;
    [SerializeField] Button _quitBtn;
    [SerializeField] Button _continueBtn;

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
    }
    
    public void Quit()
    {
        // Quit Game (Editor also)
        Application.Quit();
    }
    
    public void Continue()
    {
        // Continue Game
        Time.timeScale = 1;
    }

}
