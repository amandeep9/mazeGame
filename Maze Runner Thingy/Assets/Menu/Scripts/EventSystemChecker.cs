using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour
{

	public Text highScoreText;
	public Text lastScoreText;

	void Start(){
		highScoreText.text = "HighScore: " + (PlayerPrefs.GetFloat ("Highscore").ToString("f1"));
		lastScoreText.text = "Last Score: " + (PlayerPrefs.GetFloat ("Lastscore").ToString("f1"));
	}

	//OnLevelWasLoaded is called after a new scene has finished loading
	void OnLevelWasLoaded ()
	{
		//If there is no EventSystem (needed for UI interactivity) present
		if(!FindObjectOfType<EventSystem>())
		{
			//The following code instantiates a new object called EventSystem
			GameObject obj = new GameObject("EventSystem");

			//And adds the required components
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
		}
	}
}
