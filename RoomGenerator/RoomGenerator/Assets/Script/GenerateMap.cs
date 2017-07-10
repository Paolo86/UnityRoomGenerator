using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

	const int FLOOR = 0;
	const int OUTER_WALL = 1;
	const int ROOM_WALL = 2;



	public int mapWidth = 50;
	public int mapHeight = 50;
	public int numberOfRooms = 3;

	public bool allowOverlapping;
	public int maxAttempts = 20;


	public GameObject wall;
	public GameObject floor;



	public int maxRoomsWidth;
	public int maxRoomsHeight;
	public int minRoomsWidth;
	public int minRoomsHeight;
	
	private Transform tiles;



	int[,] map;
	List<Room> allRooms = new List<Room>();



	void Update()
	{
		
		if (maxRoomsWidth < minRoomsWidth)
			maxRoomsWidth = minRoomsWidth;

		if (maxRoomsHeight < minRoomsHeight)
			maxRoomsHeight = minRoomsHeight;

	}

	public void Generate()
	{
		allRooms.Clear ();
		map = new int[mapWidth,mapHeight];

		if (tiles != null)
			DestroyImmediate (tiles.gameObject);
		tiles = new GameObject ("Tiles").transform;

		FillWithWall ();
		PlaceRooms (numberOfRooms);

		GenerateCorridors ();

		GenerateTiles ();



	}

	private void GenerateTiles()
	{

		for (int x=0; x<mapWidth; x++) {
			for (int y=0; y<mapHeight; y++) {

				GameObject o = null;
				if(map[x,y] == OUTER_WALL)
					o = Instantiate(wall,new Vector3(x,y,0f),Quaternion.identity) as GameObject;

				if(map[x,y] == FLOOR)
					o = Instantiate(floor,new Vector3(x,y,0f),Quaternion.identity) as GameObject;

				o.transform.SetParent(tiles);
			}
		}


	}

	private void GenerateCorridors()
	{

		for (int i=0; i<allRooms.Count; i++) {
	
			int index = 0;

			while(i==index)
			{
			index = Random.Range(0,allRooms.Count);
			}

			if(!allRooms[i].isConnected)
			{
			CreateCorridor(allRooms[i],allRooms[index]);
				allRooms[i].isConnected = true;
			
			}

		
		}


	}

	private void CreateCorridor(Room first, Room second)
	{

		int xDir = first.centerX>second.centerX? -1 : 1;
		int yDir = first.centerY>second.centerY? -1 : 1;

		int x = first.centerX;

		int y = first.centerY;

		while(x!=second.centerX + xDir)
		{
			map [x, first.centerY] = FLOOR;
			x+=xDir;

		}

		while(y!=second.centerY + yDir)
		{
			map [x, y] = FLOOR;
			y+=yDir;
			
		}

		/*for (int x=first.centerX; x!=second.centerX + xDir; x+=xDir) {

			map [x, first.centerY] = FLOOR;

			for (int y=first.centerY; y!=second.centerY + yDir; y+=yDir)
				map [x, y] = FLOOR;
		}*/


	}

	private void FillWithWall()
	{
		for (int x=0; x<mapWidth; x++) {
			for (int y=0; y<mapHeight; y++) {

				map[x,y] = OUTER_WALL;
			}
		}

	}

	private void PlaceRooms(int number)
	{

		for (int i=0; i<number; i++) {

			GenerateRoom();

		}
	}

	private void GenerateRoom()
	{
		bool roomAprroved = false;

		int attempts = 0;


		while (!roomAprroved) {

			int randX = Random.Range (1, mapWidth - 1);
			int randY = Random.Range (1, mapHeight);
			
			int randWidth = Random.Range (minRoomsWidth,maxRoomsWidth);
			int randHeight = Random.Range (minRoomsHeight,maxRoomsHeight);

			Room r = new Room(randX, randY, randWidth,randHeight);
			roomAprroved = CheckIfRoomFits(r);

			if(roomAprroved)
			{
				PlaceRoom (r);
				allRooms.Add(r);
			}
			else
				attempts++;

			if(attempts==maxAttempts)
							break;
		}



	}

	private void PlaceRoom(Room room)
	{
		for (int x=room.orgX; x<room.orgX + room.width; x++) {
			for (int y=room.orgY; y<room.orgY + room.height; y++) {
				
				map[x,y] = FLOOR;
				
			}
			
		}

	}

	private bool CheckIfRoomFits(Room room)
	{

		int wallCount = 0;
		for (int x=room.orgX; x<room.orgX + room.width; x++) {
			for (int y=room.orgY; y<room.orgY + room.height; y++) {

				if(x - room.width <=0 || x + room.width >=mapWidth || y - room.height <=0 || y + room.height>=mapHeight)
					return false;

			
					if(map[x,y]!=OUTER_WALL)
						wallCount++;



				if(allowOverlapping)
					return true;

				

			
			}

		}


		return wallCount == 0;

	}





	/*void OnDrawGizmos()
	{
		if (map == null)
			return;
		for (int x=0; x<mapWidth; x++) {
			for (int y=0; y<mapHeight; y++) {
				
				if(map[x,y]==FLOOR)
					Gizmos.color = Color.white;

				if(map[x,y]==OUTER_WALL)
					Gizmos.color = Color.black;

				if(map[x,y]==ROOM_WALL)
					Gizmos.color = Color.grey;

				Gizmos.DrawCube(new Vector3(-mapWidth/2 + x + 0.5f,-mapHeight/2 + y + 0.5f,0f),Vector3.one);


			}
		}

	




	}*/
}
