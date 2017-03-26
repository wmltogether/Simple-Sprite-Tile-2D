using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using moogle.SmartTile2D.TileManager;
namespace moogle.SmartTile2D
{
	[CanEditMultipleObjects]
    [CustomEditor(typeof(ST2D_ComponentSimple))]
    [ExecuteInEditMode]
	public class ST2D_ComponentSimpleEditor : Editor 
	{

		private static GUIStyle style;
		void getProperty(){
			obj = new SerializedObject(target);
            tileTag = obj.FindProperty("tileTag");
		    zOrder = obj.FindProperty("zOrder");
			m_sprites = obj.FindProperty("m_sprites");


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
			GUILayout.Label("[Prefab Type]:Simple");
            EditorGUILayout.PropertyField(tileTag);
            EditorGUILayout.PropertyField(zOrder);
			EditorGUILayout.Space(); 
            EditorGUILayout.Space(); 
			EditorGUILayout.PropertyField(m_sprites,true);
			obj.ApplyModifiedProperties();

		}

		private SerializedObject obj;
        public SerializedProperty tileTag;
		public SerializedProperty zOrder;

		public SerializedProperty m_sprites;

	}

}
