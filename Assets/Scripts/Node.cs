using UnityEngine;
using System.Collections;

/*
 * Node :
 * seekerdistance - total distance from seeker to the current node
 * targetdistance - total distance from current node to target
 * totalcost - targetdistance + seekerdistance
 */

public class Node 
{
	// if true character can walk on this node
	public bool isWalkable;
	// will hold the position of node in world
	public Vector3 worldPosition;
	public int xPosGrid;
	public int yPosGrid;

	public int seekerdistance;
	public int targetdistance;
	public Node parent;

	public Node(bool is_Walkable, Vector3 worldpos, int gridx, int gridy) 
	{
		isWalkable = is_Walkable;
		worldPosition = worldpos;
		xPosGrid = gridx;
		yPosGrid = gridy;
	}
		
	public int totalcost 
	{
		get 
		{
			return seekerdistance + targetdistance;
		}
	}
}