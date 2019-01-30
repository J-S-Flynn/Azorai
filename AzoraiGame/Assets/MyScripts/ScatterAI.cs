using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ScatterAI : MonoBehaviour {

	private enum scatterState{

		init,
		setup,
		exploreTeritory, 
		attack, 
		escape, 
	}

	//stats 
	public float speed ; 
	private float maxHealth ; 
	private float curHealth ; 
	private float strength; 
	private float sight  ; 
	private float timer = 0;
	private float npcHealth; 
	private float npcStrength; 

	public Transform[] navPiont;
	public Transform scattterMouth;
	public GameObject scatBall;
	public GameObject spore;

	private GameObject target; 
	private GameObject closestHide = null; 
	private NavMeshAgent scatter; 
	private bool isAlive = true ; 
	private scatterState currentState; 
	private SphereCollider spCollid; 

	 
	public float getStrength(){
		return strength; 
	}

	public void setStrength(float str){
		strength = str;
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
			currentState = ScatterAI.scatterState.escape; 	
		}
		
		if(col.CompareTag("Azorai")){
			//print ("The Player entered the sphere"); 
			currentState = ScatterAI.scatterState.attack;
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
	//	print ("checking for Scatter Pod components");

		spCollid = GetComponent<SphereCollider>(); 
		scatter = GetComponent<NavMeshAgent>() ;

		if (spCollid == null) {
			Debug.LogError ("Scatter Pod spCollider is not there");
			return; 
		}

		currentState = ScatterAI.scatterState.setup;
	}

	public void setup(){

    //  print ("seting up the Scatter Pods components");

		spCollid.radius = sight ; // sets the size of the sphercollider 
		scatter.speed = speed;  // sets the speed of the Scatterpod 
		scatter.autoBraking = true;  // allows the scatterpod to breack when reaching its goal destination 

	//	print ("setup of the Scatter Pod compleat \n\n"); 
		currentState = ScatterAI.scatterState.exploreTeritory;
	}

	private void exploreTeritory(){

		if (curHealth <= 0) {
			dead ();
		}
		//print ("we are in the explore phase"); 
		//if (scatter.remainingDistance < 0.5f) {

			Vector3 explore = Random.insideUnitSphere * 10f; 

			NavMeshHit navHit; 
			NavMesh.SamplePosition (transform.position + explore, out navHit, 10f, NavMesh.AllAreas); 

			scatter.SetDestination (navHit.position);


		//}
	}

	private void attack(){


		if (curHealth <= 0) {
			dead ();
		}
		//print ("we are in the attack phase"); 
		target = GameObject.FindGameObjectWithTag ("Azorai"); 

		if (Vector3.Distance (target.transform.transform.position, transform.position) < 2f) {


			//Vector3 dist = new Vector3 (target.transform.position.x + 3, target.transform.position.y, target.transform.position.z); 
			scatter.destination = transform.localPosition ;

			spit ();
			timer = timer + 1;
		} else {
			scatter.SetDestination (target.transform.position); 
					 
			if (Vector3.Distance (target.transform.position, transform.position) > 10f) {
				currentState = ScatterAI.scatterState.escape; 
			}
		}
	}

	private void escape(){

		findClosestScatterHide (); 
		 
		scatter.destination = closestHide.transform.position;
		currentState = ScatterAI.scatterState.exploreTeritory;

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

	GameObject findClosestScatterHide(){
		GameObject[] hides; 
	    float dist;
		float currentDist;
		Vector3 loci;
		Vector3 diff;

		dist = Mathf.Infinity;
		loci = transform.position;
		hides = GameObject.FindGameObjectsWithTag ("ScatterHide");

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

	void dead (){

		int sporeChance = Random.Range (1, 1);

		//if (sporeChance == 1) {
		Instantiate (spore, transform.position, Quaternion.identity);
		//}
		Destroy (gameObject);


	}
	void Start () {

		currentState = ScatterAI.scatterState.init;
		StartCoroutine (scatterFSM()) ; 
	}
}
