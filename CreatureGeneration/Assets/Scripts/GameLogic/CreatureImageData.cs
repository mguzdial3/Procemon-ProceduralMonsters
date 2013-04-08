using UnityEngine;
using System.Collections;

//Carries metadata on creature image (and used to describe creature image)
public class CreatureImageData {
	
	
	
	//Different body sizes
	public const int BODY_SMALL = 0, BODY_MEDIUM = 1, BODY_LARGE = 2;
	public int bodySize = 0;
	
	//Different head sizes
	public const int HEAD_SMALL = 3, HEAD_MEDIUM = 4, HEAD_LARGE = 5;
	public int headSize = HEAD_SMALL;

	//Different upper limb types
	public const int NO_LIMBS = 4, SMALL_BLOBS = 5, LARGE_BLOBS = 6;
	public int limbType = NO_LIMBS;
	
	//Different legs types
	public const int NO_LEGS = 7, SMALL_LEGS = 8, LARGE_LEGS = 9;
	public int legType = NO_LEGS;
	
	//Different EyeSizes
	public const int SMALL_EYES = 10, MEDIUM_EYES = 11, LARGE_EYES = 12;
	public int eggSize = LARGE_EYES;
	
	public const int BODY_RECT = 13, BODY_CIRCLE =14, BODY_TESSEL=15;
	public int bodyType = BODY_RECT;
	
	public bool hasWings = false;
	public bool hasHorns = false;
	
	
	
	
	
}
