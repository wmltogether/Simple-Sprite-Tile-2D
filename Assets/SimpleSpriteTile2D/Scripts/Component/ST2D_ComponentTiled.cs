using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moogle.SmartTile2D.TileManager;
namespace moogle.SmartTile2D
{
    public class ST2D_ComponentTiled : MonoBehaviour
    {
        public string tileTag = "None";
		public int zOrder = 0;
        public Material mat;
		[SerializeField]
        public TileChilds tileChilds;

		public int pixelPerUnit_texture = 32;

		public int tile_size = 32;
        private string[] childNameTable = TileBase.subID;
        private Vector2[] childPosBase = TileBase.subPos;


        public void initTileObject()
        {
            if (mat == null){
				mat = Resources.Load("Material/Default2D") as Material;
			}
            Sprite[] sprites;
            for (int i = 0;i< 4;i++)
            {
                //first build first-level child object NW NE SW SE
                GameObject childObject = new GameObject();
                childObject.transform.SetParent(transform);
                childObject.name = childNameTable[i];
                childObject.transform.localPosition = childPosBase[i]* ((float)tile_size / (float)pixelPerUnit_texture);
                childObject.transform.localScale = Vector2.one * 1f;
                SpriteRenderer render = childObject.AddComponent<SpriteRenderer>();
                render.sortingOrder = zOrder;
                switch (childObject.name)
                {
                    case "NW":
                        sprites = tileChilds.GetSpritesFromID("AANW");
                        if (sprites.Length>=1) render.sprite = sprites[Random.Range(sprites.GetLowerBound(0), sprites.GetUpperBound(0))];
                        break;
                    case "NE":
                        sprites = tileChilds.GetSpritesFromID("AANE");
                        if (sprites.Length>=1) render.sprite = sprites[Random.Range(sprites.GetLowerBound(0), sprites.GetUpperBound(0))];
                        break;
                    case "SW":
                        sprites = tileChilds.GetSpritesFromID("AASW");
                        if (sprites.Length>=1) render.sprite = sprites[Random.Range(sprites.GetLowerBound(0), sprites.GetUpperBound(0))];
                        break;
                    case "SE":
                        sprites = tileChilds.GetSpritesFromID("AASE");
                        if (sprites.Length>=1) render.sprite = sprites[Random.Range(sprites.GetLowerBound(0), sprites.GetUpperBound(0))];
                        break;
                }             
            }
        }

