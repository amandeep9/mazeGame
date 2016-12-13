using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float time = 0;
	public bool running=true;

	// Use this for initialization
	void Start () {
		running = !GameObject.Find ("UI").GetComponent<Pause> ().isPaused;
	}
	
	// Update is called once per frame
	void Update () {
		if (running == true)
			time += Time.deltaTime;
		print ("time: "+time);
	}
}
