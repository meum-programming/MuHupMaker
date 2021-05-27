using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWarp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        A_Script.GameManager.i.player.UnitWarp(this.transform.position);
    }

  
}
