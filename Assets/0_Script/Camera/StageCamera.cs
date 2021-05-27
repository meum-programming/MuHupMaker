using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCamera : MonoBehaviour
{
    public Camera SubCamera;
    public Vector3 offset = new Vector3(-4.0f, 4.0f, -4.0f);

    public GameObject player;
    public bool Smooth = false;
    public float SmoothTime = 0.1f;
    private void Start()
    {
        A_Script.GameManager.i.ActiveStageCamera = this;



        player = A_Script.GameManager.i.player.gameObject;
        A_Script.GameManager.i.InitScene();
        Smooth = A_Script.GameManager.i.Smooth;
        SmoothTime = A_Script.GameManager.i.SmoothTime;
        shake = A_Script.GameManager.i.shake;
        shakeAmount = A_Script.GameManager.i.shakeAmount;
        decreaseFactor = A_Script.GameManager.i.decreaseFactor;
    }
        Vector3 velocity = Vector3.zero;
    bool IsShack = false;
    public float shake = 0.3f;
    float localshake = 0f;
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    // Update is called once per frame
    public void Shack() {
        localshake = shake;
        IsShack = true;
        originalPos = this.transform.localPosition;
    }
    void Update()
    {
        if (!IsShack)
        {
            if (!Smooth)
            {
                transform.position = player.transform.position + offset;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, SmoothTime);
            }
        }
        else {
            if (localshake > 0)
            {
                this.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                localshake -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                localshake = 0f;
                this.transform.localPosition = originalPos;
                IsShack = false;
            }


     
        }
    }
}
