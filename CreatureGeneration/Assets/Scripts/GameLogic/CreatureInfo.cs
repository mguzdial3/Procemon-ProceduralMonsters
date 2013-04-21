using UnityEngine;
using System.Collections;

public class CreatureInfo {
	//The ID# for this creature, unique for each creature in a generation
	public int ID;
	
	//Name of creature
	public string name;
	//Base attack power of creature
	public int attack;
	//Speed of this creature (does it attack first or second)
	public int speed;
	//Defense of this creature
	public int defense;
	//Special stat
	public int special;
	//Current Hitpoints
	public int hitPoints=0;
	public int maxHitPoints = 100;
	
	//Likelihood that a hit will land
	public float percentAccuracy;
	
	//The elemental type of this creature
	public int type;
	//Yellow, Brown/Green, Blue, Red/Yellow, WHITE, GRAY
	public const int NORMAL = 0, FIRELIGHTNING =1, WATER = 2, GRASSGROUND = 3, AIR =4;
	
	//Image for this creature, to be displayed during battle
	public Texture2D image;
	public CreatureImageData data;
	
	//Level is 1 to start out with
	public int level = 1;
	
	public CreatureAttack one, two, three, four;
	
	public CreatureAttack getFromLevelTwo, getFromLevelFour,getFromLevelFive;
	
	//If true, then we get an attack at that level
	public bool[] getAttackAt = {false, true, false, true, true};
	
	//Always start experience at 0
	public float experience = 0;
	
	
	//Randomly Generates a CreatureInfo, generating everything but name and image
	public CreatureInfo(string _name, Texture2D _image){
		name= _name;
		
		
		//All values sum to 10
		attack = Random.Range(1,8);
		speed = Random.Range(1, 9-attack);
		defense = Random.Range(1, 10-attack-speed);
		special = Random.Range(1, 11-attack-defense-speed);
		//Type of creature determined randomly
		type = Random.Range(0,6);
		
		//Randomly generate accuracy
		percentAccuracy = Random.Range(0.8f, 1.0f);
		
		//Hitpoints set up to 100 at first
		hitPoints = 100;
		
		image=_image;
		
	}
	
	//Constructor for creature based on passed in info
	public CreatureInfo(int _ID, string _name, int _attack, int _speed, int _defense, int _special, int _type, float _percentAccuracy, int _maxHitPoints,  Texture2D _image, CreatureImageData _data){
		ID = _ID;
		
		name = _name;
		attack = _attack;
		speed=_speed;
		special=_special;
		defense=_defense;
		type=_type;
		percentAccuracy=_percentAccuracy;
		maxHitPoints =_maxHitPoints;
		hitPoints=maxHitPoints;
		
		image=_image;
		
		data =_data;
		
	}
	
	
	public void setAttacks(CreatureAttack[] attacks, CreatureAttack[] levelUpAttacks){
		one = attacks[0];
		two = attacks[1];
		three = attacks[2];
		four = attacks[3];
		
		getFromLevelTwo = levelUpAttacks[0];
		getFromLevelFour = levelUpAttacks[1];
		getFromLevelFive = levelUpAttacks[2];
	}
	
	//Get's type of this creature as a string
	public string getType(){
		switch(type){
			case FIRELIGHTNING:
				return "Fire/Lightning";
				//break;
			case GRASSGROUND:
				return "Grass/Ground";
				//break;
			case WATER:
				return "Water";
				//break;
			case AIR:
				return "Air";
				//break;
			case NORMAL:
				return "Normal";
				//break;
		}
		
		return "";
	}
	
	//Clones Creature (Should ONLY be used on level one creatures, and leveled up appropriately elsewhere
	public CreatureInfo cloneCreature(){
		CreatureInfo c =  new CreatureInfo(ID, name, attack,speed,defense,special,type,percentAccuracy,maxHitPoints, image,data);
		CreatureAttack[] attacks = {one,two,three,four};
		
		CreatureAttack[] levelUps = {getFromLevelTwo, getFromLevelFour, getFromLevelFive};
		
		c.setAttacks(attacks, levelUps);
		return c;
	}
	
	//Heals the creature, to be called by whatever kind of pokecenter or whatever we have
	public void healCreature(){
		hitPoints = maxHitPoints; 
		
		one.resetMove();
		two.resetMove();
		three.resetMove();
		four.resetMove();
	}
	
	
	
	//Get a random attack
	public CreatureAttack getAttack(){
		int attackChoice = Random.Range(1,5);
		
		if(attackChoice ==1){
			return one;
		}
		else if(attackChoice ==2){
			return two;
		}
		else if(attackChoice ==3){
			return three;
		}
		else if(attackChoice ==4){
			return four;
		}
		
		return null;
	}
	
	//Determines if should level up, and returns true if it does level up
	
	public bool levelUp(float experienceAmnt){
		experience+=experienceAmnt;
		//Level 5 is the level cap
		if(level<5 && experience>Mathf.Pow((level+1),2)*10){
			level++;
			attack++;
			defense++;
			speed++;
			if(percentAccuracy<99){
				percentAccuracy++;
			}
			special++;
			maxHitPoints+=20;
			hitPoints=maxHitPoints;
			
			return true;
		}
		else{
			return false;
		}
		
	}
	
	public bool levelUpIgnoreRestrictions(float experienceAmnt){
		experience+=experienceAmnt;
		if(experience>Mathf.Pow((level+1),2)*10){
			Debug.Log("And this five times");
			 
			attack++;
			defense++;
			speed++;
			if(percentAccuracy<99){
				percentAccuracy++;
			}
			special++;
			maxHitPoints+=20;
			hitPoints=maxHitPoints;
			
			return true;
		}
		else{
			return false;
		}
		
	}
	
	//Levels up creature to specified level, and chooses attacks randomly
	public void levelTo(int newLevel){
		Debug.Log("Level: "+level);
		while(level<newLevel){
			level++;
			Debug.Log("Should see five times: "+level);
			
			//Make sure we have proper experience amount
			experience =Mathf.Pow((level+1),2)*10;
			
			//Add basic attributes
			levelUpIgnoreRestrictions(1.0f);
			
			if(level==2 || level==4 || level==5){
				//Leveling up, you get a new move
				CreatureAttack newMove=null;
				
				if(level==2){
					newMove =getFromLevelTwo;
				}
				else if(level==4){
					newMove = getFromLevelFour;
				}
				else if(level==5){
					newMove = getFromLevelFive;
				}
				
				//Do we take this new move?
				int takeMove = Random.Range(0,2);
				
				if(takeMove==0){
					int spotToReplace= Random.Range(0,4);
					
					if(spotToReplace==0){
						one=newMove;
					}
					else if(spotToReplace==1){
						two=newMove;
					}
					else if(spotToReplace==2){
						three=newMove;
					}
					else if(spotToReplace==3){
						four=newMove;
					}
				}
				
			}
			
		}
	}
	
	/* Override the hash code method so it is based off of the id */
	public override int GetHashCode() {
        return ID;
    }
	
	/* Override the Equals method so two creatures are equal if they have the same id*/
    public override bool Equals(object obj) {
        return Equals(obj as CreatureInfo);
    }
    public bool Equals(CreatureInfo obj) {
        return obj != null && obj.ID == this.ID;
    }
}
