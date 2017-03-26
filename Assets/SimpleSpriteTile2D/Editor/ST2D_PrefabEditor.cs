using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using moogle.SmartTile2D.TileManager;
using System.IO;

namespace moogle.SmartTile2D
{
	public class ST2D_PrefabEditor : EditorWindow {

		static ST2D_PrefabEditor mainWindow;

		[MenuItem("Tools/Simple Sprite Tiles 2D /Tile Prefab Creator")]

		static void Open(){
			//Create single instance editor window
			if (mainWindow == null){
				mainWindow = CreateInstance<ST2D_PrefabEditor> ();
			}
			// set editor window title
			mainWindow.titleContent.text = "Tile Creator";
			mainWindow.minSize = new Vector2(400f,550f);
			mainWindow.ShowUtility ();
		}
		string dstDir = "Assets/Resources/Tiles/";

		string dstPrefabName = "";
		int pixelPerUnit_texture = 32;// 图片素材的ppu

		//判断是否添加 2D Collider
		bool bHasBoxCollider = false;
		//判断是否根据texture自动生成tile prefab
		bool autoGenerateTileBySprite = false;

		// 2D材质
		Texture2D texture;
		Sprite simpleSprite;
		Material custom_mat;

		TextAsset spritesheet_json;

		int tile_Width = 32;
		int tile_Height = 32;
		/* 
		int offset_X = 0;
		int offset_Y = 0;
		int padding_X = 0;
		int padding_Y = 0;*/

		int zOrder = 0;
		string tileTag = "None";
		Vector2 SpritePivot = new Vector2(0.5f,0.5f);

