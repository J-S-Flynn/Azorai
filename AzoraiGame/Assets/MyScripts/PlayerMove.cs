using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	 
	//variables for player movemnt and object detection 
	private float moveSpeed = 10f ; 
	private float turnSpeed = 2f ; 
	private float castDist  = 1f  ;

	// variables for other game objects that may be used by the layer 
	private GameObject bossObj; 
	private GameObject aziBabe;
	private bossAzorai bossScript;  
	// Use this for initialization


	/*
	 * triigers are used when one objects colider hits anouther objects collider 
	 * the trigger event will perform a task
	 * this was a test method created to see if the Scaterpoods where spawned with 
	 * random variables for health and strength.
	 
	void OnTriggerEnter(Collider col) {
		if(col.CompareTag("ScatterPod")){
			//print("scater Health is " + col.GetComponent<ScatterAI>().getHealth());
			//print("scater Strength is " + col.GetComponent<ScatterAI>().getStrength());
		}
	}



	/**
	 * the start method is the first method is the first method called in the script
	 * */

	void Start () {

		bossObj = GameObject.FindGameObjectWithTag ("Boss");

		bossScript = bossObj.GetComponent<bossAzorai> ();

		//rBody = GetComponent<Rigidbody> (); 
		//capCollide = GetComponent<CapsuleCollider>(); 
	}
	  
	// Update is called once per frame
	void FixedUpdate () {


		Ray searchRay = new Ray (transform.position, transform.forward);
		RaycastHit objectHit; 

		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * castDist, Color.red);       

		if (Physics.Raycast (searchRay, out objectHit, castDist)) {
			if (objectHit.collider.tag == "Boss") {
				
				if(Input.GetKey(KeyCode.Return)){
					print ("You selected the Boss");

				}
			}
			if (objectHit.collider.tag == "Azorai") {

				print ("there is an azorai there");

				if(Input.GetKey(KeyCode.Return)){

					/**
					 * sendMessage is a way of sending a message to the object in collision 
					 * in the case of this message it will send the switchToAzi message.
					 * this call a method in the Azorais main script "AzoraiAI" called switchToAzi 
					 **/

					objectHit.collider.gameObject.SendMessage ("switchToAzi");


					//Destroy (gameObject);
				}
			}
		}

		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector3.forward / moveSpeed) ; 
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.Translate (Vector3.back / moveSpeed) ; 
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (Vector3.left / moveSpeed) ; 
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector3.right / moveSpeed) ; 
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Translate (Vector3.up / moveSpeed) ; 
		}
		if (Input.GetKey (KeyCode.DownArrow)) {

			transform.Translate (Vector3.down / moveSpeed) ; 
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate (Vector3.up, -turnSpeed);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate (Vector3.up, turnSpeed); 
		}
		if (Input.GetKeyDown (KeyCode.Space)) {

			bool roamOn = bossScript.getRoam();

			roamOn = !roamOn;

			bossScript.setRoam (roamOn);
			//print ("Set roam to " + roamOn);
		}
		 
	}
}
