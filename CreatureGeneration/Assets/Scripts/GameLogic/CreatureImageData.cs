using UnityEngine;
using System.Collections;

//Carries metadata on creature image (and used to describe creature image)
public class CreatureImageData {
	public Texture2D image;
	
	
	//Different body sizes
	public const int BODY_SMALL = 0, BODY_MEDIUM = 1, BODY_LARGE = 2;
	public int bodySize = 0;
	
	//Different head sizes
	public const int HEAD_SMALL = 3, HEAD_MEDIUM = 4, HEAD_LARGE = 5;
	public int headSize = HEAD_SMALL;

	//Different upper limb types
	public const int NO_HANDS = 4, RECT_HANDS = 5, OVAL_HANDS = 6;
	public int handType = NO_HANDS;
	
	//Different legs types
	public const int NO_LEGS = 7, ROUND_LEGS= 8, RECT_LEGS = 9;
	public int legType = NO_LEGS;
	
	//Different EyeSizes
	public const int SMALL_EYES = 10, MEDIUM_EYES = 11, LARGE_EYES = 12;
	public int eyeSize = LARGE_EYES;
	
	
	//Body Types
	public const int BODY_RECT = 13, BODY_CIRCLE =14, BODY_TESSEL=15, NO_BODY = 16;
	public int bodyType = BODY_RECT;
	
	//Body Types
	public const int HEAD_RECT = 17, HEAD_CIRCLE =18, HEAD_TESSEL=19;
	public int headType = HEAD_RECT;
	
	//Belly Types
	public const int BELLY_RECT = 20, BELLY_CIRCLE =21, NO_BELLY=22;
	public int bellyType = NO_BELLY;
	
	
	public bool hasEars = false;
	public bool hasHorns = false;
	
	
	public CreatureImageData(int type){
		//Sets up creature image 
		image = makeCreatureImage(type);
	}
	
	
	public CreatureImageData(int type, bool boss){
		//Sets up creature image 
		if(boss){
			image = makeCreatureImage(type,BODY_LARGE,HEAD_LARGE,BODY_TESSEL,HEAD_TESSEL,true);
		}
		else{
			image = makeCreatureImage(type);
		}
	}
	
	public CreatureImageData(Texture2D _image){
		image=_image;
	}
	
	
	
