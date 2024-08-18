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
}
