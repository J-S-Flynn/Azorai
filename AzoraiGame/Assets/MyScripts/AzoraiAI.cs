using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * This is the main brain for the Azorai 
 * */

public class AzoraiAI : MonoBehaviour {

	/**
	 * all the possible states for the azorai
	 * */
	private enum azoraiState{
		init,
		setup,
		stayInVillage, 
		exploreWorld, 
		advantageFearCheck, 
		escape,
		gather, 
		attack, 
		playerControle, 
	} 
 
	//Azorai Navagation 
	public Transform[] navPiont ; 
	public Transform powerPiont;
	public GameObject powerBall;

	//azorai camra
	public Camera aziCam;
	public Camera playCam;

	//variabls for targeting items 
	private Vector3 itemLoci ;
	private GameObject target;
	private NavMeshAgent azi;  
	private Rigidbody rig;
	private int destPiont = 0; 
	 
	//Azorai Stats 
	private float strength = 10f;
	private float sight = 7f; 
	private float maxHealth;
	private float curHealth ; 
	private float speed = 1.5f ; 
	private float timer = 0;
	//private float intalect = 5; 

	//Usable variables 
	private bool roam ; 
	private bool playerControled = false;
	private bool isAlive = true ; //is the charicter alive 
	private bool isInfected = false; 
	private bool isPiosend = false; 


	private azoraiState currentState ; // istantiates the states of the azorai 
	private SphereCollider spCollid ; // creats a new Spher collider 

	/**
	 * variables fro outside objects 
	 * */

	private GameObject bossObj; 
	private bossAzorai bossScript;  

	private float npcHealth; 
	private float npcStrength;

	/**
	 * These getters and setters are for the spawn pionts for azorai. and to allow scatterpods 
	 * and Spitterpods to get values for AdvantageFearCjheck 
	 * */

	public void setInfected(bool inf){
		isInfected = inf;
	}

	public void setPiosend(bool pos){
		isPiosend = pos; 
	}

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

	/**
	 * on trigger events are called when two coliders make contact
	 * the colliders are checked for a tacg and the code for the object with that tag, if present is 
	 * exicuted
	 * */
	void OnTriggerEnter(Collider col) {

		//this code controles what happens t a collider on a navPiont when collision occurs 
		if(col.CompareTag("AziNav")){ 

			SphereCollider navSPhere = col.GetComponent<SphereCollider> ();
			navSPhere.radius = 0; 

			System.Array.Resize (ref navPiont, (navPiont.Length + 1));


			navPiont [navPiont.Length - 1] = col.gameObject.GetComponent<Transform> (); 

			azi.destination = navPiont [navPiont.Length - 1].position;  
		}

		// controls how an Azorai reacts to contact with a Scatterpod 
		if (col.CompareTag ("ScatterPod")) {

			npcHealth = col.gameObject.GetComponent<ScatterAI> ().getHealth();
			npcStrength = col.gameObject.GetComponent<ScatterAI> ().getStrength();

			//print ("stats of the npc " + npcHealth + " and " + npcStrength);
			if (playerControled == false) {
				currentState = AzoraiAI.azoraiState.advantageFearCheck;
			}


		}

		// these statments controle what an Azorai should do if it sees an item 
		if (col.CompareTag("Land")){
			print("we hit the land");
		}

		if(col.CompareTag("water")){

			azi.destination = col.transform.position;
		}
		if(col.CompareTag("ScatterCure")){
			
			azi.destination = col.transform.position;
		}
		if(col.CompareTag("SpitterCure")){

			azi.destination = col.transform.position;
		}
		if(col.CompareTag("Spore")){

			azi.destination = col.transform.position;
		}
		if(col.CompareTag("Piosen")){

			azi.destination = col.transform.position;
		}
		if(col.CompareTag("Food")){

			azi.destination = col.transform.position;
		}
		if(col.CompareTag("Totem")){

			azi.destination = col.transform.position;
		}
	}