	public Texture2D makeCreatureImage(int type){
		Texture2D t = new Texture2D(100,100);
			
		
		
		//NORMAL, FIRE/LIGHTNING, WATER, GRASS/GROUND, AND AIR
		Color[] choices = {Color.grey, Color.red, Color.blue,Color.green,Color.white};
		
		int colorChoice = type;
		
		Color toSet = choices[colorChoice];
		
		//Only alter these for body size stuff
		int bodyWidthMin = 10; 
		int bodyWidthMax = 60;
		int bodyHeightMin = 10; 
		int bodyHeightMax = 80;
		
		
		int bodyWidth = Random.Range(bodyWidthMin, bodyWidthMax);
		int bodyHeight = Random.Range(bodyHeightMin,bodyHeightMax);
		
		//DETERMINE BODY SIZE 
		int bodyArea = bodyWidth*bodyHeight;
		
		int avgBodyArea = ((bodyWidthMax-bodyWidthMin)/2)* ((bodyHeightMax-bodyHeightMin)/2);
		
		if(bodyArea<(3*avgBodyArea)/4){
			bodySize = BODY_SMALL;
		}
		else if(bodyArea>(avgBodyArea*1.25f)){
			bodySize = BODY_LARGE;
		}
		else{
			bodySize = BODY_MEDIUM;
		}
		
		
		
		
		//0 for y is the bottom for some reason
		int bodyStartY = 0;
		int bodyStartX = 50-bodyWidth/2;
		
		
		//Only alter these for head size stuff
		int headWidthMin = 20; 
		int headWidthMax = 80;
		int headHeightMin = 10; 
		int headHeightMax = 100-bodyWidthMin;
		
		
		
		int headWidth = Random.Range(headWidthMin,headWidthMax);
		int headHeight = Random.Range(headHeightMin, headHeightMax);
		
		//DETERMINE Head SIZE 
		int headArea = headWidth*headHeight;
		
		int avgHeadArea = ((headWidthMax-headWidthMin)/2)* ((headHeightMax-headHeightMin)/2);
		
		if(headArea<(3*avgHeadArea)/4){
			headSize = HEAD_SMALL;
		}
		else if(headArea>(avgHeadArea*1.25f)){
			headSize = HEAD_LARGE;
		}
		else{
			headSize = HEAD_MEDIUM;
		}	
		
		
		
		int headStartX = Random.Range(50-headWidth/2,Mathf.Clamp(75-headWidth/2,0,100-headWidth) );
		int headStartY = bodyHeight;//Random.Range(bodyHeight+headHeight/2, bodyHeight);
		
		
		
		
		
		bodyType = Random.Range(BODY_RECT,NO_BODY+1);
		//Determine what type of body to use and DRAW THE BODY
		if(bodyType==BODY_RECT){
			drawRect(t, choices[colorChoice],bodyStartX,bodyStartY,bodyWidth,bodyHeight);
		}
		else if(bodyType==BODY_CIRCLE){
			drawOval(t, choices[colorChoice],bodyStartX,bodyStartY,bodyWidth,bodyHeight);
		}
		else if(bodyType==BODY_TESSEL){
			drawTessel(t,choices[colorChoice],bodyStartX,bodyStartY,bodyWidth,bodyHeight);
		}
		
		//DETERMINE AND DRAW BELLY TYPE, IF THERE'S A BODY TO GO WITH
		bellyType = Random.Range(BELLY_RECT,NO_BELLY+1);
		
		if(bodyType!=NO_BODY){
			
			Color bellyColor = choices[colorChoice]-new Color(Random.Range(0.0f,0.2f),Random.Range(0.0f,0.2f),Random.Range(0.0f,0.2f),0.0f);
			if(bellyType==BELLY_CIRCLE){
				//Get to go off the image so it looks like a half-circle
				drawRect(t,bellyColor,bodyStartX+10,bodyStartY-bodyHeight,bodyWidth-20,bodyHeight*2-10);
			}
			else if(bellyType==BELLY_RECT){
				drawRect(t,bellyColor,bodyStartX+10,bodyStartY,bodyWidth-20,bodyHeight-10);
			}
		}
		
	
		
		//Determine hand type and Draw hands
		handType = Random.Range(NO_HANDS,OVAL_HANDS+1);
		
		if(handType!=NO_HANDS && bodyType!=NO_BODY){
			Color handColor = choices[colorChoice] + (Color.white-choices[colorChoice])/(Random.Range(2,10));
			
			
			
			if(handType==RECT_HANDS){
				drawRect(t,handColor, bodyStartX-10,bodyStartY+bodyHeight/4,10,10);
				drawRect(t, handColor, bodyStartX+bodyWidth,bodyStartY+bodyHeight/4 ,10,10);
		
			}
			else if(handType==OVAL_HANDS){
				drawOval(t, handColor, bodyStartX-10,bodyStartY+bodyHeight/4,10,10);
				drawOval(t, handColor, bodyStartX+bodyWidth,bodyStartY+bodyHeight/4 ,10,10);
		
			}
		}
		
		
		
		
		
		
		//Determine head type and Draw head
		headType = Random.Range(HEAD_RECT,HEAD_TESSEL+1);
		Color headColor = choices[colorChoice]-new Color(Random.Range(-2.0f,0.2f),Random.Range(-2.0f,0.2f),Random.Range(0.0f,0.2f),0.0f);

		if(headType==HEAD_RECT){
			drawRect(t, toSet-new Color(0.1f,0.1f,0.1f,0.0f),headStartX,headStartY,headWidth,headHeight);
		}
		else if(headType==HEAD_CIRCLE){
			drawOval(t, toSet-new Color(0.1f,0.1f,0.1f,0.0f),headStartX,headStartY,headWidth,headHeight);
		}
		else{
			drawTessel(t, toSet-new Color(0.1f,0.1f,0.1f,0.0f),headStartX,headStartY,headWidth,headHeight);
		}
		
		int eyeSize = Random.Range(3,11);
		bool firstSet=false, secondSet=false;
		
		//Eyes in middle of head (plus a little bit off)
		int eyeY = headStartY+headHeight/2 + Random.Range(-5,5);
		
		int eyeSeperationAmnt = Random.Range((eyeSize/2)+1, ((headWidth/2)-eyeSize)+1);
		
		int eye1X = headStartX+headWidth/2 - (eyeSeperationAmnt);
		
		int eye2X = headStartX+headWidth/2 + (eyeSeperationAmnt);
		
		int hornWidth = 10;
		
		//Drawing horns
		int horn1X = headStartX+headWidth/2 - (headWidth/2);

		int horn2X = headStartX+headWidth/2 + (headWidth/2);
		
		
		int hazEarz = Random.Range(0,3);
		
		if(hazEarz==0){
			hasEars=true;
		}
		else{
			hasEars = false;
		}
		
		if(hasEars){
			drawEars(t,choices[colorChoice]+(Color.white-choices[colorChoice])/10,true,horn1X,headStartY+headHeight/3,hornWidth,headWidth/2);
			drawEars(t, choices[colorChoice]+(Color.white-choices[colorChoice])/10,false,horn2X-hornWidth,headStartY+headHeight/3,hornWidth,headWidth/2);
		}
		
		
		int hazHornz = Random.Range(0,3);
		
		if(hazHornz==0){
			hasHorns=true;
		}
		else{
			hasHorns = false;
		}
		
		if(hasHorns){
		Color hornColor = choices[colorChoice]-new Color(Random.Range(0.2f,0.9f),Random.Range(0.2f,0.9f),Random.Range(0.2f,0.9f),0.0f);

			drawHorns(t, hornColor,true,horn1X,headStartY+10+headSize/2,hornWidth,headSize/2);
			drawHorns(t, hornColor,false,horn2X-hornWidth,headStartY+10+headSize/2,hornWidth,headSize/2);
		}
		
		if(eyeY+eyeSize<0 || eyeY+eyeSize>100){
			eyeY = Random.Range(80,90);
		}
		
		for(int x = eye1X; x<eye1X+eyeSize; x++){
			for(int y = eyeY; y<eyeY+eyeSize; y++){
				if(x==eye1X+eyeSize/2-1 && y==eyeY+eyeSize/2){
					t.SetPixel(x,y,Color.white);
				}
				else{
					t.SetPixel(x,y,Color.black);
				}
			}
		}
		
		for(int x = eye2X; x<eye2X+eyeSize; x++){
			for(int y = eyeY; y<eyeY+eyeSize; y++){
				if(x==eye2X+eyeSize/2-1 && y==eyeY+eyeSize/2){
					t.SetPixel(x,y,Color.white);
				}
				else{
					t.SetPixel(x,y,Color.black);
				}
			}
		}
		
		
		
		legType = Random.Range(NO_LEGS,RECT_LEGS+1);
		
		if(legType!=NO_LEGS && bodyType!=NO_BODY){
			if(legType==RECT_LEGS){
				drawRect(t, headColor , bodyStartX-8,bodyStartY,10,10);
				drawRect(t, headColor , bodyStartX+bodyWidth-2,bodyStartY ,10,10);
			}
			else if(legType==ROUND_LEGS){
				drawOval(t, choices[colorChoice] , bodyStartX-8,bodyStartY,10,15);
				drawOval(t, choices[colorChoice] , bodyStartX+bodyWidth-2,bodyStartY ,10,15);
			}
		}
		
		
		t.Apply();
		
		
		return t;
		
	}
	
	
	//Makes a creature, but with some explicit info
	public Texture2D makeCreatureImage(int type, int _bodySize, int _headSize, int _bodyType, int _headType, bool hasEyebrows){
		Texture2D t = new Texture2D(100,100);
			
		
		
		//NORMAL, FIRE/LIGHTNING, WATER, GRASS/GROUND, AND AIR
		Color[] choices = {Color.grey, Color.red, Color.blue,Color.green,Color.white};
		
		int colorChoice = type;
		
		Color toSet = choices[colorChoice];
		
		//Only alter these for body size stuff
		int bodyWidthMin = 10; 
		int bodyWidthMax = 60;
		int bodyHeightMin = 10; 
		int bodyHeightMax = 80;
		
		
		int bodyWidth = Random.Range(bodyWidthMin, bodyWidthMax);
		int bodyHeight = Random.Range(bodyHeightMin,bodyHeightMax);
		
		bodySize = _bodySize;
		
		if(bodySize == BODY_SMALL){
			bodyWidth = Random.Range(bodyWidthMin, bodyWidthMax/3);
			bodyHeight = Random.Range(bodyHeightMin,bodyHeightMax/3);
		}
		else if(bodySize == BODY_MEDIUM){
			bodyWidth = Random.Range(bodyWidthMax/3, 3*(bodyWidthMax/4));
			bodyHeight = Random.Range(bodyHeightMax/3, 3*(bodyHeightMax/4));
		}
		else{
			bodyWidth = Random.Range(3*(bodyWidthMax/4), bodyWidthMax);
			bodyHeight = Random.Range(3*(bodyHeightMax/4), bodyWidthMax);
		}
		
		
		
		
		//0 for y is the bottom for some reason
		int bodyStartY = 0;
		int bodyStartX = 50-bodyWidth/2;
		
		
		//Only alter these for head size stuff
		int headWidthMin = 20; 
		int headWidthMax = 80;
		int headHeightMin = 10; 
		int headHeightMax = 100-bodyWidthMin;
		
		
		
		int headWidth = Random.Range(20,90);
		int headHeight = Random.Range(10, 100-bodyHeight);
		
		headSize = _headSize;
		
		if(headSize==HEAD_SMALL){
			headWidth = Random.Range(headWidthMin, headWidthMax/3);
			headHeight = Random.Range(headHeightMin,headHeightMax/3);
		}
		else if(headSize==HEAD_MEDIUM){
			headWidth = Random.Range(headWidthMax/3,2*(headWidthMax/3) );
			headHeight = Random.Range(headHeightMax/3, 2*(headHeightMax/3));
		}
		else{
			headWidth = Random.Range(2*(headWidthMax/3), headWidthMax );
			headHeight = Random.Range(2*(headHeightMax/3), headHeightMax);
		}	
		
		
		
		int headStartX = Random.Range(50-headWidth/2,Mathf.Clamp(75-headWidth/2,0,100-headWidth) );
		int headStartY = bodyHeight;//Random.Range(bodyHeight+headHeight/2, bodyHeight);
		
		
		
		
		
		bodyType = _bodyType;
		//Determine what type of body to use and DRAW THE BODY
		if(bodyType==BODY_RECT){
			drawRect(t, choices[colorChoice],bodyStartX,bodyStartY,bodyWidth,bodyHeight);
		}
		else if(bodyType==BODY_CIRCLE){
			drawOval(t, choices[colorChoice],bodyStartX,bodyStartY,bodyWidth,bodyHeight);
		}
		else if(bodyType==BODY_TESSEL){
			drawTessel(t,choices[colorChoice],bodyStartX,bodyStartY,bodyWidth,bodyHeight);
		}
		
		//DETERMINE AND DRAW BELLY TYPE, IF THERE'S A BODY TO GO WITH
		bellyType = Random.Range(BELLY_RECT,NO_BELLY+1);
		
		if(bodyType!=NO_BODY){
			
			Color bellyColor = choices[colorChoice]-new Color(Random.Range(0.0f,0.2f),Random.Range(0.0f,0.2f),Random.Range(0.0f,0.2f),0.0f);
			if(bellyType==BELLY_CIRCLE){
				//Get to go off the image so it looks like a half-circle
				drawRect(t,bellyColor,bodyStartX+10,bodyStartY-bodyHeight,bodyWidth-20,bodyHeight*2-10);
			}
			else if(bellyType==BELLY_RECT){
				drawRect(t,bellyColor,bodyStartX+10,bodyStartY,bodyWidth-20,bodyHeight-10);
			}
		}
		
	
		
		//Determine hand type and Draw hands
		handType = Random.Range(NO_HANDS,OVAL_HANDS+1);
		
		if(handType!=NO_HANDS && bodyType!=NO_BODY){
			Color handColor = choices[colorChoice] + (Color.white-choices[colorChoice])/(Random.Range(2,10));
			
			
			
			if(handType==RECT_HANDS){
				drawRect(t,handColor, bodyStartX-10,bodyStartY+bodyHeight/4,10,10);
				drawRect(t, handColor, bodyStartX+bodyWidth,bodyStartY+bodyHeight/4 ,10,10);
		
			}
			else if(handType==OVAL_HANDS){
				drawOval(t, handColor, bodyStartX-10,bodyStartY+bodyHeight/4,10,10);
				drawOval(t, handColor, bodyStartX+bodyWidth,bodyStartY+bodyHeight/4 ,10,10);
		
			}
		}
		
		
		
		//Determine head type and Draw head
		headType = _headType;
		Color headColor = choices[colorChoice]-new Color(Random.Range(-2.0f,0.2f),Random.Range(-2.0f,0.2f),Random.Range(0.0f,0.2f),0.0f);

		if(headType==HEAD_RECT){
			drawRect(t, toSet-new Color(0.1f,0.1f,0.1f,0.0f),headStartX,headStartY,headWidth,headHeight);
		}
		else if(headType==HEAD_CIRCLE){
			drawOval(t, toSet-new Color(0.1f,0.1f,0.1f,0.0f),headStartX,headStartY,headWidth,headHeight);
		}
		else{
			drawTessel(t, toSet-new Color(0.1f,0.1f,0.1f,0.0f),headStartX,headStartY,headWidth,headHeight);
		}
		
		int eyeSize = Random.Range(3,11);
		bool firstSet=false, secondSet=false;
		
		//Eyes in middle of head (plus a little bit off)
		int eyeY = headStartY+headHeight/4 + Random.Range(-5,5);
		
		int eyeSeperationAmnt = Random.Range(((headWidth/2)-eyeSize)-10, ((headWidth/2)-eyeSize)+1);
		
		int eye1X = headStartX+headWidth/2 - (eyeSeperationAmnt);
		
		int eye2X = headStartX+headWidth/2 + (eyeSeperationAmnt);
		
		int hornWidth = 10;
		
		//Drawing horns
		int horn1X = headStartX+headWidth/2 - (headWidth/2);

		int horn2X = headStartX+headWidth/2 + (headWidth/2);
		
		
		int hazEarz = Random.Range(0,3);
		
		if(hazEarz==0){
			hasEars=true;
		}
		else{
			hasEars = false;
		}
		
		if(hasEars){
			drawEars(t,choices[colorChoice]+(Color.white-choices[colorChoice])/10,true,horn1X,headStartY+headHeight/3,hornWidth,headWidth/2);
			drawEars(t, choices[colorChoice]+(Color.white-choices[colorChoice])/10,false,horn2X-hornWidth,headStartY+headHeight/3,hornWidth,headWidth/2);
		}
		
		
		int hazHornz = Random.Range(0,3);
		
		if(hazHornz==0){
			hasHorns=true;
		}
		else{
			hasHorns = false;
		}
		
		if(hasHorns){
		Color hornColor = choices[colorChoice]-new Color(Random.Range(0.2f,0.9f),Random.Range(0.2f,0.9f),Random.Range(0.2f,0.9f),0.0f);

			drawHorns(t, hornColor,true,horn1X,headStartY+10+headSize/2,hornWidth,headSize/2);
			drawHorns(t, hornColor,false,horn2X-hornWidth,headStartY+10+headSize/2,hornWidth,headSize/2);
		}
		
		//DRAW EYES
		for(int x = eye1X; x<eye1X+eyeSize; x++){
			for(int y = eyeY; y<eyeY+eyeSize; y++){
				if(x==eye1X+eyeSize/2-1 && y==eyeY+eyeSize/2){
					t.SetPixel(x,y,Color.white);
				}
				else{
					t.SetPixel(x,y,Color.black);
				}
			}
		}
		
		for(int x = eye2X; x<eye2X+eyeSize; x++){
			for(int y = eyeY; y<eyeY+eyeSize; y++){
				if(x==eye2X+eyeSize/2-1 && y==eyeY+eyeSize/2){
					t.SetPixel(x,y,Color.white);
				}
				else{
					t.SetPixel(x,y,Color.black);
				}
			}
		}
		
		//Determine if draw eyebrows, then do so
		Color eyeBrows = new Color(choices[colorChoice].r/10,choices[colorChoice].g/10,choices[colorChoice].b/10,1.0f);
		if(hasEyebrows){
			drawEars(t,eyeBrows,false,eye1X+5,eyeY,10,20);
			drawEars(t,eyeBrows,true,eye2X-5,eyeY,10,20);
		
		}
		
		
		
		//Determine type and then draw legs
		
		
		legType = Random.Range(NO_LEGS,RECT_LEGS+1);
		
		if(legType!=NO_LEGS && bodyType!=NO_BODY){
			if(legType==RECT_LEGS){
				drawRect(t, headColor , bodyStartX-8,bodyStartY,10,10);
				drawRect(t, headColor , bodyStartX+bodyWidth-2,bodyStartY ,10,10);
			}
			else if(legType==ROUND_LEGS){
				drawOval(t, choices[colorChoice] , bodyStartX-8,bodyStartY,10,15);
				drawOval(t, choices[colorChoice] , bodyStartX+bodyWidth-2,bodyStartY ,10,15);
			}
		}
		
		
		t.Apply();
		
		
		return t;
		
	}
	
	
	
