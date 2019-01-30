using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SpitterAI : MonoBehaviour {
	private enum scatterState{

		init,
		setup,
		exploreTeritory, 
		attack, 
		escape, 
	}

	//stats 
	public float speed = 1.5f; 
	private float maxHealth = 10; 
	private float curHealth = 10; 
	private float strength = 2; 
	private float sight = 0.25f; 
	private float timer = 0;
	private float npcHealth; 
	private float npcStrength; 

	public Transform[] navPiont;
	public Transform scattterMouth;
	public GameObject scatBall;


	private GameObject target; 
	private GameObject closestHide = null; 
	private NavMeshAgent scatter; 
	private bool isAlive = true ; 
	private scatterState currentState; 
	private SphereCollider spCollid; 


	public float getStrength(){
		return strength; 
	}

	public void setStrength(float sStrength){
		strength = sStrength;
	}
	public float getHealth(){
		return maxHealth;
	}

	public void setHealth(float sHealth){
		maxHealth = sHealth;
	}

	public float getSpeed() {
		return speed; 
	}

	public void setSpeed(float sSpeed){
		speed = sSpeed; 
	}

	public void setSight(float sSight){
		sight = sSight;
	}

	void OnTriggerEnter(Collider col) {
		if(col.CompareTag("Border")){
			//print ("we hit the border of the town");
			currentState = SpitterAI.scatterState.escape; 	
		}

		if(col.CompareTag("Azorai")){
			//print ("The Player entered the sphere"); 
			currentState = SpitterAI.scatterState.attack;
		}
	}
	//FSM for enamy 

	private IEnumerator scatterFSM(){
		while (isAlive) {

			switch (currentState) {

			case scatterState.init:
				init (); 
				break;
			case scatterState.setup:
				setup ();
				break;
			case scatterState.exploreTeritory:
				exploreTeritory (); 
				yield return new WaitForSeconds (Random.Range (1, 5)); 
				break;
			case scatterState.attack:	
				attack (); 
				break;
			case scatterState.escape:
				escape (); 
				break; 
			}		
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void init(){

		spCollid = GetComponent<SphereCollider>(); 
		scatter = GetComponent<NavMeshAgent>() ;

		if (spCollid == null) {
			Debug.LogError ("Spitter Pod spCollider is not there");
			return; 
		}

		currentState = SpitterAI.scatterState.setup;
	}

	public void setup(){

		spCollid.radius = sight ; // sets the size of the sphercollider 
		scatter.speed = speed;  // sets the speed of the Spitterpod 
		scatter.autoBraking = true;  // allows the scatterpod to breack when reaching its goal destination 

		//	print ("setup of the Spitter Pod compleat \n\n"); 
		currentState = SpitterAI.scatterState.exploreTeritory;
	}

	private void exploreTeritory(){

		//print ("we are in the explore phase"); 
		if (scatter.remainingDistance < 0.5f) {

			Vector3 explore = Random.insideUnitSphere * 10f; 

			NavMeshHit navHit; 
			NavMesh.SamplePosition (transform.position + explore, out navHit, 10f, NavMesh.AllAreas); 

			scatter.SetDestination (navHit.position); 
		}
	}

	private void attack(){

		//print ("we are in the attack phase"); 
		target = GameObject.FindGameObjectWithTag ("Azorai"); 

		if (Vector3.Distance (target.transform.transform.position, transform.position) < 4f) {

			Vector3 dist = new Vector3 (target.transform.position.x + 3, target.transform.position.y, target.transform.position.z); 
			scatter.SetDestination (dist);

			Quaternion targDirection = Quaternion.LookRotation (dist, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, targDirection, Time.deltaTime * 2.0f);

			spit ();
			timer = timer + 1;
		} else {
			scatter.SetDestination (target.transform.position); 

			if (Vector3.Distance (target.transform.position, transform.position) > 10f) {
				currentState = SpitterAI.scatterState.escape; 
			}
		}
	}

	private void escape(){

		findClosestSpitterHide (); 

		scatter.destination = closestHide.transform.position;
		currentState = SpitterAI.scatterState.exploreTeritory;

		curHealth = maxHealth;
	}



	private void spit(){

		if (timer == (10 - speed)) {

			GameObject spitBall;
			spitBall = Instantiate (scatBall, scattterMouth.position, transform.rotation);

			spitBall.transform.parent = gameObject.transform;
			timer = 0;
		}
	}

	GameObject findClosestSpitterHide(){
		GameObject[] hides; 
		float dist;
		float currentDist;
		Vector3 loci;
		Vector3 diff;

		dist = Mathf.Infinity;
		loci = transform.position;
		hides = GameObject.FindGameObjectsWithTag ("SpitterHide");

		foreach(GameObject hide in hides){
			diff = hide.transform.position - loci; 
			currentDist = diff.sqrMagnitude;

			if (currentDist < dist) {
				closestHide = hide;
				dist = currentDist; 
			}
		}

		return closestHide;
	}

	void Start () {




		currentState = SpitterAI.scatterState.init;
		StartCoroutine (scatterFSM()) ; 
	}
}
