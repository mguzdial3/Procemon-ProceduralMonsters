using UnityEngine;
using System.Collections;

//Detects when the player should enter a creature battle, and then runs through the battle
public class CreatureBattleHandler : MonoBehaviour {
	public PlayerMovement player;
	public PlayerCreatureHolder playerCreatureHolder;
	//Attack Replacement Handler
	public CreatureAttackReplacer attackReplacement;
	
	//Whether or not we're battling presently
	private bool battling;
	//Time that you've spent in a place where creatures can get you
	private int timeInGrass;
	public Texture2D creatureBattleBackground;
	CreatureHolder creatureHolder;
	
	CreatureInfo myCreature, attackingCreature;
	
	private CreatureAttack myAttack, itsAttack;
	
	//Whether or not my creature is going first (and which attack we're on)
	private bool iGoFirst, onFirstAttack, attackCalculated;
	
	//Timer for determining how long readouts show up
	private float readOutTimer = 0.0f;
	//Time for read outs
	public float readOutMax = 2.0f;
	//Text to display
	private string displayText;
	
	
	
	
	//The states we are in
	private int guiState=0;
	//The states we're currently in
	private const int ACTION_DECIDE=0, ATTACK_DECIDE=1, READOUT=2, SWITCH_CREATURE = 3, REPLACE_CREATURE=4,  FINAL_READOUT=5;
	
	
	//Modifiers for both creatures in the battle
	private int myAttackModifier, myDefenseModifier,  mySpeedModifier, mySpecialModifier;
	private float myAccuracyModifier;
	private int itsAttackModifier, itsDefenseModifier, itsSpeedModifier, itsSpecialModifier;
	private float itsAccuracyModifier;

	
	
	
	void Start(){
		creatureHolder = gameObject.GetComponent<CreatureHolder>();
		playerCreatureHolder = player.GetComponent<PlayerCreatureHolder>();
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		//Find out what player is over when we're in regular movement mode
		if(!battling && player.inMvmtMode){
			RaycastHit hit;
		
			if(Physics.Raycast(player.transform.position, Vector3.forward, out hit, 10.0f)){
				if(hit.collider.tag=="ContainsCreatures"){
					int chanceOfBeingAttacked = Random.Range(0,300-timeInGrass);
					timeInGrass++;
					if(chanceOfBeingAttacked ==0 && timeInGrass>10){
						timeInGrass=0;
						
						
						
						battling= true;
						player.inMvmtMode=false;
						attackingCreature = creatureHolder.allCreatures[Random.Range(0,creatureHolder.allCreatures.Length)].cloneCreature();
					
						//Grab the first creature in the array
						myCreature = playerCreatureHolder.myCreatures[0];
					
					}
				}
				else{
					timeInGrass=0;
				}
			}
		}
	}
	
	
	void OnGUI(){
		
		
		if(battling){
			
			
			
			
			//Background
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), creatureBattleBackground);
			
			//The Creatures
			GUI.DrawTexture(new Rect(0,0,100,100), myCreature.image);
			//My info:
			GUI.TextField(new Rect(0,100,100,30), "Name: "+myCreature.name);
			GUI.TextField(new Rect(0,130,100,30), "HP: "+myCreature.hitPoints);
			
			GUI.DrawTexture(new Rect(Screen.width-100,0,100,100), attackingCreature.image);
			//Enemy info: 
			GUI.TextField(new Rect(Screen.width-100,100,100,30), "Name: "+attackingCreature.name);
			GUI.TextField(new Rect(Screen.width-100,130,100,30), "HP: "+attackingCreature.hitPoints);
			
