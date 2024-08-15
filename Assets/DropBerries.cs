using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBerries : MonoBehaviour
{
    public DropBerry[] berries;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Force()
    {
        foreach (var berry in berries)
        {
            berry.Force();
        }
        
    }
}
