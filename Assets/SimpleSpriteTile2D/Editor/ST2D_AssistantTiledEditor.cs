using UnityEngine;
using UnityEditor;
using System.Collections;
using moogle.SmartTile2D.TileManager;
using UnityEngine.Events;

namespace moogle.SmartTile2D
{
	[CanEditMultipleObjects]
    [CustomEditor(typeof(ST2D_AssistantTiled))]
    [ExecuteInEditMode]
	public class ST2D_AssistantTiledEditor : Editor  {

		private static GUIStyle style;
		void getProperty(){
			obj = new SerializedObject(target);
            prefabTileAlign = obj.FindProperty("prefabTileAlign");


		}
		public void Awake(){
            style = new GUIStyle(EditorStyles.label);
			style.fontSize = 14;
			style.fontStyle = FontStyle.Bold;

        }
		void OnEnable()
        {
            getProperty();
        }
		public override void OnInspectorGUI(){
			obj.Update();
			GUILayout.Label("[Prefab Movement Assistant]");
            EditorGUILayout.PropertyField(prefabTileAlign);
			EditorGUILayout.Space(); 
            EditorGUILayout.Space(); 
			obj.ApplyModifiedProperties();

		}

		private SerializedObject obj;
		public SerializedProperty prefabTileAlign;
	}
}

