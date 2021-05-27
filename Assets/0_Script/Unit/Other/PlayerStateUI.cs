using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : UnitStateUI
{
    string temp = "을(를) 획득하였습니다";
    public Transform Textfield;

    public void ShowText(string s) {
        var target = Textfield.GetChild(0);
        target.GetComponent<Text>().text = s +temp;
        target.SetAsLastSibling();
    }
}
