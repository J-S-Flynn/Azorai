using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playCam : MonoBehaviour {
	private float turnSpeed = 2f ; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (Input.GetKey (KeyCode.RightShift)){

			if (Input.GetKey (KeyCode.UpArrow)) {
				transform.Rotate (Vector3.right, -turnSpeed); ; 
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				transform.Rotate (Vector3.right, turnSpeed) ; 
			}
		}
	}
}
