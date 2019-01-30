using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AziBag : MonoBehaviour {

	public int water =  0;
	public int scatCure = 0;
	public int spitCure = 0;
	public int spore = 0 ;
	public int piosen = 0 ;
	public int food = 0 ; 
	public int totam = 0 ;

	void OnTriggerEnter(Collider col){

		print ("found somthing ");

		if(col.CompareTag("water")){
			water ++; 
			Destroy (col.gameObject);
		}
		if(col.CompareTag("ScatterCure")){
			scatCure ++;
			Destroy (col.gameObject);
		}
		if(col.CompareTag("SpitterCure")){
			spitCure ++; 
			Destroy (col.gameObject);
		}
		if(col.CompareTag("Spore")){
			spore ++; 
			Destroy (col.gameObject);
		}
		if(col.CompareTag("Piosen")){
			piosen ++;
			Destroy (col.gameObject);
		}
		if(col.CompareTag("Food")){
			food ++; 
			Destroy (col.gameObject);
		}
		if(col.CompareTag("Totem")){
			totam ++;
			Destroy (col.gameObject);
		}

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
