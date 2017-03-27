using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace moogle.SmartTile2D
{
		public class ST2D_TilePanel : MonoBehaviour {
        public bool ForceSortOrderByYAxis = false;
        [HideInInspector]
        public string[] sortingLayerNames = new string[] {"Default","SLayer1","SLayer2","SLayer3",
                                                        "SLayer4","SLayer5","SLayer6",
                                                        "SLayer7","SLayer8","SLayer9"};
        public int sortingOrderOffset = 0;
        public int sortingLayerIndex = 1;

        // Use this for initialization
        void Start () 
        {
            SetSortingLayerID(sortingLayerIndex);
            if (ForceSortOrderByYAxis)
            {
                //强制根据Y轴来排序SortOrder
                ReSortOrderByY(sortingOrderOffset);
            }
        }
	
		// Update is called once per frame
		void Update () {
		
		}

        void SetSortingLayerID(int sortingLayerIndex){
            var renders = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renders)
            {
                r.sortingLayerID = sortingLayerIndex;
            }
        }
        float GetLowerOffset()
        {
            float lowerOffset = 0;
            List<float> y_offsets = new List<float>();
            //Resort Panel by Y -> Z
            var renders = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renders)
            {
                y_offsets.Add(r.transform.position.y);
            }
            y_offsets.Sort();
            if (y_offsets.Count >0)
            {
                lowerOffset = y_offsets[0];
                return lowerOffset;
            }
            return lowerOffset;
        }

        public void ReSortOrderByY(float lowerOffset=0f)
        {
            
            var renders = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renders)
            {
                r.sortingOrder = -Mathf.RoundToInt((r.transform.position.y - lowerOffset));
                r.sortingLayerID = sortingLayerIndex;
            }
        }

        

        public void ReSortOrderByPrefab()
        {
            var tile_com = transform.GetComponentsInChildren<ST2D_ComponentTiled>();
            foreach (var r in tile_com)
            {
                r.ResetSortingOrder();
            }
            var tile_com2 = transform.GetComponentsInChildren<ST2D_ComponentSimple>();
            foreach (var r in tile_com2)
            {
                r.ResetSortingOrder();
            }
        }

    }	
}

