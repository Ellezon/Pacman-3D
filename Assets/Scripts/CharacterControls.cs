using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class CharacterControls : MonoBehaviour {
	public Vector3 prevPos, currentPos;
	public static Vector3 dir;
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	private bool grounded = false;
	public  Text countText;
	public  Text countText1;
	public  static int points = 0;
	
	
	void Awake () 
	{
		currentPos = gameObject.transform.position;
		GetComponent<Rigidbody>().freezeRotation = true;
		GetComponent<Rigidbody>().useGravity = false;
	}
	
	void FixedUpdate () 
	{
		if (grounded) 
		{
			prevPos = currentPos;
			currentPos = gameObject.transform.position;
			dir = currentPos- prevPos;
			// Calculate how fast we should be moving
			if(Input.GetKey(KeyCode.LeftArrow))
				transform.Rotate(new Vector3(0,1,0), -speed *Time.deltaTime);
			
			if(Input.GetKey(KeyCode.RightArrow))
				transform.Rotate(new Vector3(0,1,0), speed * Time.deltaTime);
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Vertical"), 0, 0);
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;
			
			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
			

		}
		
		// We apply gravity manually for more tuning control
		GetComponent<Rigidbody>().AddForce(new Vector3 (0, -gravity * GetComponent<Rigidbody>().mass, 0));
		
		grounded = false;
	}
	
	void OnCollisionStay () 
	{
		grounded = true;    
	}
	


	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Sphere"))
		{
			other.gameObject.SetActive (false);
			points ++;
			SetCountText();
		}
		else if (other.gameObject.CompareTag ("Fruit"))
		{
			other.gameObject.SetActive (false);
			points = points + 10;
			SetCountText();
		}
	}
	void SetCountText()
	{

		if (points >= 391) {
			countText.text = "You win!";
			countText1.text = "You win!";
			Time.timeScale = 0.001f;
			Time.timeScale = 1;
			Application.LoadLevel (Application.loadedLevelName);
			points = 0;
		} else {
			countText.text = "Score: " + points.ToString ();
			countText1.text = "Score: " + points.ToString ();
		}
	}

}
