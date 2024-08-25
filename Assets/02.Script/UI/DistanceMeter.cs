using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMeter : MonoBehaviour
{
    [SerializeField] private GameObject _point;
    [SerializeField] private GameObject _record;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    
    [Tooltip("0~1")] public float position = 0;
    
    private Player _player;
    public float Goal = 1000;

    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        //TODO: SetRecord from save data
        SetRecord(PlayerPrefs.GetInt("highDistance") / 1000f, true);
    }
    
    /// <summary>
    /// position : 0 ~ 1
    /// </summary>
    /// <param name="position"></param>
    public void SetPoint(float position){
        _point.transform.position = Vector3.Lerp(_start.position, _end.position, position);
    }
    /// <summary>
    /// position : 0 ~ 1
    /// </summary>
    /// <param name="position"></param>
    public void SetRecord(float position, bool setActive = true){
        if (position <= 0.001f)
        {
            return;
        }
        _record.transform.position = Vector3.Lerp(_start.position, _end.position, position);
        _record.SetActive(setActive);
    }
    
    private void FixedUpdate()
    {
        position = _player.distance / Goal;
        SetPoint(position);
    }
}
