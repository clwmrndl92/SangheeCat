using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBerries : MonoBehaviour
{
    public DropBerry[] berries;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var berry in berries)
        {
            berry.transform.localScale = Random.Range(0.7f, 1) * Vector3.one;
        }
        StartCoroutine("DropBerryRoutine");
    }

    public void Drop()
    {
        foreach (var berry in berries)
        {
            berry.Force();
        }
        
    }
    public void DropCarrot()
    {
        foreach (var berry in berries)
        {
            berry.Force(-20, 20, 0, 20);
        }
        
    }

    IEnumerator DropBerryRoutine() {
        while(true) {
            DropCarrot();
            yield return new WaitForSeconds(2);
        }
    }
}
