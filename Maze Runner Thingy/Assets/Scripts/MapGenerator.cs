using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Vector2 mapSize;
	[Range(0,1)]
	public float outlinePercent;
	[Range(0,1)]
	public float obstaclePercent;

	public int seed = 10;
	Coord mapCenter;
    Coord nextLevelMap;
	public Vector3 playerSpawn;
    public Vector3 nextLevelSpawn;
	List<Coord> allTileCords;
	List<Coord> cornerTileCords;
	Queue<Coord> shuffledTileCoords;

	void Start()
	{
		GenerateMap ();
	}

	public void GenerateMap ()
	{
		allTileCords = new List<Coord> ();
		cornerTileCords = new List<Coord> ();
		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				allTileCords.Add (new Coord (x, y));
			}
		}
		cornerTileCords.Add (new Coord (1, 1));
		cornerTileCords.Add (new Coord (mapSize.x-2, 1));
		cornerTileCords.Add (new Coord (1, mapSize.y-2));
		cornerTileCords.Add (new Coord (mapSize.x-2, mapSize.y-2));

		var goalCorner = Random.Range (0, 3);
		shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCords.ToArray(), seed));
		mapCenter = new Coord ((int)(mapSize.x / 2), (int)(mapSize.y / 2));
		playerSpawn = CoordtoPos (mapCenter.x, mapCenter.y);
		nextLevelMap = cornerTileCords [goalCorner];
		nextLevelSpawn = CoordtoPos(nextLevelMap.x, nextLevelMap.y);
		string holderName = "Generated Map";
		if (transform.FindChild (holderName)) {
			DestroyImmediate (transform.FindChild (holderName).gameObject);
		}

		Transform mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform;

		for(int x = 0; x < mapSize.x; x++)
		{
			for(int y = 0; y < mapSize.y; y++)
			{
				Vector3 tilePosition = CoordtoPos (x, y);
				Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90)) as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent);
				newTile.parent = mapHolder;
			}
		}

		bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

		int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
		int currentObstacleCount = 0;

		for (int x = 0; x < mapSize.x; x++) 
		{
			for (int y = 0; y < mapSize.y; y++) 
			{
				if ((x == 0 || y == 0 || x == mapSize.x -1 || y == mapSize.y -1))
				{
					obstacleMap [x, y] = true;
					currentObstacleCount++;
					Vector3 obstaclePos = CoordtoPos (x, y);
					Transform newObstacle = Instantiate (obstaclePrefab, obstaclePos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
					newObstacle.parent = mapHolder;
					newObstacle.transform.localScale = new Vector3 (1f, 4f, 1f);
				}
			}
		}

		for (int i = 0; i < obstacleCount; i++) {
			Coord randomCoord = GetRandomCoord ();

			obstacleMap [randomCoord.x, randomCoord.y] = true;
			currentObstacleCount++;
			if ((randomCoord.x == 0 || randomCoord.y == 0 || randomCoord.x == mapSize.x -1 || randomCoord.y == mapSize.y -1))
			{
				currentObstacleCount--;
			}
			else if (randomCoord.x > 0 && randomCoord.x < mapSize.x-1 && randomCoord.y > 0 && randomCoord.y < mapSize.y-1 && randomCoord != mapCenter && MapIsFullyAccessible (obstacleMap, currentObstacleCount)) {
				Vector3 obstaclePos = CoordtoPos (randomCoord.x, randomCoord.y);
				Transform newObstacle = Instantiate (obstaclePrefab, obstaclePos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
				newObstacle.parent = mapHolder;
				newObstacle.transform.localScale = new Vector3 (1f, Random.Range (1f, 3f), 1f);
			}
			else
			{
				obstacleMap [randomCoord.x, randomCoord.y] = false;
				currentObstacleCount--;
			}
		}
	}

	bool MapIsFullyAccessible (bool[,] obstacleMap, int currentObstacleCount)
	{
		bool[,] mapFlags = new bool[obstacleMap.GetLength (0), obstacleMap.GetLength (1)];
		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (mapCenter);
        queue.Enqueue(nextLevelMap);
		mapFlags [mapCenter.x, mapCenter.y] = true;
        mapFlags[nextLevelMap.x, nextLevelMap.y] = true;
		int accessibleTileCount = 1;

		while (queue.Count > 0) 
		{
			Coord tile = queue.Dequeue ();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++) 
				{
					int neighborX = tile.x + x;
					int neighborY = tile.y + y;
					if (x == 0 || y == 0) 
					{
						if (neighborX >= 0 && neighborX < obstacleMap.GetLength (0) && neighborY >= 0 && neighborY < obstacleMap.GetLength (1)) 
						{
							if (!mapFlags [neighborX, neighborY] && !obstacleMap[neighborX, neighborY]) 
							{
								mapFlags [neighborX, neighborY] = true;
								queue.Enqueue (new Coord(neighborX, neighborY));
								accessibleTileCount++;
							}
						}
					}
				}
			}
		}

		int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
		return targetAccessibleTileCount == accessibleTileCount;
	}

	Vector3 CoordtoPos (int x, int y)
	{
		return new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
	}

	public Coord GetRandomCoord ()
	{
		Coord randomCoord = shuffledTileCoords.Dequeue ();
		shuffledTileCoords.Enqueue (randomCoord);
		return randomCoord;
	}

	public struct Coord
	{
		public int x;
		public int y;

		public Coord(int _x, int _y)
		{
			x = _x;
			y = _y;
		}

		public static bool operator ==(Coord c1, Coord c2)
		{
			return c1.x == c2.x && c1.y == c2.y;
		}

		public static bool operator !=(Coord c1, Coord c2)
		{
			return !(c1 == c2);
		}
	}
}
