using UnityEngine;
using System.Collections;

//Generates all the creatures, and passes them up to the CreatureHolder
//THIS IS THE ONE FOR NATHAN TO MESS WITH
public class CreatureGenerator {
	CreatureImageGenerator imageGenerator;
	
	
	//Presenlty just does it randomly, but that needs to be changes
	public CreatureInfo[] generateCreatures(){
		imageGenerator = new CreatureImageGenerator();
		
		CreatureInfo[] creatures = new CreatureInfo[60];
		
		string[] nameBits = {"un", "za", "muh", "ga","go","gi","da","do","di","du","fa","fo","fu","la","lo","lu","ma","mu","mi","naom","nurm",
		"nor","dude","bro","va","vo","vive","mon","bro","wes","gorn","florp","bag","dern","qui","ze","xon", "ley", "durn", "yui"};
		
		
		string[] fightBits = {"punch", "munch", "slam", "bam", "wham", "jam", "schism", "tackle", "strike", "pound", "slam", "jab", "bite", "slice",
			"blast", "kick", "charge", "chomp", "thrust", "blitz", "barrage", "onslaught", "foray", "skirmish", "encroach"};
		
		string[] boostBits = {"boost", "help"};
		
		//public const int LIGHTNING= 0, GROUND =1, WATER =2, FIRE = 3, GRASS =4, AIR=5, NORMAL=6;
		string[] groundBits = {"ground", "rock", "earth", "dirt", "mud","grass", "leaf", "vine", "thorn", "branch"};
		string[] waterBits = {"water", "aqua", "rain", "tsunami", "shower"};
		string[] fireBits = {"fire", "flame", "fiery", "magma", "burn", "lightning", "thunder", "electric", "volt", "magnetic"};
		string[] airBits = {"air", "wind", "storm", "sky", "zephyr", "gust"};
		string[] normalBits = {"strong", "cunning", "killer", "death", "average"};
		
		string[][] elementalBits = {normalBits, fireBits, waterBits, groundBits, airBits};
		
		for(int i = 0; i<creatures.Length; i++){
			
			
			//Make creature Name
			int nameLength= Random.Range(2,4);
			int nameIndex = 0;
			
			
			string creatureName = "";
			while(nameIndex<nameLength){
				creatureName += nameBits[Random.Range(0, nameBits.Length)];
				nameIndex++;
			}
			
			
			//All values sum to 10
			int attack = Random.Range(1,8);
			int speed = Random.Range(1, 9-attack);
			int defense = Random.Range(1, 10-attack-speed);
			int special = Random.Range(1, 11-attack-defense-speed);
			//Type of creature determined randomly
			int type = Random.Range(0,5);
			//Randomly generate accuracy
			float percentAccuracy = Random.Range(0.8f,1.0f);
			
			
			CreatureInfo ci = new CreatureInfo(creatureName,attack, speed, defense, special, type, percentAccuracy, imageGenerator.MakeACreature(type), null);

			
			
			//Four Basic Creature Attacks
			//TODO Fill this out
			CreatureAttack[] fourBasic = new CreatureAttack[4];
			
			for(int f = 0; f<fourBasic.Length; f++){
			
				//First, determine what the type of attack will be
				int attackType = Random.Range(0,3);
				
				//Damage type
				if(attackType==0){
				}
				//Boosts my stats type
				else if(attackType==1){
					
				}
				//Harms your stats type
				else if(attackType==2){
					
				}
				
				int fightIndex=0;
				int fightLength = Random.Range(1,3);
				
				string basicAttack = "";
				
				while(fightIndex<fightLength){
					
					basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
					
					fightIndex++;
				}
				
			}
			
			//Four Basic Creature Attacks
			//TODO Fill this out
			CreatureAttack[] threeLevelGained = new CreatureAttack[4];
			
			for(int f = 0; f<threeLevelGained.Length; f++){
			
				int fightIndex=0;
				int fightLength = Random.Range(1,3);
				
				string basicAttack = "";
				
				while(fightIndex<fightLength){
					
					basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
					
					fightIndex++;
				}
			}
			
			
			
			
			
			
			
			
			
			creatures[i]=ci;
			
		}
		
		return creatures;
	}
	
	
	
}