        public void ResetSortingOrder()
        {
            var renders = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renders)
            {
                r.sortingOrder = zOrder;
            }
        }
        public void ReloadSprites(Neighbor neighborData)
        {
            Sprite nw_sprite = tileChilds.GetRandSpriteFromID("AANW");
			Sprite ne_sprite = tileChilds.GetRandSpriteFromID("AANE");
			Sprite sw_sprite = tileChilds.GetRandSpriteFromID("AASW");
			Sprite se_sprite = tileChilds.GetRandSpriteFromID("AASE");
			if (neighborData.N == false && neighborData.W == false && neighborData.S == false && neighborData.E == false){
				for (int i=0;i<4;i++){
				var subObjectName = TileBase.subID[i];

				if (subObjectName == "NW") {
					transform.FindChild("NW").GetComponent<SpriteRenderer>().sprite = nw_sprite;}
				if (subObjectName == "NE") {
					transform.FindChild("NE").GetComponent<SpriteRenderer>().sprite = ne_sprite;}
				if (subObjectName == "SW") {
					transform.FindChild("SW").GetComponent<SpriteRenderer>().sprite = sw_sprite;}
				if (subObjectName == "SE") {
					transform.FindChild("SE").GetComponent<SpriteRenderer>().sprite = se_sprite;}
				}
				return;
			}
            #region NW
            if (neighborData.N == true && neighborData.W == true && neighborData.NW == true){
                nw_sprite = tileChilds.GetRandSpriteFromID("CTNW");
            }
            if (neighborData.N == true && neighborData.W == true && neighborData.NW == false){
				//concave edge :Set to [NWU]
				nw_sprite = tileChilds.GetRandSpriteFromID("CCNW");
			}
			if (neighborData.N == false && neighborData.W == true && neighborData.NW == true){
				//no edge :Set to [N]
				nw_sprite = tileChilds.GetRandSpriteFromID("UDNW");
			}
			if (neighborData.N == false && neighborData.W == true && neighborData.NW == false){
				//no edge :Set to [N]
				nw_sprite = tileChilds.GetRandSpriteFromID("UDNW");
			}
			if (neighborData.N == false && neighborData.W == false && neighborData.NW == true){
				//convex edge :Set to [NW]
				nw_sprite = tileChilds.GetRandSpriteFromID("CVNW");
			}
			if (neighborData.N == true && neighborData.W == false && neighborData.NW == true){
				//no edge :Set to [W]
				nw_sprite = tileChilds.GetRandSpriteFromID("LRNW");
			}
			if (neighborData.N == true && neighborData.W == false && neighborData.NW == false){
				//no edge :Set to [W]
				nw_sprite = tileChilds.GetRandSpriteFromID("LRNW");
			}
			if (neighborData.N == false && neighborData.W == false && neighborData.NW == false){
				//convex edge :Set to [NW]
				nw_sprite = tileChilds.GetRandSpriteFromID("CVNW");
			}
            #endregion
            #region NE
			//EDGE NE
			if (neighborData.N == true && neighborData.E == true && neighborData.NE == true){
				//no edge & all Neighbor :Set to [C]
				ne_sprite = tileChilds.GetRandSpriteFromID("CTNE");
			}
			if (neighborData.N == true && neighborData.E == true && neighborData.NE == false){
				//concave edge :Set to [NEU]
				ne_sprite = tileChilds.GetRandSpriteFromID("CCNE");
			}
			if (neighborData.N == false && neighborData.E == true && neighborData.NE == true){
				//no edge :Set to [N]
				ne_sprite = tileChilds.GetRandSpriteFromID("UDNE");
			}
			if (neighborData.N == false && neighborData.E == true && neighborData.NE == false){
				//no edge :Set to [N]
				ne_sprite = tileChilds.GetRandSpriteFromID("UDNE");
			}
			if (neighborData.N == false && neighborData.E == false && neighborData.NE == true){
				//convex edge :Set to [NE]
				ne_sprite = tileChilds.GetRandSpriteFromID("CVNE");
			}
			if (neighborData.N == true && neighborData.E == false && neighborData.NE == true){
				//no edge :Set to [E]
				ne_sprite = tileChilds.GetRandSpriteFromID("LRNE");
			}
			if (neighborData.N == true && neighborData.E == false && neighborData.NE == false){
				//no edge :Set to [E]
				ne_sprite = tileChilds.GetRandSpriteFromID("LRNE");
			}
			if (neighborData.N == false && neighborData.E == false && neighborData.NE == false){
				//convex edge :Set to [NE]
				ne_sprite = tileChilds.GetRandSpriteFromID("CVNE");
			}
			#endregion
			#region SW
			//EDGE SW
			if (neighborData.S == true && neighborData.W == true && neighborData.SW == true){
				//no edge & all Neighbor :Set to [C]
				sw_sprite = tileChilds.GetRandSpriteFromID("CTSW");
			}
			if (neighborData.S == true && neighborData.W == true && neighborData.SW == false){
				//concave edge :Set to [SWU]
				sw_sprite = tileChilds.GetRandSpriteFromID("CCSW");
			}
			if (neighborData.S == false && neighborData.W == true && neighborData.SW == true){
				//no edge :Set to [S]
				sw_sprite = tileChilds.GetRandSpriteFromID("UDSW");
			}
			if (neighborData.S == false && neighborData.W == true && neighborData.SW == false){
				//no edge :Set to [S]
				sw_sprite = tileChilds.GetRandSpriteFromID("UDSW");
			}
			if (neighborData.S == false && neighborData.W == false && neighborData.SW == true){
				//convex edge :Set to [SW]
				sw_sprite = tileChilds.GetRandSpriteFromID("CVSW");
			}
			if (neighborData.S == true && neighborData.W == false && neighborData.SW == true){
				//no edge :Set to [W]
				sw_sprite = tileChilds.GetRandSpriteFromID("LRSW");
			}
			if (neighborData.S == true && neighborData.W == false && neighborData.SW == false){
				//no edge :Set to [W]
				sw_sprite = tileChilds.GetRandSpriteFromID("LRSW");
			}
			if (neighborData.S == false && neighborData.W == false && neighborData.SW == false){
				//convex edge :Set to [SW]
				sw_sprite = tileChilds.GetRandSpriteFromID("CVSW");
			}
			#endregion
			#region SE
			//EDGE NE
			if (neighborData.S == true && neighborData.E == true && neighborData.SE == true){
				//no edge & all Neighbor :Set to [C]
				se_sprite = tileChilds.GetRandSpriteFromID("CTSE");
			}
			if (neighborData.S == true && neighborData.E == true && neighborData.SE == false){
				//concave edge :Set to [SEU]
				se_sprite = tileChilds.GetRandSpriteFromID("CCSE");
			}
			if (neighborData.S == false && neighborData.E == true && neighborData.SE == true){
				//no edge :Set to [S]
				se_sprite = tileChilds.GetRandSpriteFromID("UDSE");
			}
			if (neighborData.S == false && neighborData.E == true && neighborData.SE == false){
				//no edge :Set to [S]
				se_sprite = tileChilds.GetRandSpriteFromID("UDSE");
			}
			if (neighborData.S == false && neighborData.E == false && neighborData.SE == true){
				//convex edge :Set to [SE]
				se_sprite = tileChilds.GetRandSpriteFromID("CVSE");
			}
			if (neighborData.S == true && neighborData.E == false && neighborData.SE == true){
				//no edge :Set to [E]
				se_sprite = tileChilds.GetRandSpriteFromID("LRSE");
			}
			if (neighborData.S == true && neighborData.E == false && neighborData.SE == false){
				//no edge :Set to [E]
				se_sprite = tileChilds.GetRandSpriteFromID("LRSE");
			}
			if (neighborData.S == false && neighborData.E == false && neighborData.SE == false){
				//convex edge :Set to [SE]
				se_sprite = tileChilds.GetRandSpriteFromID("CVSE");
			}
			#endregion

            for (int i=0;i<4;i++){
				var subObjectName = TileBase.subID[i];

				if (subObjectName == "NW") {
					transform.FindChild("NW").GetComponent<SpriteRenderer>().sprite = nw_sprite;}
				if (subObjectName == "NE") {
					transform.FindChild("NE").GetComponent<SpriteRenderer>().sprite = ne_sprite;}
				if (subObjectName == "SW") {
					transform.FindChild("SW").GetComponent<SpriteRenderer>().sprite = sw_sprite;}
				if (subObjectName == "SE") {
					transform.FindChild("SE").GetComponent<SpriteRenderer>().sprite = se_sprite;}
			}


        }


    }
}