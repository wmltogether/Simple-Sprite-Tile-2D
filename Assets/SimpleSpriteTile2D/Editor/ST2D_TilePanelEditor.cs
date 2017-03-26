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
        void getProperty()
        {
            obj = new SerializedObject(target);
            sortingOrderOffset = obj.FindProperty("sortingOrderOffset");
            ForceSortOrderByYAxis = obj.FindProperty("ForceSortOrderByYAxis");
            layerName = obj.FindProperty("layerName");
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
            GUILayout.Label("Set Layer Name of this panel:");
            EditorGUILayout.PropertyField(layerName);
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
        public SerializedProperty layerName;
    }

}
