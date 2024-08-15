using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    public Transform dropPosition;
    bool alreadyDrop = false;
    public GameObject dropBerryPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!alreadyDrop && Input.GetKeyDown(KeyCode.RightArrow)){
            StartCoroutine(DestoryObject(Instantiate(dropBerryPrefab, dropPosition.position, Quaternion.identity)));
        }
        
    }

    IEnumerator DestoryObject(GameObject obj){
        alreadyDrop = true;
        yield return new WaitForSeconds(1.5f);
        Destroy(obj);
        alreadyDrop = false;
    }
}
