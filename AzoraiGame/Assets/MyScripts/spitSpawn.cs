using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spitSpawn : MonoBehaviour {

	public Transform spawnPiont;
	public GameObject sPod;

	private float health ;
	private float strength;
	private float speed;
	private float sight;
	private float spawnTime ;
	private float timer = 0 ; 
	// Use this for initialization
	void Start () {

		health = Random.Range (10, 100);
		strength = Random.Range (10, 100);
		speed = Random.Range (1f, 3f);
		sight = Random.Range (0.10f, 0.50f);
		spawnTime = Random.Range (10, 20);

		Invoke ("spawn", 0.1f);
	}

	void Update(){

		spawnTime = Random.Range (1000f, 3000f);
		timer += 1; 

		if (timer == spawnTime) {
			health = Random.Range (10, 100);
			strength = Random.Range (10, 100);
			speed = Random.Range (1f, 3f);
			sight = Random.Range (0.10f, 0.50f);
			spawnTime = Random.Range (10, 20);

			Invoke ("spawn", 0.1f);
			timer = 0; 
		}

	}
	// Update is called once per frame
	void spawn (){

		GameObject spitter;

		spitter = Instantiate (sPod, spawnPiont.position, Quaternion.identity);

		spitter.GetComponent<SpitterAI> ().setHealth (health);
		spitter.GetComponent<SpitterAI> ().setStrength (strength);
		spitter.GetComponent<SpitterAI> ().setSpeed (speed);
		spitter.GetComponent<SpitterAI> ().setSight (sight);


	}
}