	//FSM controles the behavior of the Azorai 
	private IEnumerator azoraiFSM() {

		while (isAlive) {

			switch (currentState) {

				case azoraiState.init:
					init ();
					break;
				case azoraiState.setup:
					setup (); 
					break;
				case azoraiState.stayInVillage: 
					stayInVillage();
					break;
				case azoraiState.exploreWorld:
					exploreWorld();
					yield return new WaitForSeconds (Random.Range (1, 5));
					break;
				case azoraiState.advantageFearCheck:
					advantageFearcheck(); 
					break;
				case azoraiState.attack:
					attack();
					break; 
				case azoraiState.escape:
					escape ();
				yield return new WaitForSeconds (Random.Range(1,5));
					break; 
				case azoraiState.gather:
					gather(); 
					break; 
				case azoraiState.playerControle:
					playCon();
					break;
			}

			yield return null ;
		}

	}


	/**
	 * gets all components for the Azorai
	 * */
	private void init(){

		//gameObject.GetComponent<Renderer> ().material.color = Color.cyan;
		curHealth = gameObject.GetComponentInChildren<AziHealth> ().getHealth();

		bossObj = GameObject.FindGameObjectWithTag ("Boss");
		bossScript = bossObj.GetComponent<bossAzorai> ();


		azi = GetComponent<NavMeshAgent>() ;
		azi.autoBraking = true; 

		spCollid = GetComponent<SphereCollider>();

		if (spCollid == null) {
			Debug.LogError ("spCollider is not there mate");
			return; 
		}
 
		currentState = AzoraiAI.azoraiState.setup;
	}


	/**
	 * setup for the Azorai components
	 * */
	private void setup(){

		spCollid.radius = sight;
		azi.speed = speed;

		if(roam.Equals(false)){
			currentState = AzoraiAI.azoraiState.stayInVillage;
		}
		else{
			currentState = AzoraiAI.azoraiState.exploreWorld;
		}
	}


	/**
	 *  What the Azorai should do if it is in the village
	 * */
	private void stayInVillage(){

		blast ();
		roam = bossScript.getRoam();

		if (navPiont.Length == 0) {
			return;
		} 
			
		if(azi.remainingDistance < 0.5f){
			
			azi.destination = navPiont[destPiont].position ;
			destPiont = Random.Range (0, 11) ;
		}
		if(roam.Equals(true)){
			azi.destination = navPiont [12].position;
			currentState = AzoraiAI.azoraiState.exploreWorld;
		}
	}
		
	/**
	 *  What the azorai should do if it is out in the world
	 * */
	private void exploreWorld(){ 

		roam = bossScript.getRoam();

		if (destPiont < (navPiont.Length - 1)) {

			destPiont = navPiont.Length - 1; 

			azi.destination = navPiont [destPiont].position;
		} else {
			if (azi.remainingDistance < 0.5f) {

				Vector3 explore = Random.insideUnitSphere * 15f; 

				NavMeshHit navHit; 
				NavMesh.SamplePosition (transform.position + explore, out navHit, 15f, NavMesh.AllAreas); 

				azi.SetDestination (navHit.position);
			}
			if (roam.Equals (false)) {
				if (azi.remainingDistance > 0) {
					currentState = AzoraiAI.azoraiState.stayInVillage;
				}
			}
		}
	}


	/**
	 * Advantage fear check is performed when the azorai meets and enamy 
	 * */
	private void advantageFearcheck() {
 
		float fearLevel;
		float advantageCheck;
		float fearCheck; 

		fearLevel = Random.Range (1, 100);
		advantageCheck = ((curHealth + strength) - (npcHealth + npcStrength)) / fearLevel;
		fearCheck = strength / fearLevel; 

		if (advantageCheck > fearCheck) {
			currentState = AzoraiAI.azoraiState.attack;
		} else {
			//print ("we are scared ... run away ... RUN AWAY"); 
			currentState = AzoraiAI.azoraiState.escape; 
		}
	}


