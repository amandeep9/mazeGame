using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour {
	GameObject flashlight;

	void Start ()
	{
		flashlight = transform.GetChild (0).gameObject;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) 
		{
			if (flashlight.activeSelf)
				flashlight.SetActive (false);
			else
				flashlight.SetActive (true);
		}
	
	}
}
