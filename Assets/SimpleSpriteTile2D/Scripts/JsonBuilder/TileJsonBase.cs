using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace moogle.SmartTile2D{
	public class TileJsonBase 
	{
	
	[System.Serializable]
	public class TextureInfo{
		public string textureName;
		public int width;
		public int Height;

		public int pixelPerUnit;//texture pixpixelPerUnit;
		public TileInfo[] tiles;


	}
	[System.Serializable]
	public class TileInfo{
		
		public string tileType; //Simple or Tile
		public string tileName; //Prefab name
		public string tileTag; //Prefab Tile Tag;
		public int tileWidth;
		public SpriteMeta[] spriteMetaData;// array to store sprite meta data

	}
	[System.Serializable]
	public class SpriteMeta
	{
		public string name;
		public int x;
		public int y;
		public int width;
		public int height;
		public float pivotX;
		public float pivotY;
		
	}
	
	}

}
