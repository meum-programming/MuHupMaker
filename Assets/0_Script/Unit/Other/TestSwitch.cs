using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitch : MonoBehaviour
{
    public List<GameObject> list1;
    public List<GameObject> list2;
    public List<GameObject> list3;
    public Light l;
    public void list1s()
    {
        var b = list1[0].activeSelf;
        foreach (var l in list1)
        {
            l.SetActive(!b);
        }
    }
    public void list2s()
    {
        var b = list2[0].activeSelf;
        foreach (var l in list2)
        {
            l.SetActive(!b);
        }
    }
    public void list3s()
    {
        var b = list3[0].activeSelf;
        foreach (var l in list3)
        {
            l.SetActive(!b);
        }
    }
    public void shdow() {
        if (l.shadows == LightShadows.Soft)
        {
            l.shadows = LightShadows.None;
        }
        else {
            l.shadows = LightShadows.Soft;
        }
    }
}
