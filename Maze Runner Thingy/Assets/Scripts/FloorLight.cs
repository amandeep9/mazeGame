﻿using UnityEngine;
using System.Collections;

public class FloorLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "Player") 
		{
			transform.GetChild (0).gameObject.SetActive (true);
		}
	}
}
