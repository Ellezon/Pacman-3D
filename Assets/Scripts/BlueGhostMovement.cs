using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BlueGhostMovement : MonoBehaviour
{
	Pathfinding pf;
	public Transform seeker, target;
	public List<Node> path = new List<Node>();
	public GameObject astar;
	public GameObject redGhost;
	public Vector3 home;
	bool gohome = true;
	// Use this for initialization
	void Start () {
		astar = GameObject.Find ("A*");
		pf = astar.GetComponent<Pathfinding>();
		redGhost = GameObject.Find ("Red Ghost");
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 targetVelocity = CharacterControls.dir;
		//Debug.Log (targetVelocity);
		Vector3 predictedNode = target.position + (targetVelocity*30);
		Vector3 predictedDestination = 2*(predictedNode - redGhost.transform.position); 
		if (GhostInteraction.ghostsVulnerable) {
			if (GhostInteraction.ghostsVulnerable) {
				if( seeker.position ==  new Vector3(0,15,0))
				{
					path = pf.FindPath(seeker.position,home);
				}
				if(gohome == false) 
				{
					path = pf.FindPath(seeker.position,new Vector3(0,15,0));
				}
				else if(seeker.position != new Vector3(-445.0f,15.0f,-475.0f))
				{
					path = pf.FindPath(seeker.position,home);
				}
				else
				{
					Debug.Log("changing");
					gohome = false;
				}
			}

		}
		else if (pf.grid.PointToNode (predictedDestination).isWalkable)
			path = pf.FindPath (seeker.position, predictedDestination);
		else 
			path = pf.FindPath (seeker.position, target.position);
		
		Vector3 worldPoint;
		
		int x = 0;
		foreach(Node node in path)
		{	x++;
			if (x==5) break; 
			
			worldPoint = pf.worldLowerLeftCorner + Vector3.right * (node.xPosGrid * pf.grid.nodeSizeHalf*2 + pf.grid.nodeSizeHalf) + Vector3.forward * (node.yPosGrid * pf.grid.nodeSizeHalf*2 + pf.grid.nodeSizeHalf);
			worldPoint.y = 15;
			float step =  15 * Time.deltaTime;
			Vector3 look = new Vector3(target.position.x, 13, target.position.z);
			seeker.LookAt(look);
			seeker.position = Vector3.MoveTowards(seeker.position, worldPoint, step);
		}
	}
}