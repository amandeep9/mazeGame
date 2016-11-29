using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {

	public float distance;

	private float maxDistance = 25f;

	// Use this for initialization
	void Start () {
	
	}

	void Update ()
	{
		distance = transform.parent.gameObject.GetComponent<Raycast3>().distance3;

	}

	void LateUpdate () 
	{
		if(distance > maxDistance)
		{
			distance = maxDistance;
		}

		transform.position = new Vector3 (0, 0, distance);
	}
}
