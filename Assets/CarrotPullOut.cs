using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotPullOut : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _rotSpeed;
    public Vector3 _startPos;
    public Vector3 _bezierPos;
    public Vector3 _endPos;
    float _time;
    // Start is called before the first frame update
    void Start()
    {
        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_time >= _duration) {
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }
        Vector3 p1 = Vector3.Lerp(_startPos, _bezierPos, _time);
        Vector3 p2 = Vector3.Lerp(_bezierPos, _endPos, _time);
        // transform.position = Vector3.Lerp(p1, p2, _time);
        transform.position = Vector3.Lerp(p1, p2, _time);
        _time += Time.deltaTime / _duration;

        transform.Rotate(_rotSpeed * Vector3.forward);
    }
}
