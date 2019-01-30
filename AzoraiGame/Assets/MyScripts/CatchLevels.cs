using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchLevels : MonoBehaviour {

	private int water =  0;
	private int scatCure = 0;
	private int spitCure = 0;
	private int spore = 0 ;
	private int piosen = 0 ;
	private int food = 0 ; 
	private int totam = 0 ;
	// Use this for initialization

	public int getWater(){
		return water;
	}

	public void setWater(int sWater){
		water = sWater;
	}

	public int getScatCure(){
		return scatCure;
	}

	public void setScatCure(int scCure){
		scatCure = scCure;
	}

	public int getSpitCure(){
		return spitCure;
	}

	public void setSpitCure( int spCure){
		spitCure = spCure;	
	}

	public int getSpore(){
		return spore;
	}

	public void setSpore(int sSpore){
		spore = sSpore;
	}

	public int getPiosen(){
		return piosen;
	}
		
	public void setPiosen(int sPiosen){
		piosen = sPiosen;
	}

	public int getFood(){
		return food;
	}

	public void setFood(int sFood){
		food = sFood; 
	}

	public int getTotam(){
		return totam;
	}

	public void setTotem(int sTotem){
		totam = sTotem;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
