using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterController : MonoBehaviour
{
    public float speed = 1;
    private SpriteRenderer _sprite;
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
            transform.position += speed * Time.deltaTime * Vector3.right;
            _sprite.flipX = true;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            transform.position += speed * Time.deltaTime * Vector3.left;
            _sprite.flipX = false;
        }
    }
}
