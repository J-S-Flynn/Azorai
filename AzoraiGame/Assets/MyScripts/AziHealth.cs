using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The azi Health script controls the looks after the helth of an azorai.
 **/ 

public class AziHealth : MonoBehaviour {

	// helth variabls 
	private float maxHealth = 100;
	private float curHealth = 30  ;
	private bool isAlive = true ; 
	private liveState currentState ;

	// accessos for other scripts to get the elth of the azorai 
	public float getHealth(){
		return curHealth; 
	}

	// should an enamy damage the azorai this function will aply that damage
	void applyScatDamage(float dam){

		curHealth = curHealth - dam;
		int infected = 1;

		if (infected == 1) {
			gameObject.GetComponentInParent<AzoraiAI> ().setInfected (true);
		}

	}

	// should an enamy damage the azorai this function will aply that damag
	void applySpitDamage(float dam){
	
		curHealth = curHealth - dam;
	}
		
	//states availible for health 
	private enum liveState {
		setup,
		alive,
		dead,
	}

	//finite state machine for swithching between states 
	private IEnumerator aliveFSM(){
	
		while (isAlive) {
		
			switch (currentState) {
			case liveState.setup:
				setup ();
				break;
			case liveState.alive:
				alive ();
				break;
			case liveState.dead:
				dead();
				break;
			}
			yield return null; 
		}
	}

	private void setup(){

		currentState = AziHealth.liveState.alive;
	}

	// while the azorai is alive this state will always run 
	private void alive(){

		if (curHealth == 0) {
			currentState = AziHealth.liveState.dead;
		}
	}

	// if health drops to 0 destroy the object
	private void dead(){
		Destroy (transform.parent.gameObject);
	}


	// Use this for initialization
	void Start () {

		//like starting a thread 
		StartCoroutine (aliveFSM());
	}
	

}
