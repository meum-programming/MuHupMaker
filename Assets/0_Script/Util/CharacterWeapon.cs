using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    public Transform TranL;
    public Transform TranR;

    // Start is called before the first frame update


    public void SetPrefab(int type)
    {
        //   Debug.Log(TranL == null || TranR == null);
        if (TranL == null || TranR == null)
            return;
        foreach (Transform t in TranL)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in TranR)
        {
            Destroy(t.gameObject);
        }
        GameObject temp;
        switch (type)
        {
            case 1:
                temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Kunckle + "Knuckle_01_01"));
                temp.transform.SetParent(TranL);
                temp.transform.localPosition = new Vector3(-0.046f, -0.011f, 0.057f);
                temp.transform.localEulerAngles = new Vector3(0, -113.82f, -90);
                temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Kunckle + "Knuckle_01_01"));
                temp.transform.SetParent(TranR);
                temp.transform.localPosition = new Vector3(-0.046f, -0.011f, 0.057f);
                temp.transform.localEulerAngles = new Vector3(0, -113.82f, -90);
                temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 2:

                temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Sword + "Sword_01_01"));
                temp.transform.SetParent(TranR);
                temp.transform.localPosition = new Vector3(-0.076f, -0.01f, 0.008f);
                temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 3:
                temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Katana + "Katana_01_01"));
                temp.transform.SetParent(TranR);
                temp.transform.localPosition = new Vector3(-0.065f, -0.006f, 0.015f);
                temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 4:
                temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Lance + "Lance_01_01"));
                temp.transform.SetParent(TranR);
                temp.transform.localPosition = new Vector3(-0.069f, -0.015f, -0.236f);
                temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case 5:
                temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Mace + "Mace_01_01"));
                temp.transform.SetParent(TranR);
                temp.transform.localPosition = new Vector3(-0.092f, -0.015f, -0.035f);
                temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
        }

    }
}
