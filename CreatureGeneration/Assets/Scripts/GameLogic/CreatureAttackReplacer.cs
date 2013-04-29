using UnityEngine;
using System.Collections;

public class CreatureAttackReplacer : MonoBehaviour {
	//Reference to player
	public PlayerMovement movement;
	//Background picture
	public Texture2D background;
	//The creature who has an extra attack
	private CreatureInfo creature;
	//Attack we are seeing if we need to replace
	private CreatureAttack attack;
	
	
	//Whether or not this script is active
	private bool active;
	
	
	
	void OnGUI(){
		if(active){
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), background);
			
			GUI.DrawTexture(new Rect(0,0,100,100), creature.image);
			GUI.TextArea( new Rect(Screen.width/2-120,0,240,25), "You leveled up and larged a new move!");
			GUI.TextArea( new Rect(Screen.width/2-120,25,240,25), "Replace what for "+attack.name + "?");
			
			if(GUI.Button(new Rect(0,100,100,50), ""+creature.one.name )){
				creature.one=attack;
				active=false;
				movement.inMvmtMode = true;
			}
			if(GUI.Button(new Rect(0,150,100,50), ""+creature.two.name )){
				creature.two=attack;
				active=false;
				movement.inMvmtMode = true;
			}
			if(GUI.Button(new Rect(0,200,100,50), ""+creature.three.name )){
				creature.three=attack;
				active=false;
				movement.inMvmtMode = true;
			}
			if(GUI.Button(new Rect(0,250,100,50), ""+creature.four.name )){
				creature.four=attack;
				active=false;
				movement.inMvmtMode = true;
			}
			
			if(GUI.Button(new Rect(Screen.width/2-50,Screen.height-50,100,50), "Nevermind")){
				active=false;
				movement.inMvmtMode = true;
			}
			
			
		}
	}
	
	public void activate(CreatureInfo creatureToUse){
		creature = creatureToUse;
		active =true;
		
		print("Levels: "+creatureToUse.level); 
		if(creatureToUse.level==2){
			attack = creature.getFromLevelTwo;
		}
		else if(creatureToUse.level==4){
			attack = creature.getFromLevelFour;
		}
		else if(creatureToUse.level==5){
			attack = creature.getFromLevelFive;
		}
		
		movement.inMvmtMode = false;
	}
	
	public void activate(CreatureInfo creatureToUse, CreatureAttack _attack){
		creature = creatureToUse;
		attack = _attack;
		active =true;
		
		movement.inMvmtMode = false;
	}
}