		GUILayoutOption[] spriteFieldOptions = new [] {
        		GUILayout.Width (64),
        		GUILayout.Height (64)
    		};
		GUILayoutOption[] pivotFieldOptions = new [] {
        		GUILayout.Width (160),
        		GUILayout.Height (20)
    		};
		void OnGUI(){
			GUILayout.Label(Language.GetLanguage("Select Prefab Type"));
			toolBarselected = GUILayout.Toolbar(toolBarselected, new string[]{ "Simple ", "Tile", "Load From JSON"});

			GUILayout.Label(Language.GetLanguage("Prefab Output Path:"));
			dstDir = GUILayout.TextField(dstDir);
			EditorGUILayout.Space();  
			GUILayout.Label("---------------------------------------");
        	EditorGUILayout.Space();
			switch (toolBarselected)
			{
				case 0:
					GUILayout.Label("--------Single Sprite Mode------");
					UI_LoadSimpleContents ();
					EditorGUILayout.Space();  
        			EditorGUILayout.Space();
					
					
					break;
				case 1:
					GUILayout.Label("--------Single Tile Mode------");
					UI_LoadTileContents();
					break;
				case 2:
					GUILayout.Label("--------Multiple Tile Mode------");
					UI_LoadJSONContents();
					break;

			}
			EditorGUILayout.Space();  
        	EditorGUILayout.Space();
			GUI.Label(new Rect(260f,520f,200f,20f),"Created by moogle,2017");
			

		}
		void UI_LoadSimpleContents(){
			//Draw GUI for Editor

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(Language.GetLanguage("Select 2D Sprite"));
			simpleSprite = EditorGUILayout.ObjectField(simpleSprite , typeof(Sprite), false ,spriteFieldOptions) as Sprite;
			EditorGUILayout.EndHorizontal();
			GUILayout.Label("------------------");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(Language.GetLanguage("Pixel Per Unit:"));
			pixelPerUnit_texture = EditorGUILayout.IntField(pixelPerUnit_texture);
			GUILayout.Label(Language.GetLanguage("Select Tile Width:"));
			EditorGUILayout.IntField(tile_Width);
			GUILayout.Label(Language.GetLanguage("Tile Tag:"));
			tileTag = EditorGUILayout.TextField(tileTag);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space(); 
			if (simpleSprite != null && (dstPrefabName == null || dstPrefabName == "") ){
					dstPrefabName = simpleSprite.name;
					if (tileTag == "None"){
						tileTag = simpleSprite.name;
					}
				}
				GUILayout.Label(Language.GetLanguage("Output Prefab Name:"));
				EditorGUILayout.BeginHorizontal();
				dstPrefabName = EditorGUILayout.TextField(dstPrefabName);
				GUILayout.Label(Language.GetLanguage(".prefab"));
				EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			
			GUILayout.Label(Language.GetLanguage("Add Collider:"));
			bHasBoxCollider = EditorGUILayout.Toggle(bHasBoxCollider);
			EditorGUILayout.EndHorizontal();
			GUILayout.Label(Language.GetLanguage("Custom Material"));
			custom_mat = EditorGUILayout.ObjectField(custom_mat, typeof(Material), false) as Material;

			EditorGUILayout.Space();  
        	EditorGUILayout.Space();
			EditorGUILayout.Space();  
        	EditorGUILayout.Space();
			if (GUILayout.Button("Create Prefab"))
			{
				bool createResult = CreateSimplePrefab();
				if (createResult == false){
					EditorUtility.DisplayDialog("Creator", string.Format("Creating Failed.\nErrorSource:(CreateSimplePrefab())"), "OK", "Cancel");
				}
				else{
					EditorUtility.DisplayDialog("Creator", string.Format("Success:prefab {0} created.",dstPrefabName), "OK", "Cancel");
				}
				

			}
			EditorGUILayout.HelpBox(Language.GetLanguage("1). First Set up your sprite with Sprite Editor in [Project Window] ") + "\n" + 
									Language.GetLanguage("    Make sure the pivot & sprite are correct in scene display.") + "\n\n" + 
									Language.GetLanguage("2). Drag Sprite on this window and Set pixelPerUnit of your sprite. ") + "\n" + 
									Language.GetLanguage("    Set width of this prefab") + "\n\n" + 
									Language.GetLanguage("3). Create Prefab :XD"), MessageType.Info);
		}
		void UI_LoadTileContents()
		{
			autoGenerateTileBySprite = true;
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(Language.GetLanguage("Select 2D Texture"));
			texture = EditorGUILayout.ObjectField(texture , typeof(Texture2D), false ,spriteFieldOptions) as Texture2D;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(Language.GetLanguage("Auto Generate Tile from Texture2D"));
			EditorGUILayout.EndHorizontal();
			{
				GUILayout.Label("------------------");
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(Language.GetLanguage("Z Order:"));
				zOrder = EditorGUILayout.IntField(zOrder);
				GUILayout.Label(Language.GetLanguage("Tile Tag:"));
				tileTag = EditorGUILayout.TextField(tileTag);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(Language.GetLanguage("Pixel Per Unit:"));
				pixelPerUnit_texture = EditorGUILayout.IntField(pixelPerUnit_texture);
				GUILayout.Label(Language.GetLanguage("Select Tile Size:"));
				GUILayout.Label(Language.GetLanguage("W"));
				EditorGUILayout.IntField(tile_Width);
				GUILayout.Label(Language.GetLanguage("H"));
				
				EditorGUILayout.IntField(tile_Height);
				EditorGUILayout.Space(); 
				EditorGUILayout.Space(); 
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space(); 
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Vector2Field("Pivot:",SpritePivot, pivotFieldOptions);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space(); 
				EditorGUILayout.Space(); 
				if (texture != null && (dstPrefabName == null || dstPrefabName == "") ){
					dstPrefabName = texture.name;
					if (tileTag == "None"){
						tileTag = texture.name;
					}
				}
				GUILayout.Label(Language.GetLanguage("Output Prefab Name:"));
				EditorGUILayout.BeginHorizontal();
				dstPrefabName = EditorGUILayout.TextField(dstPrefabName);
				GUILayout.Label(Language.GetLanguage(".prefab"));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space(); 
				GUILayout.Label("------------------");
				if (GUILayout.Button("Edit Texture")){
					if (texture != null){
						List<SpriteMetaData> sheetData = CreateSpriteSheetAutoTile(texture, tile_Width,tile_Height);
						SaveSpriteSheet(sheetData, texture);
						//For preview spritesheet and re-edit sprites
						OnOpenSpriteEditor(texture);

					}
					else{
						EditorUtility.DisplayDialog("Error", string.Format("Please Select a Texture First!"), "OK", "Cancel");
					}
				}

			}
			
			
			EditorGUILayout.BeginHorizontal();
			
			GUILayout.Label(Language.GetLanguage("Add Collider:"));
			bHasBoxCollider = EditorGUILayout.Toggle(bHasBoxCollider);
			EditorGUILayout.EndHorizontal();
			GUILayout.Label(Language.GetLanguage("Custom Material"));
			custom_mat = EditorGUILayout.ObjectField(custom_mat, typeof(Material), false) as Material;

			EditorGUILayout.Space();  
        	EditorGUILayout.Space();
			EditorGUILayout.Space();  
        	EditorGUILayout.Space();
			if (GUILayout.Button("Create Prefab"))
			{
				bool createResult = CreateTilePrefab();
				if (createResult == false){
					EditorUtility.DisplayDialog("Creator", string.Format("Creating Tile Failed.\nErrorSource:(CreateTilePrefab())"), "OK", "Cancel");
				}
				else{
					EditorUtility.DisplayDialog("Creator", string.Format("Success:prefab {0} created.",dstPrefabName), "OK", "Cancel");
				}
			}

		}
		void UI_LoadJSONContents()
		{
			autoGenerateTileBySprite = false;
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(Language.GetLanguage("Select 2D Texture"));
			texture = EditorGUILayout.ObjectField(texture , typeof(Texture2D), false ,spriteFieldOptions) as Texture2D;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(Language.GetLanguage("Auto Generate Tile from Texture2D and JSON."));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical();
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(Language.GetLanguage("Select spritesheet .json"));
				spritesheet_json = EditorGUILayout.ObjectField(spritesheet_json , typeof(TextAsset), false) as TextAsset;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(Language.GetLanguage("Z Order:"));
				zOrder = EditorGUILayout.IntField(zOrder);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginHorizontal();
			
			GUILayout.Label(Language.GetLanguage("Add Collider:"));
			bHasBoxCollider = EditorGUILayout.Toggle(bHasBoxCollider);
			EditorGUILayout.EndHorizontal();
			GUILayout.Label(Language.GetLanguage("Custom Material"));
			custom_mat = EditorGUILayout.ObjectField(custom_mat, typeof(Material), false) as Material;

			EditorGUILayout.Space();  
        	EditorGUILayout.Space();
			EditorGUILayout.Space();  
        	EditorGUILayout.Space();
			if (GUILayout.Button("Create Prefab"))
			{
				bool createResult = BatchCreatePrefabs();
				if (createResult == false){
					EditorUtility.DisplayDialog("Creator", string.Format("Creating Tile Failed.\nErrorSource:(BatchCreatePrefabs())"), "OK", "Cancel");
				}
				else{
					EditorUtility.DisplayDialog("Creator", string.Format("Success:prefab {0} created.",spritesheet_json.name), "OK", "Cancel");
				}
			}
		}
		bool BatchCreatePrefabs(){
			string input_text = "[]";
			if (spritesheet_json == null){
				Debug.Log("Prefab Creator Error: JSON file Found.");
				return false;
			}
			//TileJsonReader jsonReader = new TileJsonReader();
			TileJsonReader.TextureInfo textureInfo = new TileJsonReader.TextureInfo();
			input_text = spritesheet_json.text;
			bool result = TileJsonReader.ReadJson(input_text, out textureInfo);
			string texture_name_with_ext = Path.GetFileName(AssetDatabase.GetAssetPath(texture));
			
			if (texture_name_with_ext.ToLower() != textureInfo.textureName.ToLower()){
				EditorUtility.DisplayDialog("Error", string.Format("texture name in json not vaild.texture is {0}, but got {1} in json",texture_name_with_ext,textureInfo.textureName), "OK", "Cancel");
				return false;
			}
			if (result == false){
				EditorUtility.DisplayDialog("Error", string.Format("JSON {0} not vaild.",spritesheet_json.name), "OK", "Cancel");
				return false;
			}
			SaveSpriteSheet(LoadSpriteSheetFromClass(textureInfo),texture);
			Debug.Log(string.Format("Got Texture... Reading {0} tiles",textureInfo.tiles.Length));
			foreach (var tileInfo in textureInfo.tiles){
				switch (tileInfo.tileType){
					case "Tile":
						TileChilds tileChilds = new TileChilds();
						tileChilds.AA = new ChildSprites(GetSpritesByName(tileInfo.tileName + "_" + "AANW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "AANE",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "AASW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "AASE",texture));
						tileChilds.CT = new ChildSprites(GetSpritesByName(tileInfo.tileName + "_" + "CTNW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CTNE",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CTSW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CTSE",texture));
						tileChilds.UD = new ChildSprites(GetSpritesByID("UDNW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "UDNE",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "UDSW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "UDSE",texture));
						tileChilds.LR = new ChildSprites(GetSpritesByName(tileInfo.tileName + "_" + "LRNW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "LRNE",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "LRSW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "LRSE",texture));
						tileChilds.CV = new ChildSprites(GetSpritesByName(tileInfo.tileName + "_" + "CVNW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CVNE",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CVSW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CVSE",texture));
						tileChilds.CC = new ChildSprites(GetSpritesByName(tileInfo.tileName + "_" + "CCNW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CCNE",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CCSW",texture),
												GetSpritesByName(tileInfo.tileName + "_" + "CCSE",texture));

						GameObject obj = new GameObject(tileInfo.tileName);
						ST2D_ComponentTiled base_comp = obj.AddComponent<ST2D_ComponentTiled>();
						ST2D_AssistantTiled base_Assistant = obj.AddComponent<ST2D_AssistantTiled>();
						base_comp.pixelPerUnit_texture = textureInfo.pixelPerUnit;
						base_comp.tile_size = tileInfo.tileWidth;
						base_comp.tileChilds = tileChilds;
						base_comp.zOrder = zOrder;
						base_comp.tileTag = tileInfo.tileTag;
						if (custom_mat != null){
							base_comp.mat = custom_mat;
						}
						base_Assistant.prefabTileAlign = (float)tileInfo.tileWidth / (float)textureInfo.pixelPerUnit;
						base_comp.initTileObject();
						if (!System.IO.Directory.Exists(dstDir)){
							System.IO.Directory.CreateDirectory(dstDir);
						}
						if (System.IO.File.Exists(dstDir + tileInfo.tileName + ".prefab"))
						{
							File.Delete(dstDir + tileInfo.tileName + ".prefab");
						}
						PrefabUtility.CreatePrefab(dstDir + tileInfo.tileName + ".prefab", obj);
						GameObject.DestroyImmediate(obj);
						Debug.Log(string.Format("Prefab {0} Created.", dstDir + tileInfo.tileName + ".prefab"));
						break;
					case "Simple":
						GameObject objSimple = new GameObject(tileInfo.tileName);
						ST2D_ComponentSimple base_comp1 = objSimple.AddComponent<ST2D_ComponentSimple>();
						ST2D_AssistantSimple base_ast = objSimple.AddComponent<ST2D_AssistantSimple>();
						base_comp1.m_sprites = GetAllSpritesByTileInfo(tileInfo, texture);
						if (custom_mat != null){
							base_comp1.mat = custom_mat;
						}
						base_comp1.zOrder = zOrder;
						base_comp1.tileTag = tileInfo.tileTag;
						base_ast.prefabTileAlign = (float)tileInfo.tileWidth / (float)textureInfo.pixelPerUnit;
						base_comp1.initSimpleTile();
						if (!System.IO.Directory.Exists(dstDir)){
							System.IO.Directory.CreateDirectory(dstDir);
						}
						if (System.IO.File.Exists(dstDir + tileInfo.tileName + ".prefab"))
						{
							File.Delete(dstDir + tileInfo.tileName + ".prefab");
						}
						PrefabUtility.CreatePrefab(dstDir + tileInfo.tileName + ".prefab", objSimple);
						GameObject.DestroyImmediate(objSimple);
						Debug.Log(string.Format("Prefab {0} Created.", dstDir + tileInfo.tileName + ".prefab"));
						break;
				}

			}
			return true;
		}
		bool CreateSimplePrefab(){
			if ((simpleSprite == null) || (tile_Width == 0 || dstPrefabName == null))
			{	
				Debug.Log("Prefab Creator Error: no sprite or Prefab Name Found.");
				return false;
			}
			GameObject objSimple = new GameObject(dstPrefabName);
			ST2D_ComponentSimple base_comp1 = objSimple.AddComponent<ST2D_ComponentSimple>();
			ST2D_AssistantSimple base_ast = objSimple.AddComponent<ST2D_AssistantSimple>();
			base_comp1.m_sprites = new Sprite[]{simpleSprite};
			if (custom_mat != null){
				base_comp1.mat = custom_mat;
			}
			base_comp1.zOrder = zOrder;
			base_comp1.tileTag = tileTag;
			base_ast.prefabTileAlign = (float)tile_Width / (float)pixelPerUnit_texture;
			base_comp1.initSimpleTile();
			if (!System.IO.Directory.Exists(dstDir)){
						System.IO.Directory.CreateDirectory(dstDir);
			}
			if (System.IO.File.Exists(dstDir + dstPrefabName + ".prefab"))
			{
				Debug.Log(string.Format("Warning: Prefab {0} already exists.",
						(dstDir + dstPrefabName + ".prefab")));
				bool warning = EditorUtility.DisplayDialog("Warning", string.Format("Warning: Prefab {0} already exists.\n OverWrite?",
								(dstDir + dstPrefabName + ".prefab")), "OK", "Cancel");
				if (warning == true){
						File.Delete(dstDir + dstPrefabName + ".prefab");
						PrefabUtility.CreatePrefab(dstDir + dstPrefabName + ".prefab",objSimple);
						GameObject.DestroyImmediate(objSimple);
				}
						
			}
			else{
				PrefabUtility.CreatePrefab(dstDir + dstPrefabName + ".prefab", objSimple);
				GameObject.DestroyImmediate(objSimple);
			}
			Debug.Log(string.Format("Prefab {0} Created.", dstDir + dstPrefabName + ".prefab"));
			return true;
		}
		bool CreateTilePrefab(){
			if ((texture == null)||(dstPrefabName == "")){
				return false;
			}
			if (autoGenerateTileBySprite)		
			{
				// Do not use .json template , generate tiles from texture only.
				// TILE TEXTURE MUST BE LIKE THIS:
				// [SIZE X SIZE] [SIZE X SIZE]
				// [SIZE X SIZE] [SIZE X SIZE]
				// [SIZE X SIZE] [SIZE X SIZE]
				TileChilds tileChilds = new TileChilds();
				tileChilds.AA = new ChildSprites(GetSpritesByID("AANW",texture),
												GetSpritesByID("AANE",texture),
												GetSpritesByID("AASW",texture),
												GetSpritesByID("AASE",texture));
				tileChilds.CT = new ChildSprites(GetSpritesByID("CTNW",texture),
												GetSpritesByID("CTNE",texture),
												GetSpritesByID("CTSW",texture),
												GetSpritesByID("CTSE",texture));
				tileChilds.UD = new ChildSprites(GetSpritesByID("UDNW",texture),
												GetSpritesByID("UDNE",texture),
												GetSpritesByID("UDSW",texture),
												GetSpritesByID("UDSE",texture));
				tileChilds.LR = new ChildSprites(GetSpritesByID("LRNW",texture),
												GetSpritesByID("LRNE",texture),
												GetSpritesByID("LRSW",texture),
												GetSpritesByID("LRSE",texture));
				tileChilds.CV = new ChildSprites(GetSpritesByID("CVNW",texture),
												GetSpritesByID("CVNE",texture),
												GetSpritesByID("CVSW",texture),
												GetSpritesByID("CVSE",texture));
				tileChilds.CC = new ChildSprites(GetSpritesByID("CCNW",texture),
												GetSpritesByID("CCNE",texture),
												GetSpritesByID("CCSW",texture),
												GetSpritesByID("CCSE",texture));

				GameObject obj = new GameObject(dstPrefabName);
				ST2D_ComponentTiled base_comp = obj.AddComponent<ST2D_ComponentTiled>();
				ST2D_AssistantTiled base_Assistant = obj.AddComponent<ST2D_AssistantTiled>();
				base_comp.pixelPerUnit_texture = pixelPerUnit_texture;
				base_comp.tile_size = tile_Width;
				base_comp.tileChilds = tileChilds;
				base_comp.zOrder = zOrder;
				base_comp.tileTag = tileTag;
				if (custom_mat != null){
					base_comp.mat = custom_mat;
				}
				base_Assistant.prefabTileAlign = (float)tile_Width / (float)pixelPerUnit_texture;
				base_comp.initTileObject();
				if (!System.IO.Directory.Exists(dstDir)){
				System.IO.Directory.CreateDirectory(dstDir);
				}
				if (System.IO.File.Exists(dstDir + dstPrefabName + ".prefab")){
					Debug.Log(string.Format("Warning: Prefab {0} already exists.",
							(dstDir + dstPrefabName + ".prefab")));
					bool warning = EditorUtility.DisplayDialog("Warning", string.Format("Warning: Prefab {0} already exists.\n OverWrite?",
							(dstDir + dstPrefabName + ".prefab")), "OK", "Cancel");
					if (warning == true){
						File.Delete(dstDir + dstPrefabName + ".prefab");
						PrefabUtility.CreatePrefab(dstDir + dstPrefabName + ".prefab", obj);
						GameObject.DestroyImmediate(obj);
						return true;
					}
				
				}
				PrefabUtility.CreatePrefab(dstDir + dstPrefabName + ".prefab", obj);
				GameObject.DestroyImmediate(obj);
				return true;
			}
			return false;

		}

		void OnOpenSpriteEditor(Texture2D texture)
 		{
     		Selection.activeObject = texture;
        	//反射获取SpriteEditorWindow
        	var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SpriteEditorWindow");
        	var window = EditorWindow.GetWindow(type);
			window.titleContent = new GUIContent() {text="Custom Sprite Editor"};
			
 		}
		// simple split texture and create sheets
		List<SpriteMetaData> CreateSpriteSheet(Texture2D texture, int tile_Width, int tile_Height, int offset_X, int offset_Y,int padding_X,int padding_Y){
			//create SpriteMetaData by tile width & tile Height
			//Then we can get a mulitple sprite
			List<SpriteMetaData> sheet = new List<SpriteMetaData>();
			string basename = Path.GetFileNameWithoutExtension(texture.name);
			for (int row = 0;row< (texture.height - offset_Y) / (tile_Height + offset_Y);row++){
				for (int col = 0;col<(texture.width - offset_X) / (tile_Width + padding_X);col++){
					var meta = new SpriteMetaData();
					Rect r = new Rect();
					r.width = tile_Width;
					r.height = tile_Height;
					r.x = offset_X + col * tile_Width + padding_X;
					r.y = texture.height - (offset_Y + row * tile_Height + padding_Y)  - r.height;
					meta.rect = r;
					meta.name = string.Format("{0}_{1}_{2}", basename, col,row);
					meta.pivot = SpritePivot;
					sheet.Add(meta);
				}
			}
			return sheet;
		}

		//create a tiled sheet, child sprites are named with specified id
		List<SpriteMetaData> CreateSpriteSheetAutoTile(Texture2D texture, int tile_Width, int tile_Height){
			/* CVNW CVNE CCNW CCNE
			 * CVSW CVSE CCSW CCSE
			 * CVNW LRNE LRNW CVNE
			 * UDSW CTSE CTSW UDSE
			 * UDNW CTNE CTNW UDNE
			 * CVSW LRSE LRSW CVSE
			 */
			string basename = Path.GetFileNameWithoutExtension(texture.name);
			string[,] nameTable = new string[,] {
				{"AANW","AANE","CCNW","CCNE"},
				{"AASW","AASE","CCSW","CCSE"},
				{"CVNW","UDNE","UDNW","CVNE"},
				{"LRSW","CTSE","CTSW","LRSE"},
				{"LRNW","CTNE","CTNW","LRNE"},
				{"CVSW","UDSE","UDSW","CVSE"}
			};
			List<SpriteMetaData> sheet = new List<SpriteMetaData>();
			if ((texture.width < tile_Width  * 2) || (texture.height < tile_Height * 3)){
				Debug.Log("[ST2D Prefab Creator]Error width/height, can't generate the mulitple sprite");
				return sheet;
			}
			if (tile_Width % 2 != 0 || tile_Height % 2 != 0){
				Debug.Log("[ST2D Prefab Creator]Error tile width/height, you can't set a odd number for tile width/height");
			}
			for (int row = 0;row < (texture.height * 2 / tile_Height);row++){
				for (int col = 0;col <(texture.width * 2 / tile_Width);col++){
					var meta = new SpriteMetaData();
					Rect r = new Rect();
					r.width = tile_Width / 2;
					r.height = tile_Height / 2;
					r.x = col * tile_Width / 2;
					r.y = texture.height - (row * tile_Height/2) - r.height;
					meta.rect = r;
					meta.name = string.Format("{0}_{1}{2}_{3}", basename,col,row, nameTable[row,col]);
					meta.pivot = SpritePivot;
					sheet.Add(meta);
				}
			}
			return sheet;
		}



		//Load sheet info from json file
		List<SpriteMetaData> LoadSpriteSheetFromClass(TileJsonReader.TextureInfo textureInfo)
		{
			
			List<SpriteMetaData> sheet = new List<SpriteMetaData>();
			foreach (var tileInfo in textureInfo.tiles){
				foreach (var c in tileInfo.spriteMetaData){
					var meta = new SpriteMetaData();
					Rect r = new Rect();
					r.width = c.width;
					r.height = c.height;
					r.x = c.x;
					r.y = textureInfo.Height -  c.y - r.height;
					meta.rect = r;
					meta.name = tileInfo.tileName + "_" + c.name;
					meta.pivot = new Vector2(c.pivotX, c.pivotY);
					sheet.Add(meta);
				}
			}
			return sheet;
		}
		void SaveSpriteSheet( List<SpriteMetaData> sheet, Texture2D texture )
    	{
        	string path = AssetDatabase.GetAssetPath( texture );
        	TextureImporter importer = AssetImporter.GetAtPath( path ) as TextureImporter;
        	importer.spritesheet = sheet.ToArray();
        	importer.textureType = TextureImporterType.Sprite;

        	TextureImporterSettings settings = new TextureImporterSettings();
        	importer.ReadTextureSettings( settings );
			settings.spriteMeshType = SpriteMeshType.FullRect;
			settings.spritePixelsPerUnit = pixelPerUnit_texture;
        	settings.mipmapEnabled = false;
			settings.spriteMeshType = SpriteMeshType.FullRect;
			settings.alphaIsTransparency = true;
        	settings.filterMode = FilterMode.Point; //force set to point
        	settings.spriteMode = (int) SpriteImportMode.Multiple;
        	importer.SetTextureSettings( settings );

        	AssetDatabase.ImportAsset( path, ImportAssetOptions.ForceUpdate );
    	}

		Sprite[] GetAllSpritesByTileInfo(TileJsonReader.TileInfo tile,  Texture2D texture){
			List<Sprite> result = new List<Sprite>();
			List<string> names = new List<string>();
			foreach (var meta in tile.spriteMetaData){
				names.Add(tile.tileName + "_" + meta.name);
			}
			Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));
			foreach (var tex in sprites){

				var t = tex.name;
				if (names.Contains(t)){
					result.Add((Sprite)tex);
				}
			}
			return result.ToArray();
		}
		Sprite[] GetSpritesByName(string spriteName, Texture2D texture){
			List<Sprite> result = new List<Sprite>();
			Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));
			foreach (var tex in sprites){

				var t = tex.name;
				if (t == spriteName){
					result.Add((Sprite)tex);
				}
			}
			return result.ToArray();
		}

		Sprite[] GetSpritesByID(string tileID, Texture2D texture)
		{
			List<Sprite> result = new List<Sprite>();
			Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));
			foreach (var tex in sprites){

				var t = tex.name.Split('_');
				string _id = t[t.GetUpperBound(0)];
				if (_id == tileID){
					result.Add((Sprite)tex);
				}
			}
			return result.ToArray();
		}
		int toolBarselected = 0;

	}

}

