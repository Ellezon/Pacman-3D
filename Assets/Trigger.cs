using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour 
{


	IEnumerator OnTriggerEnter(Collider other) 
	{ 
		if (other.tag == "Pacman") 
		{	
			gameObject.transform.position = new Vector3(0,-30,0);
			GhostInteraction.ghostsVulnerable = true;
			GameObject[] ghost = GameObject.FindGameObjectsWithTag ("Ghost");
			GameObject[] spheres = GameObject.FindGameObjectsWithTag ("Sphere");
			Color spherecolor = spheres[0].transform.GetComponent<Renderer> ().material.color;
			Color[] currentColor = new Color[ghost.Length];
			int i = 0;
			foreach (GameObject g in ghost) 
			{
				currentColor[i] = g.transform.GetComponent<Renderer> ().material.color;
				i++;
				g.transform.GetComponent<Renderer> ().material.color = new Color (0.2f, 0.2f, 0.5f, 1.0f);
			}
			foreach (GameObject s in spheres) 
			{
				s.transform.GetComponent<Renderer> ().material.color = new Color (0f, 0.5f, 0.5f, 1.0f);
			}
			i = 0;
			yield return StartCoroutine(Wait());
			foreach (GameObject g in ghost) 
			{
				g.transform.GetComponent<Renderer> ().material.color = currentColor[i];		
				GhostInteraction.ghostsVulnerable = false;
				RedGhostMovement.gohome = true;
				i++;
			}	
			foreach (GameObject s in spheres) 
			{
				s.transform.GetComponent<Renderer> ().material.color = spherecolor;		
			}	
			gameObject.SetActive (false);
		}	
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(10);

	}


}
