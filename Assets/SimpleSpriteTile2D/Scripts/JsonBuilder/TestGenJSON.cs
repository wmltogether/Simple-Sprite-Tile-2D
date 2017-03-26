using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using moogle.SmartTile2D;

public class TestGenJSON : MonoBehaviour {

	public List<string> SpriteIDTable = new List<string>() {
				"AANW","AANE","CCNW","CCNE",
				"AASW","AASE","CCSW","CCSE",
				"CVNW","LRNE","LRNW","CVNE",
				"UDSW","CTSE","CTSW","UDSE",
				"UDNW","CTNE","CTNW","UDNE",
				"CVSW","LRSE","LRSW","CVSE"};
	// Use this for initialization
	void Start () 
	{

		TileJsonReader.TextureInfo textureInfo = new TileJsonReader.TextureInfo();

		textureInfo.textureName = "groundBlock.png";
		textureInfo.pixelPerUnit = 32;
		textureInfo.width = 64;
		textureInfo.Height = 96;

		List<TileJsonReader.TileInfo> tiles = new List<TileJsonReader.TileInfo>();
		
		for (int tile_num = 0; tile_num<1;tile_num++)
		{
			TileJsonReader.TileInfo m_tile = new TileJsonReader.TileInfo();
			m_tile.tileTag = "GroundBlock";
			m_tile.tileType = "Tile";
			m_tile.tileName = "GroundBlock";
			m_tile.tileWidth = 32;
			var metas = new List<TileJsonReader.SpriteMeta>();

			for (int row=0;row <6;row++){
				for (int col=0;col<4;col++){
					TileJsonReader.SpriteMeta meta = new TileJsonReader.SpriteMeta();
					int x = col * 16;
					int y = row * 16;
					meta.name = SpriteIDTable[col + row*4];// SET ID 
					meta.x = x;
					meta.y = y;
					meta.pivotX = 0.5f;
					meta.pivotY = 0.5f;
					meta.width = 16;
					meta.height = 16;
					metas.Add(meta);
				}
			}
			m_tile.spriteMetaData = metas.ToArray();
			tiles.Add(m_tile);

		}
		textureInfo.tiles = tiles.ToArray();
		string dst = TileJsonReader.WriteJSON(textureInfo);
		File.WriteAllText("./Assets/SimpleSpriteTile2D/Sample/sample.json",dst,Encoding.UTF8);
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
