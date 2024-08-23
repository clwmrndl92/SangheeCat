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
        _retryBtn.onClick.AddListener(()=>Debug.Log("retry"));
        _quitBtn.onClick.AddListener(()=>Debug.Log("quit"));
        _continueBtn.onClick.AddListener(()=>Debug.Log("continue"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
