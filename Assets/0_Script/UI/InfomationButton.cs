using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfomationButton : MonoBehaviour
{
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonEvent);

    }
    void ButtonEvent() {
       
            Target.SetActive(!Target.activeSelf);
        
    }

}
