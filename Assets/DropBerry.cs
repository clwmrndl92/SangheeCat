using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBerry : MonoBehaviour
{
    Rigidbody2D _rigid;
    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Force()
    {
        transform.localPosition = Vector3.zero;
        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(new Vector2( Random.Range(-50,0), Random.Range(0,100f)), ForceMode2D.Impulse);
        
    }
}