	/**
	 * if the azorai thinks itcan win do this 
	 * */
	private void attack () {

		target = GameObject.FindGameObjectWithTag ("ScatterPod"); 

		if (Vector3.Distance (target.transform.transform.position, transform.position) < 2f) {

			Vector3 dist = new Vector3 (target.transform.position.x + 2, target.transform.position.y, target.transform.position.z); 

			//azi.SetDestination (dist);
			blast ();
			timer = timer + 1;

		} else {
			blast ();
			//azi.SetDestination (target.transform.position); 

			if (Vector3.Distance (target.transform.position, transform.position) > 10f) {
				currentState = AzoraiAI.azoraiState.escape; 
			}
		}
	
		if (isInfected == true) {
			gameObject.GetComponent<Renderer> ().material.color = Color.cyan;
			//print (" I HAVE BEEN INFECTED BY A SCATTERPOD");
		}
		//print ("we are gong to attack now");
	}


	/**
	 * if the Azorai thinks it may die do this
	 * */
	private void escape () {

		roam = bossScript.getRoam(); 

		azi.destination = navPiont [0].position;

		if (azi.remainingDistance < 0.5f) {

			if (roam == true) {

				azi.destination = navPiont [navPiont.Length - 1].position;
				currentState = AzoraiAI.azoraiState.exploreWorld;
			} else if(roam == false) {
				currentState = AzoraiAI.azoraiState.stayInVillage; 
			}
		}
	}

	/**
	 * how the Azorai behavis if it finds a gatherable objects 
	 * */ 
	private void gather() {
	
	}
		
	/**
	 * controls how the Azorai behaves if the player is in controle. 
	 * */
	void playCon(){

		float moveSpeed = 5f;
		float turnSpeed = 50f;



		//transform.Translate(speed * Input.GetAxis("Horizontal") *  Time.deltaTime , 0f,  speed * Input.GetAxis("Vertical") * Time.deltaTime);

		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.S)) {
			transform.Translate (-Vector3.forward * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.up, turnSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.RightShift)) {
			blast ();
		}
		
		if(Input.GetKeyDown (KeyCode.Return)){
			aziCam.enabled = false;
			playCam.enabled = true;
			playerControled = false;

			Start ();

			if(roam == true){
				currentState = AzoraiAI.azoraiState.exploreWorld;
			}
			else{
				currentState = AzoraiAI.azoraiState.stayInVillage;	
			}
		}
	}

	void switchToAzi(){
		playerControled = true;
		playCam.enabled = false;
		aziCam.enabled = true;

		azi.Stop ();
		currentState = AzoraiAI.azoraiState.playerControle;
	}

	private void blast(){

		if (playerControled == false) {
			if (timer == (10 - speed)) {

				GameObject pBall;
				pBall = Instantiate (powerBall, powerPiont.transform.position, transform.rotation);

				pBall.transform.parent = gameObject.transform;
				timer = 10;
			}
		}
	
		else {

			/**
			 * this code is used to creat a new projectile for the azorai. due to the way it attaches the 
			 * projectile as a child object of the azorai it creats a strange glitch that means if the Azorai tuns the 
			 * projectile will move to be in frount of it still. this means that a projectile apeears to be rinning along an
			 * invisable pole that the azorai has in frunt of it. 
			 * a good whay to fix this might be to add the child access the variable then remove the child 
			 * so that it can move alonge its own vector 
			 * */

			GameObject pBall;
			pBall = Instantiate (powerBall, powerPiont.transform.position, transform.rotation);

			pBall.transform.parent = gameObject.transform;
		
			timer = 10;
		}
	}

	// Use this for initialization
	void Start () {
		 


		aziCam.enabled = false;

		currentState = azoraiState.init;

		StartCoroutine (azoraiFSM());

	}
		
}

