using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace moogle.SmartTile2D
{
	public class ST2D_AddPanelEditor : EditorWindow 
	{

	
		[MenuItem("Tools/Simple Sprite Tiles 2D /Add Tile Panel...")]
		static void Open()
		{
			Debug.Log("Setting ZOrder...");
			AddPanel();

			Debug.Log("ZOrder Rebuilding Complete.");
		}

		static void AddPanel()
		{
			GameObject panel = new GameObject("TilePanel");
            panel.AddComponent<ST2D_TilePanel>();

		}
	}

}

