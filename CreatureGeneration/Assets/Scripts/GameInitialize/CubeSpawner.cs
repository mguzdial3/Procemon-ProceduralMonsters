using UnityEngine;
using System.Collections;


//Gaurav, you'll want to put your stuff here, basically
public class CubeSpawner : MonoBehaviour {
	private class patch{
		public string type;
		public GameObject cube;
		public patch(GameObject cb,string typ)
		{
			cube=cb;
			type=typ;
		}
	}
	private class point{
		public int x,y;
		public point(int x1,int y1)
		{
			x=x1;
			y=y1;
		}
	}
	// Use this for initialization
	int sizeX=40,sizeY=40;
	patch[,] mapContents=new patch[40,40];
	private Texture grassTexture,roadTexture,fenceTexture,waterTexture,beachTexture;
	
		
	public GameObject Cube;
	ArrayList paths=new ArrayList();
	// Use this for initialization
	void Start () {
		grassTexture = Resources.Load("grass") as Texture;
		roadTexture = Resources.Load("road") as Texture;
		fenceTexture = Resources.Load("fence") as Texture;
		waterTexture = Resources.Load("water") as Texture;
		beachTexture = Resources.Load("beach") as Texture;
		generateMap();
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	//The funcs I give are below this line
	private void generateMap()
	{
		//Grassify everything and Add border
		createGrass();
		//Pathing
		createRoad();
		//Watering hole
		createWater();
	}
	private void createGrass()
	{
		for (int y = 0; y < sizeX; y++) {
			for (int x = 0; x < sizeY; x++) {
				//GameObject cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
				//cube.transform.position = new Vector3(x-sizeX/2, y-sizeY/2, 0);
				GameObject cube = Instantiate(Cube,new Vector3((x-sizeX/2)*13,(y-sizeY/2)*13,0.0f), Cube.transform.rotation) as GameObject;

				if(x<2 || y<2 || x>sizeX-3 || y>sizeY-3)
				{
					//cube.renderer.material.color = Color.red;
					cube.renderer.material.mainTexture=fenceTexture;
					cube.tag="Unpassable";
					mapContents[x,y]=new patch(cube,"fence");
				}
				else
				{
					//cube.renderer.material.color = Color.green;
					cube.renderer.material.mainTexture=grassTexture;
					if(Random.value<0.4)
					{
						cube.tag="ContainsCreatures";
					}
					mapContents[x,y]=new patch(cube,"grass");
				}
			}
		}
	}
	private void createRoad()
	{
		string[] directions={"up","down","left","right"};
		string going=directions[0];
		int count=0;
		int seedX=12,seedY=19;
		while(count<280)
		{
			createRoadPatch(seedX,seedY);
			if(going=="up")
			{
				//createRoadPatch(seedX-1,seedY);
				seedY--;
			}
			if(going=="down")
			{
				//createRoadPatch(seedX-1,seedY);
				seedY++;
			}
			if(going=="left")
			{
				//createRoadPatch(seedX,seedY-1);
				seedX--;
			}
			if(going=="right")
			{
				//createRoadPatch(seedX,seedY-1);
				seedX++;
			}
			while(seedX<2 || seedX > sizeX-3 || seedY<2 || seedY>sizeY-3)
			{
				point branch=(point)paths[(int)(Random.value*(paths.Count-1))];
				seedX=branch.x;
				seedY=branch.y;
				going=directions[different(going)];
			}
			//if(Random.value>0.95)
			//	going=directions[different(going)];
			count++;
		}
	}
	private int different(string g)
	{
		if(g=="up" || g=="down")
		{
			if(Random.value>0.5)
				return 2;
			return 3;
		}
		if(Random.value>0.5)
			return 1;
		return 0;
	}
	private void createRoadPatch(int seedX,int seedY)
	{
		patch p=mapContents[seedX,seedY];
		paths.Add(new point(seedX,seedY));
		GameObject cube=p.cube;
		cube.renderer.material.mainTexture=roadTexture;
		p.type="road";
		cube.tag="Road";
	}
	private void createWater()
	{
		int noLakes=Random.Range(2,4);
		do
		{
			int waterSizeX=Random.Range(6,15);
			int waterSizeY=Random.Range(6,15);
			int count=0;
			do
			{
				int seedX=Random.Range(3,sizeX-3);
				int seedY=Random.Range(3,sizeY-3);
				bool canPlace=checkPlacement(seedX,seedY,waterSizeX,waterSizeY);
				if(canPlace)
				{
					placeWater(seedX,seedY,waterSizeX,waterSizeY);
					count=100;
				}
				count++;
			}while(count<100);
			noLakes--;
		}while(noLakes>0);
	}
	private bool checkPlacement(int left,int top,int right,int bottom)
	{
		right=left+right;
		bottom=top+bottom;
		for(int x=left;x<right;x++)
		{
			for(int y=top;y<bottom;y++)
			{
				if(mapContents[x,y].type!="grass")
					return false;
			}
		}
		return true;
	}
	private void placeWater(int left,int top,int right,int bottom)
	{
		right=left+right;
		bottom=top+bottom;
		for(int x=left;x<right;x++)
		{
			for(int y=top;y<bottom;y++)
			{
				if(x==left || x==right-1 || y==top || y==bottom-1)
				{
					mapContents[x,y].cube.renderer.material.mainTexture=beachTexture;
					mapContents[x,y].type="beach";
					mapContents[x,y].cube.tag="Beach";
				}
				else
				{
					mapContents[x,y].cube.renderer.material.mainTexture=waterTexture;
					mapContents[x,y].type="water";
					mapContents[x,y].cube.tag="Unpassable";
				}
			}
		}
	}
}
