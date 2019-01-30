using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatHealth : MonoBehaviour {

	private float maxHealth = 100;
	private float curHealth = 30 ;
	private bool isAlive = true ; 
	private liveState currentState ;


	public float getHealth(){
		return curHealth; 
	}

	void applyAziDamage(float dam){

		curHealth = curHealth - dam;

	}
		
	private enum liveState {
		setup,
		alive,
		dead,
	}

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

		currentState = ScatHealth.liveState.alive;
	}

	private void alive(){

		if (curHealth == 0) {
			currentState = ScatHealth.liveState.dead;
		}
	}

	private void dead(){
		Destroy (transform.parent.gameObject);
	}


	// Use this for initialization
	void Start () {
		StartCoroutine (aliveFSM());

		currentState = ScatHealth.liveState.setup;
	}
}
