using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tiles : MonoBehaviour
{
	//the size of the grid, sometimes non-square grid may be useful
	public float width = 32.0f;
	public float height = 32.0f;

	//offset, just in case someone wants to adjust the position of the grid
	public float offsetX = 0.0f;
	public float offsetY = 0.0f;

	//the color of the lines, someone it has to be adjusted for better visibility
	public Color color = Color.white;

	//shortcuts 
	public string drawKey = "";
	public string deleteKey = "";
	public string disableKey = "";
	public string alignKey = "";
	public string setParentKey = "";

	//depth += 1.0f shortcut
	public string incDepthKey = "";
	//depth -= 1.0f shortcut
	public string decDepthKey = "";

	//depth at which the tiles are placed
	public float depth = 0.0f;

	//objects' offset from grid
	public float objOffsetX = 0.0f;
	public float objOffsetY = 0.0f;

	//is tile placing enabled
//	public bool enabled = false;

	//the tiles' parent object
	public Transform parent;

	void OnDrawGizmos()
	{
		if (!enabled)
			return;

		//Camera.current gets us the sceneview's camera
		Camera c = Camera.current;
		Vector3 cPos = c.transform.position;

		Gizmos.color = color;

		//draw horizontal lines... I cheat length and how many lines are drawn a bit here, ugly but works
		//float.MinValue, float.MaxValue and float.NegativeInfinity, float.PositiveInfinity didn't work
		for (float y = cPos.y - c.orthographicSize*4.0f; y < cPos.y + c.orthographicSize*4.0f; y+= height)
		{

			Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y/height) * height + offsetY, 0.0f), 
				new Vector3(1000000.0f, Mathf.Floor(y/height) * height + offsetY, 0.0f));
		}

		//pretty much the same thing for the vertical lines
		for (float x = cPos.x - c.orthographicSize*4.0f; x < cPos.x + c.orthographicSize*4.0f; x+= height)
		{

			Gizmos.DrawLine(new Vector3(Mathf.Floor(x/width) * width + offsetX, -1000000.0f, 0.0f), 
				new Vector3(Mathf.Floor(x/width) * width + offsetX, 1000000.0f, 0.0f));
		}
	}
}