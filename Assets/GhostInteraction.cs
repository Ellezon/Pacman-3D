using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GhostInteraction : MonoBehaviour {
	public static bool ghostsVulnerable;
	public Text text;
	public Text text2;

	// Use this for initialization
	void Start () {
		ghostsVulnerable = false;
	}

	IEnumerator OnTriggerEnter(Collider other) 
	{   if (other.tag == "GhostMain") {
			if(!ghostsVulnerable)
			{
				text.text = "You Lost!";
				text2.text = "You Lost!";
				text.fontSize = 80;
				text2.fontSize = 80;
				Time.timeScale = 0.001f;
				yield return StartCoroutine(Wait());
				Time.timeScale = 1;
				Application.LoadLevel (Application.loadedLevelName);
			}
			if(ghostsVulnerable){
				CharacterControls.points+=200;
				text.text = "Score: " + CharacterControls.points.ToString();
				text2.text = "Score: " + CharacterControls.points.ToString();
				other.gameObject.transform.position = new Vector3(0,15,0);
			}
		}
	}
	IEnumerator Wait(){
		yield return new WaitForSeconds(0.002f);
	}



}