	//Draws a horn of the given color in the given box
	private void drawEars(Texture2D t, Color c, bool leftHorn, int startX, int startY, int width, int height){
		for(int y = startY; y<startY+height; y++){
			for(int x = startX; x<startX+width; x++){
				//if(!leftHorn && x>Mathf.Asin(y) && x<width+startX){
				
				float ySin =  (((float)(y-startY))/ ((float)(height)));
				float xSin = ((float)(x-startX))/ ((float)(width));
				if(leftHorn && xSin<ySin){
					if(y>0 && y<100 && x>0 && x<100){
						t.SetPixel(x,y,c);
					}
				}
				else if(!leftHorn && xSin<ySin){
					
					//Debug.Log("X: "+xSin+". Arc of y: "+Mathf.Asin(ySin));
					int xPlace = ((startX+(width/2))-x)+(startX+(width/2));
					if(y>0 && y<100 && xPlace>0 && xPlace<100){
						t.SetPixel(xPlace,y,c);
					}
				}
				
			}
		}
		
		
	}
	
	//Draws a horn of the given color in the given box
	private void drawHorns(Texture2D t, Color c, bool leftHorn, int startX, int startY, int width, int height){
		for(int y = startY; y<startY+height; y++){
			for(int x = startX; x<startX+width; x++){
				//if(!leftHorn && x>Mathf.Asin(y) && x<width+startX){
				
				int yPlace = ((startY+(height/2))-y)+(startY+(height/2));
				if(yPlace<100 && yPlace>0 && x>0 && x<100){	
					float ySin =  ((float)(y-startY))/ ((float)(height));
					float xSin = ((float)(x-startX))/ ((float)(width));
					if(leftHorn && xSin<ySin){
						
						//Debug.Log("X: "+xSin+". Arc of y: "+Mathf.Asin(ySin));
						t.SetPixel(x,yPlace,c);
					}
					else if(!leftHorn && xSin<ySin){
						
						//Debug.Log("X: "+xSin+". Arc of y: "+Mathf.Asin(ySin));
						int xPlace = ((startX+(width/2))-x)+(startX+(width/2));
						t.SetPixel(xPlace,yPlace,c);
					}
				}
			}
		}
		
		
	}
	
	
	
	
	
