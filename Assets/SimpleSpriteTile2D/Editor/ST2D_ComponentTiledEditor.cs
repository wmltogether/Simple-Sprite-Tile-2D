using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace moogle.SmartTile2D
{
	[CanEditMultipleObjects]
    [CustomEditor(typeof(ST2D_ComponentTiled))]
    [ExecuteInEditMode]
	public class ST2D_ComponentTiledEditor : Editor 
	{

		private static GUIStyle style;
		void getProperty(){
			obj = new SerializedObject(target);
            tileTag = obj.FindProperty("tileTag");
		    zOrder = obj.FindProperty("zOrder");
			tileChilds = obj.FindProperty("tileChilds");
			pixelPerUnit_texture = obj.FindProperty("pixelPerUnit_texture");
			tile_size = obj.FindProperty("tile_size");
			pivot = obj.FindProperty("pivot");

		}
		public void Awake(){
            style = new GUIStyle(EditorStyles.label);
			style.fontSize = 18;
			style.fontStyle = FontStyle.Bold;

        }
		void OnEnable()
        {
            getProperty();
        }
		public override void OnInspectorGUI(){
			obj.Update();
			GUILayout.Label("[Prefab Type]:Tile");
            EditorGUILayout.PropertyField(tileTag);
            EditorGUILayout.PropertyField(zOrder);
			EditorGUILayout.Space(); 
            EditorGUILayout.Space(); 
			EditorGUILayout.HelpBox(string.Format("PixelPerUnit:{0}\n\nTile Size:{1}\n\n",
									pixelPerUnit_texture.intValue, 
									tile_size.intValue
									), MessageType.None);
			EditorGUILayout.PropertyField(tileChilds,true);

			obj.ApplyModifiedProperties();

		}
		

		private SerializedObject obj;
        public SerializedProperty tileTag;
		public SerializedProperty zOrder;

		public SerializedProperty tileChilds;
		public SerializedProperty pixelPerUnit_texture;

		public SerializedProperty tile_size;
		public SerializedProperty pivot;

	}

}
