using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using moogle.SmartTile2D.TileManager;

namespace moogle.SmartTile2D
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ST2D_TilePanel))]
    [ExecuteInEditMode]

    public class ST2D_TilePanelEditor : Editor
    {
        public static GUIContent[] sortingLayerDisplay = new GUIContent[] {};
        
        public static int[] sortingLayerUniqueIDs= new int[]{};
        void getProperty()
        {
            obj = new SerializedObject(target);
            sortingOrderOffset = obj.FindProperty("sortingOrderOffset");
            ForceSortOrderByYAxis = obj.FindProperty("ForceSortOrderByYAxis");
            sortingLayerIndex = obj.FindProperty("sortingLayerIndex");
        }

        void OnEnable()
        {
            getProperty();
            InitTileSortingLayer();
        }
        public override void OnInspectorGUI()
        {
            InitTileSortingLayer();
            DrawCustomInspector();
        }
        public void DrawCustomInspector()
        {
            obj.Update();
            EditorGUILayout.PropertyField(sortingOrderOffset);
            GUILayout.Label("Set Sorting Layer of this panel:");
            EditorGUILayout.IntPopup(sortingLayerIndex,sortingLayerDisplay,sortingLayerUniqueIDs);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.Label("__________________");
            EditorGUILayout.PropertyField(ForceSortOrderByYAxis);
            obj.ApplyModifiedProperties();
            if (ForceSortOrderByYAxis.boolValue){

            
            if (GUILayout.Button(Language.GetLanguage("Update Order"))) SetOrder();
            if (GUILayout.Button(Language.GetLanguage("Recover Order"))) SetOrderByPrefab();
            EditorGUILayout.HelpBox(Language.GetLanguage("All sorting orders in this panel will be rewritten by Y Axis,When you press [Update Order].\n") +
                                    Language.GetLanguage("If you still want to use the original sorting orders defined in prefab,\n Please Press [Recover Order]"), MessageType.Info);
            }
        }

        void InitTileSortingLayer(){
            List<GUIContent> _displaySLayerNames = new List<GUIContent>(){};
            List<int> _sortingLayerUniqueIDs = new List<int>();
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty m_SortingLayers = tagManager.FindProperty("m_SortingLayers");
            for (int i = 0; i < m_SortingLayers.arraySize; i++)
            {
                SerializedProperty t = m_SortingLayers.GetArrayElementAtIndex(i);
                _displaySLayerNames.Add (new GUIContent(){text=t.FindPropertyRelative("name").stringValue});
                _sortingLayerUniqueIDs.Add(t.FindPropertyRelative("uniqueID").intValue);
            }
            sortingLayerDisplay = _displaySLayerNames.ToArray();
            sortingLayerUniqueIDs = _sortingLayerUniqueIDs.ToArray();
            
        }
       
        void SetOrder()
        {
            if (((ST2D_TilePanel)target).ForceSortOrderByYAxis == true)
            {
                ((ST2D_TilePanel)target).ReSortOrderByY();
            }
            else
            {
                SetOrderByPrefab();
            }
        }
        void SetOrderByPrefab()
        {
            ((ST2D_TilePanel)target).ReSortOrderByPrefab();
        }
        private SerializedObject obj;
        public SerializedProperty ForceSortOrderByYAxis;

        public SerializedProperty sortingOrderOffset;
        public SerializedProperty sortingLayerIndex;
    }

}
