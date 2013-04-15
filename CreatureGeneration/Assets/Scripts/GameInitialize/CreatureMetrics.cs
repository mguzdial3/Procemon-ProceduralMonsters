using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureMetrics : MonoBehaviour {
	//For this run, holds the player preferences for creatures (negative is bad, positive is good.)
	public static Dictionary<int, float> creaturePrefs;
	
	
	//THEY ONLY AREN'T CONSTANTS SO THEY ARE PUBLICLY ACCESSIBLE
	
	//How much to decrement when a character is switched out
	public static float DECREMENT_SWITCH =-0.5f;
	//How much to increment when a character is switched in
	public static float INCREMENT_SWITCH =0.5f;
	
	//How much to alter the creatures score when attempting to catch that creature
	public static float ATTEMPT_TO_CATCH = 0.1f;
	
	
	
	//Reset the dictionary for this playthrough
	public static void setup(){
		creaturePrefs = new Dictionary<int, float>();
		
		
	}
	
	//Alter the score (value) given a key
	public static void alterScore(int key, float alteration){
		//Debug.Log ("Score Altered");
		//If we don't already have this key, add it
		if(!creaturePrefs.ContainsKey(key)){
			creaturePrefs.Add(key, alteration);
			
		}
		else{//else alter it
			creaturePrefs[key] +=alteration;
		}
	}
	
	
}
