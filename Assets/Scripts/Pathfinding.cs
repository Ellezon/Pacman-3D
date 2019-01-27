using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * Node :
 * seekerdistance - total distance from seeker to the current node
 * targetdistance - total distance from current node to target
 * totalcost - targetdistance + seekerdistance
 */

public class Pathfinding : MonoBehaviour 
{
	public Transform seeker, target;
	public Grid grid;
	GameObject astar;
	public Vector3 worldPoint;
	public Vector3 worldLowerLeftCorner ;
	public List<Node> path;
	
	void Awake() 
	{	
		astar = GameObject.Find ("A*");
		grid = astar.GetComponent<Grid>();
		worldLowerLeftCorner = grid.center - Vector3.right * grid.worldSize.x/2 - Vector3.forward * grid.worldSize.y/2;
	}
	
	public  List<Node> FindPath(Vector3 startPos, Vector3 targetPos) 
	{
		//node containing seeker
		Node startNode = grid.PointToNode (startPos);
		//node containing target
		Node targetNode = grid.PointToNode (targetPos);
		//holds all nodes that are waiting to be checked
		List<Node> openSet = new List<Node> ();
		//holds all nodes that have been checked
		HashSet<Node> closedSet = new HashSet<Node> ();
		openSet.Add (startNode);
		
		//find the node with the shortest total cost or if the current node's total 
		//cost is the same as the next node's, choose based on shortest target distance 
		while (openSet.Count > 0) {
			Node currentNode = openSet [0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet [i].totalcost < currentNode.totalcost || (openSet [i].totalcost == currentNode.totalcost && openSet [i].targetdistance < currentNode.targetdistance)) {
					currentNode = openSet [i];
				}
			}
			
			openSet.Remove (currentNode);
			closedSet.Add (currentNode);
			
			//if target has been reached retrace path 
			if (currentNode == targetNode) {
				return findPath (startNode, targetNode);
				
			}
			
			//for each unchecked and isisWalkable neighbour of current node, calculate seeker distance, target distance and total cost and add it to openSet
			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.isWalkable || closedSet.Contains (neighbour)) {
					continue;
				}
				//calculates the seeker distance using the current nodes seeker distance
				int newMovementCostToNeighbour = currentNode.seekerdistance + DistanceBetNodes (currentNode, neighbour);
				//if the node has not been visited yet or the newly calculated seeker distance is smaller than the current seeker distance, 
				//update its values
				if (newMovementCostToNeighbour < neighbour.seekerdistance || !openSet.Contains (neighbour)) {
					neighbour.seekerdistance = newMovementCostToNeighbour;
					neighbour.targetdistance = DistanceBetNodes (neighbour, targetNode);
					neighbour.parent = currentNode;
					
					if (!openSet.Contains (neighbour)) {
						openSet.Add (neighbour);
					}
					
				}
			}
		}
		return null;
	}
	
	// gets shortest path from seeker to target by following the nodes' parents starting from end node 
	public List<Node> findPath(Node startNode, Node endNode) 
	{
		List<Node> path_t = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode)
		{
			path_t.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path_t.Reverse();
		
		path = path_t;
		return path;
		
	}
	
	public int DistanceBetNodes(Node nodeA, Node nodeB) 
	{
		int distanceX = Mathf.Abs(nodeA.xPosGrid - nodeB.xPosGrid);
		int distanceY = Mathf.Abs(nodeA.yPosGrid - nodeB.yPosGrid);
		
		if (distanceX > distanceY) 
		{
			//14 is the cost of diagonal movement 
			//10 is the cost of horizontal and vertical movement 
			//distance = the total diagonal distance required  + the remaining horizontal distance
			return 14 * distanceY + 10 * (distanceX - distanceY);
		}
		//distance = the total diagonal distance required  + the remaining vertical distance
		return 14*distanceX + 10 * (distanceY-distanceX);
	}
	
}



