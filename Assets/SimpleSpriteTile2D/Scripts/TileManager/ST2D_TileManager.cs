using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace moogle.SmartTile2D.TileManager
{

	public static class TileBase
	{
		public static string[] subID = new string[]{"NW","NE","SW","SE"};
		public static HashSet<string> subIDSet = new HashSet<string>() {"NW","NE","SW","SE"};
		public static Vector2[] subPos = new Vector2[] {new Vector2 (-0.25f,0.25f),//NW
												  new Vector2 (0.25f,0.25f),//NE
												  new Vector2 (-0.25f,-0.25f),//SW
												  new Vector2 (0.25f,-0.25f)//SE
												  };

	}
	public enum ST2D_PrefabType
	{
		Simple,
		Tile
	}
	
	public class TileInfo
	{
		public Vector2 xy;

		public string layerName = "Default";

		public ST2D_ComponentTiled tile_Component;

	}
	[System.Serializable]
	public class TileChilds
	{
		public HashSet<string> SpriteIDTable = new HashSet<string>() {
				"AANW","AANE","CCNW","CCNE",
				"AASW","AASE","CCSW","CCSE",
				"CVNW","LRNE","LRNW","CVNE",
				"UDSW","CTSE","CTSW","UDSE",
				"UDNW","CTNE","CTNW","UDNE",
				"CVSW","LRSE","LRSW","CVSE"};
		public ChildSprites AA; //ALL EDGE ,NO NEIGHBOR
		public ChildSprites CT; //CENTER
		public ChildSprites LR; // LEFT AND RIGHT EDGE 
		public ChildSprites UD; // UP AND DOWN EDGE
		public ChildSprites CV; // CONVEX
		public ChildSprites CC; // CONCAVE

		public Sprite GetRandSpriteFromID(string id){
			Sprite[] v = this.GetSpritesFromID(id);
			if (v.Length > 0){
				return v[Random.Range(v.GetLowerBound(0), v.GetUpperBound(0))];
			}
			else{
				return null;
			}
		}
		public Sprite[] GetSpritesFromID(string id)
		{
			Sprite[] result = new Sprite[]{};
			if (!SpriteIDTable.Contains(id)){
				return result;
			}
			switch (id){
				case "AANW":
					result = AA.NW;
					break;
				case "AANE":
					result = AA.NE;
					break;
				case "AASW":
					result = AA.SW;
					break;
				case "AASE":
					result = AA.SE;
					break;
				case "CTNW":
					result = CT.NW;
					break;
				case "CTNE":
					result = CT.NE;
					break;
				case "CTSW":
					result = CT.SW;
					break;
				case "CTSE":
					result = CT.SE;
					break;
				case "LRNW":
					result = LR.NW;
					break;
				case "LRNE":
					result = LR.NE;
					break;
				case "LRSW":
					result = LR.SW;
					break;
				case "LRSE":
					result = LR.SE;
					break;
				case "UDNW":
					result = UD.NW;
					break;
				case "UDNE":
					result = UD.NE;
					break;
				case "UDSW":
					result = UD.SW;
					break;
				case "UDSE":
					result = UD.SE;
					break;
				case "CVNW":
					result = CV.NW;
					break;
				case "CVNE":
					result = CV.NE;
					break;
				case "CVSW":
					result = CV.SW;
					break;
				case "CVSE":
					result = CV.SE;
					break;
				case "CCNW":
					result = CC.NW;
					break;
				case "CCNE":
					result = CC.NE;
					break;
				case "CCSW":
					result = CC.SW;
					break;
				case "CCSE":
					result = CC.SE;
					break;				
			}
			return result;
		}
	}
	[System.Serializable]
	public class ChildSprites
	{
		public Sprite[] NW;
		public Sprite[] NE;
		public Sprite[] SW;
		public Sprite[] SE;

		public ChildSprites(Sprite[] nw, Sprite[] ne, Sprite[] sw ,Sprite[] se){
			NW = nw;
			NE = ne;
			SW = sw;
			SE = se;
		}

	}

	public class Neighbor 
	{
		public bool N = false;
		public bool S = false;
		public bool W = false;
		public bool E = false;

		public bool NE = false;
		public bool NW = false;
		public bool SE = false;
		public bool SW = false;

	}
	public class Singleton < T> where T : new ()
	{
    	protected static T instance = default (T);
    	public static T Instance
    	{
        	get
        	{
            	if (instance == null )
            	{
                	instance = new T();
            	}
            	return instance;
        	}
    	}
	}

	public class ST2D_TileManager : Singleton<ST2D_TileManager>
	{

		private HashSet<Vector2> neighborVectors = new HashSet<Vector2>{
				new Vector2(-1f,1f),
				new Vector2(0f, 1f),
				new Vector2 (1f, 1f),

				new Vector2(-1f,0f),
				new Vector2(0f,0f),
				new Vector2(1f,0f),

				new Vector2(-1f,-1f),
				new Vector2(0f,-1f),
				new Vector2(1f, -1f)

		};
		public HashSet<TileInfo> tiles = new HashSet<TileInfo>();
		public void AddTile(TileInfo tile)
		{
			if (!tiles.Contains(tile)){
				//Debug.Log(string.Format("Add Tile {0}:{1}",tile.xy,tile.tile_Component.tileTag));
				tiles.Add(tile);
			}
		}

		public Neighbor FindNeighbor(TileInfo tile, float intervalUnit)
		{
			var current_xy = tile.xy;
			var current_tag = tile.tile_Component.tileTag;
			var current_layer = tile.layerName;

			Neighbor neighborData = new Neighbor();
			foreach (TileInfo m_tile in tiles)
			{
				var xy = m_tile.xy;
				string tileTag = m_tile.tile_Component.tileTag;
				string layerName = m_tile.layerName;
				if (tileTag == current_tag && layerName == current_layer)
				{
					if (xy == (current_xy + new Vector2(0f,intervalUnit)))
					{
						// N 
						neighborData.N = true;
					}
					if (xy == (current_xy + new Vector2(0f,-intervalUnit)))
					{
						// N 
						neighborData.S = true;
					}
					if (xy == (current_xy + new Vector2(-intervalUnit,0f)))
					{
						// N 
						neighborData.W = true;
					}
					if (xy == (current_xy + new Vector2(intervalUnit,0f)))
					{
						// N 
						neighborData.E = true;
					}
					if (xy == (current_xy + new Vector2(-intervalUnit,intervalUnit)))
					{
						// N 
						neighborData.NW = true;
					}
					if (xy == (current_xy + new Vector2(intervalUnit,intervalUnit)))
					{
						// N 
						neighborData.NE = true;
					}
					if (xy == (current_xy + new Vector2(-intervalUnit,-intervalUnit)))
					{
						// N 
						neighborData.SW = true;
					}
					if (xy == (current_xy + new Vector2(intervalUnit,-intervalUnit)))
					{
						// N 
						neighborData.SE = true;
					}
				}
			}
			return neighborData;



		}
		public void MoveTile(TileInfo tile, Vector2 dstPos)
		{
			if (tiles.Contains(tile)){
				tiles.Remove(tile);
			}
			tile.xy = dstPos;
			tiles.Add(tile);
			//Debug.Log(string.Format("Move Tile {0} to {1},{2} :{3}",o,dstPos.x,dstPos.y,tile.tile_Component.tileTag));
		}
		public void RemoveTile(TileInfo tile){
			if (tiles.Contains(tile)){
				//Debug.Log(string.Format("Remove Tile {0} :{1}",tile.xy,tile.tile_Component.tileTag));
				tiles.Remove(tile);
			}
			
		}

		public void DestoryAllTile()
		{
			tiles.Clear();
		}
		public void UpdateAllTileByTag(string tileTag, float intervalUnit)
		{
			//Force Update All tiles by Tile Tag
			//强制更新当前Tag下的所有tile
			foreach (var m_tile in tiles){
				if (m_tile.tile_Component.tileTag == tileTag)
				{
					var neighborData = this.FindNeighbor(m_tile, intervalUnit);
					m_tile.tile_Component.ReloadSprites(neighborData);

				}
			}
		}
		public void UpdateTileByNeighbor(Vector2 position, string tileTag, float intervalUnit)
		{
			//Don't Update all tiles , only update 8 Neighbor tiles instead
			//只更新周围的8个临近tile，其他的tile不做更新（适合实时修改地图结构使用）
			foreach (var m_tile in tiles){
				if (m_tile.tile_Component.tileTag == tileTag)
				{
					if (neighborVectors.Contains((m_tile.xy - position)/intervalUnit)){
						var neighborData = this.FindNeighbor(m_tile, intervalUnit);
						m_tile.tile_Component.ReloadSprites(neighborData);
					}
				}
			}

		}

	}

}
