using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	//unwalkable layer
	public LayerMask unwalkableMask;
	//dimensions of world  
	public Vector2 worldSize;
	//size of node
	public float nodeSizeHalf;
	//two dimensional array of nodes
	public Node[,] grid;
	//twice the size of node radius
	float nodeSize;
	//hold the number of nodes in the x and y axis of the grid
	public int gridSizeX, gridSizeY;
	public Vector3 center;
	
	public List<Node> path;
	
	void Start() 
	{
		nodeSize = nodeSizeHalf*2;
		//gets the number of nodes in the x and  y directions 
		gridSizeX = Mathf.RoundToInt(worldSize.x/nodeSize);
		gridSizeY = Mathf.RoundToInt(worldSize.y/nodeSize);
		CreateGrid();
	}
	
	void CreateGrid() 
	{
		//2 dimensional array of nodes 
		grid = new Node[gridSizeX,gridSizeY];
		center = transform.position;
		//holds co-ordinates of the bottom left corner of the world ie (0,0) in grid world
		Vector3 worldLowerLeftCorner = center - Vector3.right * worldSize.x/2 - Vector3.forward * worldSize.y/2;
		
		for (int x = 0; x < gridSizeX; x ++) 
		{
			for (int y = 0; y < gridSizeY; y ++) 
			{
				//get world position from the current grid co-ordinates 
				Vector3 worldPoint = worldLowerLeftCorner + Vector3.right * (x * nodeSize + nodeSizeHalf) + Vector3.forward * (y * nodeSize + nodeSizeHalf);
				//check if walkable
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeSizeHalf,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint,x,y);
			}
		}
	}
	
	
	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();
		
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0 ||Mathf.Abs(x) + Mathf.Abs(y) == 2)
					continue;
				
				int checkX = node.xPosGrid + x;
				int checkY = node.yPosGrid + y;
				
				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}
		
		return neighbours;
	}
	
	
	public Node PointToNode(Vector3 worldPosition) 
	{ //this method converts a world position to the equivalent position in the node grid
		
		//percentX/Y hold the poisition in terms of a percentage of the node in the world relative to the world dimensions
		//worldSize/2 is added to the current world position due to the fact that the center of the grid is in 0,0,0.
		//Therefore logically there are negative grid coordinates, however in the implementations 2D arrays start at 0,0. 
		//Therefore half the size of each dimension is added to overcome this implementation problem
		float percentX = (worldPosition.x + worldSize.x/2) / worldSize.x;
		float percentY = (worldPosition.z + worldSize.y/2) / worldSize.y;
		//make sure the percentage is between 0 and 1 (0 to 100%)
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		
		//the -1 is there to ensure that the x,y coords are not out of the bounds of the grid array
		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}
	


}