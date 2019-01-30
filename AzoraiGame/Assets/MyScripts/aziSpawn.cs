using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aziSpawn : MonoBehaviour {
	public Transform spawnPiont;
	public GameObject aSpawn;

	// variables for storing new stats for created Azorai 
	private float health ;
	private float strength;
	private float speed;
	private float sight; 
	// Use this for initialization


	void Start () {

		/**
		 * when the Azorai is created the new stats will be randamly assigned 
		 * between the valus presented
		 * */

		health = Random.Range (10, 100);
		strength = Random.Range (10, 100);
		speed = Random.Range (1f, 3f);
		sight = Random.Range (0.10f, 0.50f);

		Invoke ("spawn", 0.1f); // creats the Azorai 
	}

	void Update(){



	}
	// Update is called once per frame
	void spawn (){

		/**
		 * this method creats the new azorai and sets the new stats to that of the new azorai 
		 * */ 

		GameObject azorai;

		azorai = Instantiate (aSpawn, spawnPiont.position, Quaternion.identity);

		//azorai.GetComponent<AzoraiAI> ().setHealth (health);
		azorai.GetComponent<AzoraiAI> ().setStrength (strength);
		azorai.GetComponent<AzoraiAI> ().setSpeed (speed);
		azorai.GetComponent<AzoraiAI> ().setSight (sight);


	}
}
