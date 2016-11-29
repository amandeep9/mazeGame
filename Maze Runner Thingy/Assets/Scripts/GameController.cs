using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Transform playerPrefab;
    public Transform nextLevel;
	// Use this for initialization
	void Start () 
	{
		int rand = (int)Random.Range (0f, 1000000f);
		print (rand);
		GameObject.Find("Map").GetComponent<MapGenerator>().seed = rand;
		GameObject.Find ("Map").GetComponent<MapGenerator> ().obstaclePercent = Random.Range (0.9f, 1f);
		GameObject.Find("Map").GetComponent<MapGenerator>().GenerateMap();
		Transform Player = Instantiate (playerPrefab, GameObject.Find ("Map").GetComponent<MapGenerator> ().playerSpawn + Vector3.up *0.5f, Quaternion.identity) as Transform;
		Transform next = Instantiate(nextLevel, GameObject.Find("Map").GetComponent<MapGenerator>().nextLevelSpawn + Vector3.up * 0.5f, Quaternion.identity) as Transform;
		Player.name = "Player";
        next.name = "nextLevel";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