	//Draw a tesseling image 
	public void drawTessel(Texture2D t, Color c, int startX, int startY, int width, int height){
		int prevX = startX;
		for(int y = startY; y<startY+height; y++){
			
			if(y>0 && y<100){
				int start = prevX + Random.Range(-1,2);
				prevX=start;
				
				if(start<0  ||(startX+width>100)){
					start = startX;
				}
				
				for(int x= start; x<startX+width-(start-startX); x++){
					
					//BUMPY
					//Color toSet = c+new Color(Random.Range(-0.05f,0.05f),Random.Range(-0.05f,0.05f),Random.Range(-0.05f,0.05f),1.0f);	
					//t.SetPixel(x,y,toSet);
					
					//COLOR SHADING
					//c = c+new Color(Random.Range(-0.01f,0.01f),Random.Range(-0.01f,0.01f),Random.Range(-0.01f,0.01f),1.0f);	
					
					if(x>0 && x<100 && y>0 && y<100){
						t.SetPixel(x,y,c);
					}
					
				}
			}
		}
	}
	
	//public void drawCircle(Texture2D t, Color c, int 
	
	//Draw a rectangle
	public void drawRect(Texture2D t,Color c, int startX, int startY, int width, int height){
		for(int x = startX; x<startX+width; x++){
			for(int y = startY; y<startY+height; y++){
				if(x>0 && x<100 && y>0 && y<100){
					t.SetPixel(x,y,c);
				}
			}
		}
	}
	
