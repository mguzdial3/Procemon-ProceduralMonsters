using UnityEngine;
using System.Collections;

//NO LONGER MAKING USE OF


public class CreatureShower : MonoBehaviour {
	public Texture2D[] creature;
	public int curr = 0;
	public CreatureInfo[] creatureInfos;
	//Wherther or not we're fighting presently
	bool fight;
	
	//Two different creatures 
	public int creatureMine, creatureAI;
	
	
	int state =0;
	const int DECIDE=0, FIRSTTURN=1, SECONDTURN=2; 
	
	int moveType = 0;
	const int ATTACK=0, DEFEND=1, SPECIAL=2;
	float moveTimer = 0;
	float moveTimerMAX = 4.5f;
	/**
	
	// Use this for initialization
	void Start () {
		
		creature = new Texture2D[60];
		creatureInfos = new CreatureInfo[60];
		
		
		
		
		string[] nameBits = {"un", "za", "muh", "ga","go","gi","da","do","di","du","fa","fo","fu","la","lo","lu","ma","mu","mi","naom","nurm",
		"nor","dude","bro","va","vo","vive","mon","bro","wes","gorn","florp","bag","dern","qui","ze","xon", "ley", "durn", "yui"};
		
		
		string[] fightBits = {"punch", "munch", "slam", "bam", "wham", "jam", "schism", "tackle", "strike", "pound", "slam", "jab", "bite", "slice",
			"blast", "kick", "charge", "chomp", "thrust", "blitz", "barrage", "onslaught", "foray", "skirmish", "encroach"};
		
		//public const int LIGHTNING= 0, GROUND =1, WATER =2, FIRE = 3, GRASS =4, AIR=5, NORMAL=6;
		string[] lightningBits = {"lightning", "thunder", "electric", "volt", "magnetic"};
		string[] groundBits = {"ground", "rock", "earth", "dirt", "mud"};
		string[] waterBits = {"water", "aqua", "rain", "tsunami", "shower"};
		string[] fireBits = {"fire", "flame", "fiery", "magma", "burn"};
		string[] grassBits = {"grass", "leaf", "vine", "thorn", "branch"};
		string[] airBits = {"air", "wind", "storm", "sky", "zephyr"};
		string[] normalBits = {"strong", "cunning", "killer", "death", "average"};
		
		
		string[][] elementalBits = {lightningBits, groundBits, waterBits, fireBits, grassBits, airBits, normalBits};
		
		CreatureGenerationScript c = new CreatureGenerationScript();
		
		for(int i = 0; i<creature.Length; i++){
			
			
			int nameLength= Random.Range(2,4);
			int nameIndex = 0;
			
			
			string creatureName = "";
			while(nameIndex<nameLength){
				creatureName += nameBits[Random.Range(0, nameBits.Length)];
				nameIndex++;
			}
			
			//Basic attack
			int fightIndex=0;
			int fightLength = Random.Range(1,3);
			
			string basicAttack = "";
			
			while(fightIndex<fightLength){
				
				basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
				
				fightIndex++;
			}
			
			
			
			
			//creatureInfos[i] = new CreatureInfo(char.ToUpper(creatureName[0]) + creatureName.Substring(1));
			
			
			
			//creatureInfos[i].setBasic(char.ToUpper(basicAttack[0]) + basicAttack.Substring(1));
			
			
			//Special
			string specialAttack = elementalBits[creatureInfos[i].type][Random.Range(0, elementalBits[creatureInfos[i].type].Length)]
			+ "-"+fightBits[Random.Range(0, fightBits.Length)];
			
			//creatureInfos[i].setSpecial(char.ToUpper(specialAttack[0]) + specialAttack.Substring(1));
			
			creature[i]=c.MakeACreature(creatureInfos[i].type);
			
		}
		
		
		creatureAI = Random.Range(0,creature.Length);
		while(creatureAI==creatureMine){
			creatureMine=Random.Range(0,creature.Length);
		}
		
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.D)){
			if(curr<creature.Length-1){
				curr++;
			}
			else{
				curr=0;
			}
		}
		else if(Input.GetKeyDown(KeyCode.A)){
			if(curr>1){
				curr--;
			}
			else{
				curr=creature.Length-1;
			}
		}
	}
	
	void OnGUI(){
		string _fight = "Endless Fight?";
		
		if(fight){
			_fight = "New Creature?";
		}
		
		
		
		if(GUI.Button(new Rect(Screen.width-100, 0, 100,40), _fight)){
			if(!fight){
				creatureMine=curr;
				creatureInfos[creatureMine].hitPoints=100;
			}
			
			fight=!fight;
		}
		
		
		if(!fight){
			
			GUI.TextArea(new Rect(Screen.width/2-75, Screen.height/4+50, 150, 30), "Use "+creatureInfos[curr].name+"?");
			
			GUI.Box( new Rect(Screen.width/2-50, Screen.height/2-50, 100,100), creature[curr]);
			GUI.TextArea( new Rect(0, 0, 30,30), ""+curr);
			
			//GUI.TextArea(new Rect(Screen.width/2-50, Screen.height/2+50, 100,150), "Name: "+creatureInfos[curr].name+"\nAttack: "+creatureInfos[curr].attack+"\nArmor: "+creatureInfos[curr].armor +"\nSpeed: "+creatureInfos[curr].speed +"\nSpecial: "+creatureInfos[curr].special +"\nType: "+creatureInfos[curr].getType()); 
		
			if(GUI.Button( new Rect(Screen.width/4-30, Screen.height/3,60,30), "Prev")){
				if(curr>1){
					curr--;
				}
				else{
					curr=creature.Length-1;
				}
			}
			if(GUI.Button( new Rect(Screen.width*3/4-30, Screen.height/3,60,30), "Next")){
				if(curr<creature.Length-1){
					curr++;
				}
				else{
					curr=0;
				}
			}
		
		
		}
		else{
			//Creature One
			GUI.Box( new Rect(0, Screen.height/2-50, 100,100), creature[creatureMine]);
			
			//Hit points
			GUI.TextArea( new Rect(0, Screen.height/2-80, 100,30), "Hitpoints: "+creatureInfos[creatureMine].hitPoints);
			
			//Name
			GUI.TextArea( new Rect(0, Screen.height/2-115, 100,35), "Name: "+creatureInfos[creatureMine].name);
			
			
			//Creature Two
			GUI.Box( new Rect(Screen.width-100, Screen.height/2-50, 100,100), creature[creatureAI]);
			GUI.TextArea( new Rect(Screen.width-100, Screen.height/2-80, 100,30), "Hitpoints: "+creatureInfos[creatureAI].hitPoints);
			
			//Name
			GUI.TextArea( new Rect(Screen.width-100, Screen.height/2-115, 100,35), "Name: "+creatureInfos[creatureAI].name);
			/**
			if(state==DECIDE){
				//ATTACK
				if(GUI.Button( new Rect(0, Screen.height/2+100, 100, 30), "ATTACK")){
					//creatureInfos[creatureMine].currMove=ATTACK;
					state++;
					moveTimer=moveTimerMAX;
					creatureInfos[creatureAI].currMove= Random.Range(0,3);
				}
				//DEFEND
				if(GUI.Button( new Rect(0, Screen.height/2+130, 100, 30), "DEFEND")){
					creatureInfos[creatureMine].currMove=DEFEND;
					state++;
					moveTimer=moveTimerMAX;
					creatureInfos[creatureAI].currMove= Random.Range(0,3);
				}
				//SPECIAL
				if(GUI.Button( new Rect(0, Screen.height/2+160, 100, 30), "SPECIAL")){
					creatureInfos[creatureMine].currMove=SPECIAL;
					state++;
					moveTimer=moveTimerMAX;
					
					creatureInfos[creatureAI].currMove= Random.Range(0,3);
				}
			
				
				
			}
			else if(state==FIRSTTURN){
				//We're going first
				if(creatureInfos[creatureMine].speed>creatureInfos[creatureAI].speed){
					
					string whatWeDid = "";
					
					if(creatureInfos[creatureMine].currMove==ATTACK){
						whatWeDid ="Your " + creatureInfos[creatureMine].name +" used "+ creatureInfos[creatureMine].basicAttack + "!";
					}
					else if(creatureInfos[creatureMine].currMove==DEFEND){
						whatWeDid ="Your " + creatureInfos[creatureMine].name +" defended!";
					}
					else if(creatureInfos[creatureMine].currMove==SPECIAL){
						whatWeDid ="Your " + creatureInfos[creatureMine].name +" used "+ creatureInfos[creatureMine].specialAttack + "!";
					}
					
					
					GUI.TextArea( new Rect(Screen.width/2-300, Screen.height*(3/4), 300, 30), whatWeDid);
				
					if(moveTimer>0){
						moveTimer-=Time.deltaTime;
					}
					else{
						
						doDamage(creatureMine, creatureAI);
						
						if(creatureInfos[creatureAI].hitPoints<0){
							int newCreatureAI = 0;
							
							while(newCreatureAI==creatureAI || newCreatureAI==creatureMine || creatureInfos[newCreatureAI].hitPoints<0){
								newCreatureAI = Random.Range(0, creatureInfos.Length);
							}
							
							creatureAI=newCreatureAI;
							
							
						}
						
						moveTimer=moveTimerMAX;
						state++;
					}
				}
				else{//AI GOES first
					
					string whatWeDid = "";
					
					if(creatureInfos[creatureAI].currMove==ATTACK){
						whatWeDid ="" + creatureInfos[creatureAI].name +" used "+ creatureInfos[creatureAI].basicAttack + "!";
					}
					else if(creatureInfos[creatureAI].currMove==DEFEND){
						whatWeDid ="" + creatureInfos[creatureAI].name +" defended!";
					}
					else if(creatureInfos[creatureAI].currMove==SPECIAL){
						whatWeDid ="" + creatureInfos[creatureAI].name +" used "+ creatureInfos[creatureAI].specialAttack + "!";
					}
					
					print("Creature AI currMove: "+creatureInfos[creatureAI].currMove);
					
					
					GUI.TextArea( new Rect(Screen.width/2-300, Screen.height*(3/4), 300, 30), whatWeDid);
					
					
					if(moveTimer>0){
						moveTimer-=Time.deltaTime;
					}
					else{
						doDamage(creatureAI, creatureMine);
						
						
						if(creatureInfos[creatureMine].hitPoints<0){
							int newCreatureMine = 0;
							
							while(newCreatureMine==creatureMine || newCreatureMine==creatureAI || creatureInfos[newCreatureMine].hitPoints<0){
								newCreatureMine = Random.Range(0, creatureInfos.Length);
							}
							
							creatureMine=newCreatureMine;
						}
						
						
						
						moveTimer=moveTimerMAX;
						state++;
					}
				}
				
				
			}
			else if(state==SECONDTURN){
				//We're going second
				if(creatureInfos[creatureMine].speed<=creatureInfos[creatureAI].speed && creatureInfos[creatureMine].currMove!=-1){
					
					string whatWeDid = "";
					
					if(creatureInfos[creatureMine].currMove==ATTACK){
						whatWeDid ="Your " + creatureInfos[creatureMine].name +" used "+ creatureInfos[creatureMine].basicAttack + "!";
					}
					else if(creatureInfos[creatureMine].currMove==DEFEND){
						whatWeDid ="Your " + creatureInfos[creatureMine].name +" defended!";
					}
					else if(creatureInfos[creatureMine].currMove==SPECIAL){
						whatWeDid ="Your " + creatureInfos[creatureMine].name +" used "+ creatureInfos[creatureMine].specialAttack + "!";
					}
					
					
					GUI.TextArea( new Rect(Screen.width/2-300, Screen.height*(3/4), 300, 30), whatWeDid);
				
					if(moveTimer>0){
						moveTimer-=Time.deltaTime;
					}
					else{
						
						doDamage(creatureMine, creatureAI);
						
						if(creatureInfos[creatureAI].hitPoints<0){
							int newCreatureAI = 0;
							
							while(newCreatureAI==creatureAI || newCreatureAI==creatureMine|| creatureInfos[newCreatureAI].hitPoints<0){
								newCreatureAI = Random.Range(0, creatureInfos.Length);
							}
							
							creatureAI=newCreatureAI;
							
							
						}
						
						moveTimer=moveTimerMAX;
						state=0;
					}
				} 
				else if(creatureInfos[creatureMine].speed>creatureInfos[creatureAI].speed && creatureInfos[creatureAI].currMove!=-1){//AI GOES second
					
					string whatWeDid = "";
					
					if(creatureInfos[creatureAI].currMove==ATTACK){
						whatWeDid ="" + creatureInfos[creatureAI].name +" used "+ creatureInfos[creatureAI].basicAttack + "!";
					}
					else if(creatureInfos[creatureAI].currMove==DEFEND){
						whatWeDid ="" + creatureInfos[creatureAI].name +" defended!";
					}
					else if(creatureInfos[creatureAI].currMove==SPECIAL){
						whatWeDid ="" + creatureInfos[creatureAI].name +" used "+ creatureInfos[creatureAI].specialAttack + "!";
					}
					
					print("Creature AI currMove: "+creatureInfos[creatureAI].currMove);
					
					GUI.TextArea( new Rect(Screen.width/2-300, Screen.height*(3/4), 300, 30), whatWeDid);
					
					
					if(moveTimer>0){
						moveTimer-=Time.deltaTime;
					}
					else{
						
						doDamage(creatureAI, creatureMine);
						
						if(creatureInfos[creatureMine].hitPoints<0){
							int newCreatureMine = 0;
							
							while(newCreatureMine==creatureMine || newCreatureMine==creatureAI || creatureInfos[newCreatureMine].hitPoints<0){
								newCreatureMine = Random.Range(0, creatureInfos.Length);
							}
							
							creatureMine=newCreatureMine;
						}
						
						
						
						
						moveTimer=moveTimerMAX;
						state=0;
					}
				}
				else{
					state=0;
				}
			}
			
			
		}
		
	}
	
	
	public void doDamage(int attacker, int target){
		/**
		if(creatureInfos[attacker].currMove==ATTACK){
			creatureInfos[target].hitPoints-= (int)(creatureInfos[attacker].attack*10*getDefendingModifier(target));
			
		}
		else if(creatureInfos[attacker].currMove==SPECIAL){
			creatureInfos[target].hitPoints-=(int)(creatureInfos[attacker].attack*10*getDefendingModifier(target)*attackModifier(creatureInfos[attacker].type, creatureInfos[target].type));
		}
		
		
		
	}
	
	
	public float getDefendingModifier(int defendingCreature){
		if(creatureInfos[defendingCreature].currMove==DEFEND){
			return 0.1f;
		}
		return 1.0f;
	}
	
	
	//Returns modifier on attack given that it was a special type
	public float attackModifier(int attackerType, int defenderType){
		
		int LIGHTNING= 0, GROUND =1, WATER =2, FIRE = 3, GRASS =4, AIR=5, NORMAL=6;
		
		if(attackerType==LIGHTNING){
			if(defenderType==GROUND){
				return 0.5f;
			}
			else if(defenderType==WATER){
				return 2.0f;
			}
			
		}
		else if(attackerType==GROUND){
			if(defenderType==WATER){
				return 0.5f;
			}
			else if(defenderType==LIGHTNING){
				return 2.0f;
			}
		}
		else if(attackerType==WATER){
			if(defenderType==LIGHTNING){
				return 0.5f;
			}
			else if(defenderType==GROUND){
				return 2.0f;
			}
		}
		
		if(attackerType==FIRE){
			if(defenderType==AIR){
				return 0.5f;
			}
			else if(defenderType==GRASS){
				return 2.0f;
			}
			
		}
		else if(attackerType==AIR){
			if(defenderType==GRASS){
				return 0.5f;
			}
			else if(defenderType==FIRE){
				return 2.0f;
			}
		}
		else if(attackerType==GRASS){
			if(defenderType==FIRE){
				return 0.5f;
			}
			else if(defenderType==AIR){
				return 2.0f;
			}
		}
		
		return 1.0f;
	}
	
		*/
}