			if(guiState == ACTION_DECIDE){//DETERMINE THE USER'S ACTION
				
				if(GUI.Button(new Rect(0, 200, 100,50), "Attack")){
					//Switch Procemon
					guiState = ATTACK_DECIDE;
				}
				if(GUI.Button(new Rect(0, 250, 100,50), "Catch")){
					//Attempt to Catch Procemon
					float chanceToCatch = ((float)(attackingCreature.maxHitPoints-attackingCreature.hitPoints)/(float)attackingCreature.maxHitPoints);
					
					bool didWeCatchIt = Random.Range(chanceToCatch,1.0f)>0.95f;
					
					
					
					if(didWeCatchIt){
						displayText = "You caught a "+attackingCreature.name+"!";
						//Heal and snag the attacking creature
						
						if(playerCreatureHolder.myCreatures.Length<4){//If we have room for the creature
							attackingCreature.hitPoints = attackingCreature.maxHitPoints;
							playerCreatureHolder.addCreature(attackingCreature);
							guiState = FINAL_READOUT;
							
							//Actually switching for it
							CreatureMetrics.alterScore(attackingCreature.ID,CreatureMetrics.INCREMENT_SWITCH);
						}
						else{//Figure out what creature we're replacing
							print("I should be heading to replace creature");
							guiState= REPLACE_CREATURE;
						}
						
						
						
					}
					else{
						//You failed to get him, now he'll just attack you
						promptOtherToAttack();
					}
					
					
					//YOU TRIED TO CATCH A CREATURE, THAT MEANS YOU MUST LIKE IT
					CreatureMetrics.alterScore(attackingCreature.ID,CreatureMetrics.ATTEMPT_TO_CATCH);
					
					
					
				}
				if(GUI.Button(new Rect(0, 300, 100,50), "Switch")){
					guiState =SWITCH_CREATURE;
				}
				if(GUI.Button(new Rect(0, 350, 100,50), "Flee")){
					//Easy way to cheat back to reality (for now)
					returnToGameplay();
				}
			}
			else if(guiState == ATTACK_DECIDE){//DETERMINE THE CREATURE'S ATTACK
				
				bool attackChosen = false;
				
				if(GUI.Button(new Rect(0, 200, 200,50),myCreature.one.name )){
					myAttack = myCreature.one;
					attackChosen = true;
				}
				if(GUI.Button(new Rect(0, 250, 200,50),myCreature.two.name )){
					myAttack=myCreature.two;
					attackChosen = true;
				}
				if(GUI.Button(new Rect(0, 300, 200,50),myCreature.three.name )){
					myAttack=myCreature.three;
					attackChosen = true;
				}
				if(GUI.Button(new Rect(0, 350, 200,50),myCreature.four.name )){
					myAttack = myCreature.four;
					attackChosen = true;
				}
			
				if(attackChosen){
					//Pick other dude's attacks
					itsAttack = attackingCreature.getAttack();
					
					//Figure out who goes first
					iGoFirst = (myCreature.speed+mySpeedModifier)>=attackingCreature.speed+(itsSpeedModifier);
					
					//Switch screens
					guiState=READOUT;
					onFirstAttack = true;
					attackCalculated=false;
				}
				
			
				if(GUI.Button(new Rect(Screen.width/2-50, Screen.height-100, 100,50),"Back" )){
					//Gives users the option to go back
					guiState=ACTION_DECIDE;
				}
			
			}
			else if(guiState==READOUT){
				if(onFirstAttack){
					if(iGoFirst && !attackCalculated){//Determine the results of my attack
						calculateAttackEffect(attackingCreature,myCreature,myAttack,true);

						attackCalculated=true;
					}
					else if(!iGoFirst && !attackCalculated){//Determine the results of its attack
						calculateAttackEffect(myCreature,attackingCreature,itsAttack,false);
						attackCalculated=true;
					}
					else{//Display till we're out of timer
						GUI.TextArea(new Rect(Screen.width/2-150,0, 300,60), displayText);
						
						if(readOutTimer<readOutMax){
							readOutTimer+=Time.deltaTime;
						}
						else{
							//We're done with displaying, onto the next one
							if(onFirstAttack){//On the first attack, just go to the second one
								onFirstAttack=false;
								readOutTimer=0.0f;
								attackCalculated=false;
							}
							else{//On the second attack, move to deciding what next to do
								readOutTimer=0.0f;
								guiState=ACTION_DECIDE;
							}
						}
						
					}
				}
				else{
					
					if(!iGoFirst && !attackCalculated){//Determine the results of my attack
						
						calculateAttackEffect(attackingCreature,myCreature,myAttack,true);
						attackCalculated=true;
					}
					else if(iGoFirst && !attackCalculated){//Determine the results of its attack
						attackCalculated=true;
						calculateAttackEffect(myCreature,attackingCreature,itsAttack,false);
					}
					else{//Display till we're out of timer
						
						
						if(myCreature.hitPoints<=0 && attackingCreature.hitPoints>0){
							//See if we have another creature
							
							guiState = SWITCH_CREATURE;
							
						}
						else if(attackingCreature.hitPoints<=0){
							//Calculate experience
							float experienceGain = attackingCreature.level*20;
							
							//See if we level up (if we do, transfer to level up handler)
							//TODO; Extend this to encompass other creatures leveling up
							bool leveledUp = myCreature.levelUp(experienceGain);
							
							
							//Display that we won
							if(leveledUp){
								if(myCreature.getAttackAt[myCreature.level-1]){
									returnToGameplay();
									attackReplacement.activate(myCreature);
								}
								else{
									displayText =""+myCreature.name+" leveled up!";
									guiState = FINAL_READOUT;
								}
								
							}
							else{
								displayText = "You win!";
								readOutTimer=0.0f;
							
								guiState = FINAL_READOUT;
							}
							
							
							
						}
						else{
							GUI.TextArea(new Rect(Screen.width/2-150,0, 300,60), displayText);
							
							if(readOutTimer<readOutMax){
								readOutTimer+=Time.deltaTime;
							}
							else{
								//We're done with displaying, onto the next one
								if(onFirstAttack){//On the first attack, just go to the second one
									onFirstAttack=false;
									readOutTimer=0.0f;
									attackCalculated=false;
								}
								else{//On the second attack, move to deciding what next to do
									readOutTimer=0.0f;
									guiState=ACTION_DECIDE;
								}
							}
						}
					}
				}
			}
			else if(guiState==REPLACE_CREATURE){
				//BACKGROUND
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), creatureBattleBackground);
				
				//Instructions
				GUI.TextArea(new Rect(Screen.width/2-150, 0,300,25), "Which Creature to replace for "+attackingCreature.name+"?");
				
				CreatureInfo[] creatures = playerCreatureHolder.myCreatures;
				
				for(int i =0; i<creatures.Length; i++){//Go through creatures of player and show each one
						if(creatures[i]!=null){
							GUI.DrawTexture(new Rect(10+i*(Screen.width/6),Screen.height/4, Screen.width/8, Screen.width/8),creatures[i].image);
							GUI.TextArea(new Rect(10+i*(Screen.width/6),(Screen.height/4)+(Screen.width/8), Screen.width/8,Screen.width/4), 
							"Name: "+creatures[i].name+"\nAttack: "+creatures[i].attack+"\nArmor: "+creatures[i].defense +"\nSpeed: "+creatures[i].speed +"\nSpecial: "+creatures[i].special +"\nType: "+creatures[i].getType()); 
								
							//Actually switching for it
							CreatureMetrics.alterScore(attackingCreature.ID,CreatureMetrics.INCREMENT_SWITCH);
							CreatureMetrics.alterScore(creatures[i].ID,CreatureMetrics.DECREMENT_SWITCH);
						
							if(GUI.Button(new Rect(10+i*(Screen.width/6), (Screen.height/4)-30, Screen.width/8, 30), "Replace")){
								
							//Heal the creature FOR NOW and replace
							attackingCreature.healCreature();
							playerCreatureHolder.setCreature(attackingCreature,i);
							
							
							//Head back to reality
							returnToGameplay();
						}
					}
				}
				
				
				if(attackingCreature!=null && GUI.Button(new Rect(Screen.width-200, 0, 200, 25), "Nevermind, Get rid of "+attackingCreature.name)){
					//Just switch back to reality
					returnToGameplay();
				}
				
			}
			else if(guiState == SWITCH_CREATURE){
				//Switch Procemon
					
				//Background
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), creatureBattleBackground);
					
				CreatureInfo[] creatures = playerCreatureHolder.myCreatures;
					
				int numAboveZero = 0;
				
				for(int i = 0; i<creatures.Length; i++){
					if(creatures[i].hitPoints>0){
						numAboveZero++;
					}
				}
				
				if(numAboveZero!=0){//If there's someone alive we can switch to
					for(int i =0; i<creatures.Length; i++){//Go through creatures of player and show each one
						if(creatures[i]!=null){
							GUI.DrawTexture(new Rect(10+i*(Screen.width/6),Screen.height/4, Screen.width/8, Screen.width/8),creatures[i].image);
							GUI.TextArea(new Rect(10+i*(Screen.width/6),(Screen.height/4)+(Screen.width/8), Screen.width/8,Screen.width/4), 
							"Name: "+creatures[i].name+"\nAttack: "+creatures[i].attack+"\nArmor: "+creatures[i].defense +"\nSpeed: "+creatures[i].speed +"\nSpecial: "+creatures[i].special +"\nType: "+creatures[i].getType()); 
								
							
							if(GUI.Button(new Rect(10+i*(Screen.width/6), (Screen.height/4)-30, Screen.width/8, 30), "Switch")){
								//print("Switch Pressed");
								resetPlayerModifiers();
								//If we didn't select the same creature and that creature isn't dead
								if(creatures[i].hitPoints>0){
									myCreature = creatures[i];
									
									//And the other creature gets to attack
									promptOtherToAttack();
									
									
								}
							}
						}
					}
				}
				else{
					//Reload level for now. Later well need to go to the last heal place.
					Application.LoadLevel(Application.loadedLevelName);
					
				}
				
				if(myCreature.hitPoints>0){
					if(GUI.Button(new Rect(Screen.width/2-50,0,100,30), "Back")){
						guiState = ACTION_DECIDE;
					}
				}
			}
			else if(guiState==FINAL_READOUT){
				GUI.TextArea(new Rect(Screen.width/2-150,0, 300,60), displayText);
							
				if(readOutTimer<readOutMax){
					readOutTimer+=Time.deltaTime;
				}
				else{
					//We're done with displaying, back to regular scene
					returnToGameplay();
				}
			}
			
			
			
		}
		
		
		
	}
	
	
	//Returns to regular/world gameplay
	private void returnToGameplay(){
		player.inMvmtMode=true;
		guiState=ACTION_DECIDE;
		attackingCreature=null;
		battling=false;
		
		resetPlayerModifiers();
		resetAttackerModifiers();
	}
	
	//Reset player's modifiers
	private void resetPlayerModifiers(){
		myAttackModifier = 0;
		myDefenseModifier = 0;
		mySpeedModifier = 0;
		mySpecialModifier = 0;
		myAccuracyModifier = 0;
	}
	
	//Reset attacker's modifiers
	private void resetAttackerModifiers(){
		itsAttackModifier = 0;
		itsDefenseModifier = 0;
		itsSpeedModifier = 0;
		itsSpecialModifier = 0;
		itsAccuracyModifier = 0;
	}
	
	//Prompts the other creature to attack
	private void promptOtherToAttack(){
		guiState = READOUT;
		onFirstAttack=false;
		iGoFirst=true;
		readOutTimer=0.0f;
		itsAttack = attackingCreature.getAttack();
		attackCalculated=false;
	}
	
	//Is it me doing the attacking
	private void calculateAttackEffect(CreatureInfo target,CreatureInfo attacker, CreatureAttack attack, bool isMe){
		//You used an attack, better have it do something
		attack.timesUsed++;
		
		//If damage type
		if(attack.attackType==0){
			//First let's figure out if we missed
			float valToBeBelow = Random.value;
			
			
			if(isMe){
				//You hit!
				if(valToBeBelow>1.0-(attack.accuracyOfAttack+itsAccuracyModifier)){
					target.hitPoints -= (attack.power+(myAttackModifier+attacker.attack))/(target.defense+itsDefenseModifier);
					displayText=(attacker.name +" used "+attack.name+"! It hit!");
				}
				else{
					//You missed
					displayText=(attacker.name +" used "+attack.name+"! But they missed!");
				}
			}
			else{
				//You hit!
				if(valToBeBelow>1.0-(attack.accuracyOfAttack+itsAccuracyModifier)){
					
					if(target.defense-myDefenseModifier!=0){
						if((target.defense+myDefenseModifier)!=0){
							target.hitPoints -= (attack.power+(itsAttackModifier+attacker.attack))/(target.defense+myDefenseModifier);
						}
						else{
							target.hitPoints -= (attack.power+(itsAttackModifier+attacker.attack));
						}
					}
					else{
						target.hitPoints -= (attack.power+(itsAttackModifier+attacker.attack));
					}
					displayText=(attacker.name +" used "+attack.name+"! It hit!");
				}
				else{
					//You missed
					displayText=(attacker.name +" used "+attack.name+"! But they missed!");
				}
			}
			
			
		}
		else if(attack.attackType==1){//Boosts self type
			
			//ATTACK=1, DEFENSE=2, SPEED= 3, SPECIAL =4, ACCURACY = 5;
			if(attack.statTarget==1){//Boost attack
				if(isMe){
					myAttackModifier+=attack.power;
				}
				else{
					itsAttackModifier+=attack.power;
				}
				
				displayText=(attacker.name +" used "+attack.name+"! "+attacker.name+"'s attack increased!");
			}
			else if(attack.statTarget==2){//Boost defense
				if(isMe){
					myDefenseModifier+=attack.power;
				}
				else{
					itsDefenseModifier+=attack.power;
				}
				displayText=(attacker.name +" used "+attack.name+"! "+attacker.name+"'s defense increased!");
			}
			else if(attack.statTarget==3){//Boost speed
				if(isMe){
					mySpeedModifier+=attack.power;
				}
				else{
					itsSpeedModifier+=attack.power;
				}
				displayText=(attacker.name +" used "+attack.name+"! "+attacker.name+"'s speed increased!");
			}
			else if(attack.statTarget==4){//Boost special
				if(isMe){
					mySpecialModifier+=attack.power;
				}
				else{
					itsSpecialModifier+=attack.power;
				}
				displayText=(attacker.name +" used "+attack.name+"! "+attacker.name+"'s special increased!");
			}
			else if(attack.statTarget==5){//Boost accuracy
				if(isMe){
					myAccuracyModifier+=attack.power;
				}
				else{
					itsAccuracyModifier+=attack.power;
				}
				
				displayText=(attacker.name +" used "+attack.name+"! "+attacker.name+"'s accuracy increased!");
			}
			
		}
		else if(attack.attackType==2){//Hurts targets modifiers
			//ATTACK=1, DEFENSE=2, SPEED= 3, SPECIAL =4, ACCURACY = 5;
			if(attack.statTarget==1){//Hurt attack
				if(isMe){
					if(itsAttackModifier+target.attack-attack.power>0){
						itsAttackModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s attack decreased!");
					}
					else{
						itsAttackModifier = -1*target.attack;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s attack could not be lowered!");
					}
					
				}
				else{
					if(myAttackModifier+target.attack-attack.power>0){
						myAttackModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s attack decreased!");
					}
					else{
						myAttackModifier = -1*target.attack;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s attack could not be lowered!");
					}
				}
			}
			else if(attack.statTarget==2){//Hurt defense
				if(isMe){
					if(itsDefenseModifier+target.defense-attack.power>0){
						itsDefenseModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s defense decreased!");
					}
					else{
						itsDefenseModifier=-1*target.defense;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s defense could not be lowered!");
					}
					
				}
				else{
					if(myDefenseModifier+target.defense-attack.power>0){
						myDefenseModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s defense decreased!");
					}
					else{
						myDefenseModifier=-1*target.defense;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s defense could not be lowered!");
					}
				}
			}
			else if(attack.statTarget==3){//Hurt speed
				if(isMe){
					if(itsSpeedModifier+target.speed-attack.power>0){
						itsSpeedModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s speed decreased!");
					}
					else{
						itsSpeedModifier=-1*target.speed;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s speed could not be lowered!");
					}
					
				}
				else{
					if(mySpeedModifier+target.speed-attack.power>0){
						mySpeedModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s speed decreased!");
					}
					else{
						mySpeedModifier=-1*target.speed;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s speed could not be lowered!");
					}
				}
			}
			else if(attack.statTarget==4){//Hurt special
				if(isMe){
					if(itsSpecialModifier+target.special-attack.power>0){
						itsSpecialModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s special decreased!");
					}
					else{
						itsSpecialModifier=-1*target.special;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s special could not be lowered!");
					}
					
				}
				else{
					if(mySpecialModifier+target.special-attack.power>0){
						mySpecialModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s special decreased!");
					}
					else{
						mySpecialModifier=-1*target.special;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s special could not be lowered!");
					}
				}
			}
			else if(attack.statTarget==5){//Hurt accuracy
				if(isMe){
					if(itsAccuracyModifier+target.percentAccuracy-attack.power>0){
						itsAccuracyModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s accuracy decreased!");
					}
					else{
						itsAccuracyModifier=-1*target.percentAccuracy;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s accuracy could not be lowered!");
					}
					
				}
				else{
					if(myAccuracyModifier+target.percentAccuracy-attack.power>0){
						myAccuracyModifier-=attack.power;
						displayText=(attacker.name +" used "+attack.name+"! "+target.name+"'s accuracy decreased!");
					}
					else{
						myAccuracyModifier=-1*target.percentAccuracy;
						displayText=(attacker.name +" used "+attack.name+"! But "+target.name+"'s accuracy could not be lowered!");
					}
				}
			}
			
		}
	}
	
}
