using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{

    [SerializeField] private PlayableAsset _intro;
    [SerializeField] private PlayableAsset _epilogue;
    [SerializeField] private PlayableDirector _director;
    private static TimelineManager _instance;
    
    public Player player;

    public static TimelineManager Instance { 
        get {
            if(_instance == null) 
                _instance = new TimelineManager();
            return _instance;
        }
    }

    private void Awake() {
        if(_instance == null) 
            _instance = this;
        OnIntroEnd.AddListener(OnIntroEndFunction);
    }

    public static UnityEvent OnIntroEnd = new UnityEvent();

    public void IntroStart() {
        _director.Play(_intro);
        Debug.Log("Intro Start!");
    }
    public void IntroEnd() {
        OnIntroEnd.Invoke();
        Debug.Log("Intro End Invoke!");
    }
    public void EpilogueStart() {
        _director.Play(_epilogue);
        Debug.Log("Epilogue Start!");
    }

    private void OnIntroEndFunction()
    {
        var ground = GameObject.Find("Ground_0").GetComponent<Ground>();
        ground.screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x;
        ground = GameObject.Find("Ground_1").GetComponent<Ground>();
        ground.screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x;
        ground = GameObject.Find("Ground_2").GetComponent<Ground>();
        ground.screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x;
        ground = GameObject.Find("Ground_3").GetComponent<Ground>();
        ground.screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x;
        
        player.isCinema = false;
    }
}