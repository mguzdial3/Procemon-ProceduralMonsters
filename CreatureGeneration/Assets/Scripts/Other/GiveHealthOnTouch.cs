using UnityEngine;
using System.Collections;

public class GiveHealthOnTouch : MonoBehaviour {
	
	//When player enters into, heal all creatures
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			PlayerCreatureHolder creatureHolder = other.GetComponent<PlayerCreatureHolder>();
			PlayerMovement movement = other.GetComponent<PlayerMovement>();
			
			movement.setOrigPosition();
			
			for(int i = 0; i<creatureHolder.myCreatures.Length; i++){
				creatureHolder.myCreatures[i].healCreature();
			}
		}
	}
	
	//When player stays, heal all creatures
	void OnTriggerStay(Collider other){
		if(other.tag=="Player"){
			PlayerCreatureHolder creatureHolder = other.GetComponent<PlayerCreatureHolder>();
			PlayerMovement movement = other.GetComponent<PlayerMovement>();
			if(movement!=null){
				movement.setOrigPosition();
			}
			for(int i = 0; i<creatureHolder.myCreatures.Length; i++){
				creatureHolder.myCreatures[i].healCreature();
			}
		}
	}
}
