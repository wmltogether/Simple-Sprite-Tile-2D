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
        public static GUIContent[] sortingLayerDisplay = new GUIContent[] {
            new GUIContent(){text="SLayer1"},
            new GUIContent(){text="SLayer2"},
            new GUIContent(){text="SLayer3"},
            new GUIContent(){text="SLayer4"},
            new GUIContent(){text="SLayer5"},
            new GUIContent(){text="SLayer6"},
            new GUIContent(){text="SLayer7"},
            new GUIContent(){text="SLayer8"},
            new GUIContent(){text="SLayer9"}

        };
        public static int[] sortingLayerUniqueIDs= new int[] {1,2,3,4,5,6,7,8,9};
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
        }
        public override void OnInspectorGUI()
        {
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

        void AddSortingLayer(string sortingLayerName,int uniqueID){
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty m_SortingLayers = tagManager.FindProperty("m_SortingLayers");
            bool found = false;
            for (int i = 0; i < m_SortingLayers.arraySize; i++)
            {
                SerializedProperty t = m_SortingLayers.GetArrayElementAtIndex(i);
                if (t.FindPropertyRelative("name").stringValue.Equals(sortingLayerName))
                {
                    found = true;
                    t.FindPropertyRelative("uniqueID").longValue = (long)uniqueID;
                    break;
                }
            }
            if (!found)
            {
                m_SortingLayers.InsertArrayElementAtIndex(1);
                SerializedProperty cur = m_SortingLayers.GetArrayElementAtIndex(1);
                if (cur != null)
                {
                    cur.FindPropertyRelative("name").stringValue = sortingLayerName;
                    cur.FindPropertyRelative("uniqueID").longValue = (long)uniqueID;
                }
            }
            tagManager.ApplyModifiedProperties();


        }
        void SetOrder()
        {

            AddSortingLayer(sortingLayerDisplay[sortingLayerIndex.intValue - 1].text,sortingLayerIndex.intValue);
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
