using UnityEngine;
using System.Collections;

public class footSteps : MonoBehaviour {
    public AudioClip footStp1;
    public AudioClip footStp2;
    public AudioSource audioSopurce;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void footStep1()
    {
        audioSopurce.PlayOneShot(footStp1);
	}

	public void footStep2()
	{
        audioSopurce.PlayOneShot(footStp2);
	}
}

