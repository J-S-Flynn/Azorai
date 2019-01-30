using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerBallScript : MonoBehaviour {
	private float ballTimer = 2f; 
	private float speed = 3f;
	private float damage;



	void OnTriggerEnter(Collider col){
		if (col.CompareTag ("scatHealth")) {

			col.gameObject.SendMessage ("applyAziDamage", damage);  
			CancelInvoke ();  
			destroyPowerBall ();

			print("we hit an scatterpod for " + damage);
		}
	}

	void Start(){

		damage = gameObject.GetComponentInParent<AzoraiAI> ().getStrength ();
		Invoke ("destroyPowerBall", ballTimer);  
	}

	void Update(){
		gameObject.transform.position += transform.forward * speed * Time.deltaTime; 
	}

	void destroyPowerBall(){
		Destroy (gameObject);
	}


}
