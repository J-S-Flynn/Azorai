using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitScript : MonoBehaviour {
	private float spitTimer = 4f; 
	private float speed = 3f;
	private float damage ;



	void OnTriggerEnter(Collider col){
		if (col.CompareTag ("aziHealth")) {

			/**
			 *  WE NEED TO GET THE DAMAGE TO COME FROM THE BASE STRENGTH OF THE POD
			 * */
					    
			col.gameObject.SendMessage ("applyScatDamage", damage);  
			CancelInvoke ();  
			destroySpit ();

			print("we hit an Azorai for " + damage);
		}
	}
		
	void Start(){

		damage = gameObject.GetComponentInParent<SpitterAI> ().getStrength ();
		Invoke ("destroySpit", spitTimer);  
	}

	void Update(){
		transform.position += transform.forward * speed * Time.deltaTime; 
	}
	 
	void destroySpit(){
		Destroy (gameObject);
	}


}
