using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public int orgX;
	public int orgY;

	public int width;
	public int height;

	public int centerX;
	public int centerY;

	public bool isConnected;

	public Room(int x, int y, int w, int h)
	{
		isConnected = false;
		orgX = x;
		orgY = y;
		width = w;
		height = h;

		centerX = x + width / 2;
		centerY = y + height / 2;
	}
}