	//Returns true if within rect
	public bool insideRect(int x, int y, int minX, int minY, int width, int height){
		return ((x>minX) && x<(minX+width)) && ((y>minY) && y<(minY+height));
	}
	
	
	//Draws an oval (pass in bounding rectangle for oval info)
	private void drawOval(Texture2D t,Color c, int startX, int startY, int width, int height){
		//Debug.Log("Bounding rect: "+ new Rect(startX,startY,width,height));
		int centerX = ((width)/2) + startX;
		int centerY = ((height)/2) + startY;
		
		//Debug.Log("Centerx: "+centerX+ ". Centery: "+centerY);
		
		//Trace bouncing rectangle
		for(int x = startX; x<startX+width; x++){
			for(int y = startY; y<startY+height; y++){
				//Only draw if it is within oval
				if(insideOval(x,y,centerX,centerY,width/2,height/2)){
					if(x>0 && x<100 && y>0 && y<100){
						t.SetPixel(x,y,c);
					}
				}
				
			}
		}
		
	}
	
	//Returns true if within oval
	public bool insideOval(int x, int y, int centerX, int centerY, int width, int height){
		float dx = ((float) (x-centerX))/((float)width);
		float dy =( (float)(y-centerY))/((float)height);
		
		//Debug.Log("Dx: "+dx + ". Dy: "+dy);
		
		
		return (Mathf.Pow(dx,2) +Mathf.Pow(dy,2))<=(1);
	}
	
	//Returns true if within circle
	public bool insideCircle(int x, int y, int centerX, int centerY, int radius){
	
		
		return (Mathf.Pow((x-centerX),2) +Mathf.Pow((x-centerX),2))<=(radius*radius);
	}
}
