using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace A_Script
{
    public class WaypointSender : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            Debug.LogError("Waypoints");
            GameManager.i.Waypoints = GetComponentsInChildren<Transform>().ToList();
            GameManager.i.Waypoints.RemoveAt(0);
        }

 
    }

}