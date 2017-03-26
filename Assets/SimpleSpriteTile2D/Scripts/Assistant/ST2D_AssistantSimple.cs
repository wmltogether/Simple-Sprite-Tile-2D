using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace moogle.SmartTile2D
{
	[RequireComponent (typeof(ST2D_ComponentSimple))]
	[ExecuteInEditMode]
	public class ST2D_AssistantSimple : ST2D_AssistantBase 
	{
		// prefab的宽度，默认设定为1 unit，每个tile预设的中心间隔是1;
		// pixelperunit sprite默认的ppu，对于像素游戏可以设置的更小
		//public int prefabPixelPerUnit = 100;
		public bool hideChildTiles = true;

		public float prefabTileAlign = 1.0f;

		private Vector2 oldPosition;

		void OnEnable()
		{
			oldPosition = new Vector2 (Mathf.Round (this.transform.localPosition.x), Mathf.Round (this.transform.localPosition.y));
		}

		void OnDisable()
		{
			
		}

		void Start () 
		{
		
		}
	
		// Update is called once per frame
		void Update () 
		{
			if (!Application.isPlaying){
				PositionAlignment();
				if (ComparePosition(transform.localPosition, oldPosition,prefabTileAlign) == false)
				{
					oldPosition.x = transform.localPosition.x;
					oldPosition.y = transform.localPosition.y;	
				}
			}
		}
		private void PositionAlignment()
		{
			Vector3 pos = transform.localPosition;

			if (prefabTileAlign > 0) 
			{
				var dstX = prefabTileAlign * Mathf.Round (pos.x / prefabTileAlign);
				var dstY = prefabTileAlign * Mathf.Round (pos.y / prefabTileAlign);
				var dstZ = prefabTileAlign * Mathf.Round (pos.z / prefabTileAlign);
				Vector3 fixed_pos = new Vector3(
					dstX,dstY,dstZ
				);
				transform.localPosition = FixNaN(fixed_pos);
				

			}
		}

		
		void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0,(float)1f,(float)0.75f,(float)0.3f);
			Gizmos.DrawWireCube(transform.position,transform.localScale * prefabTileAlign);
			var from = transform.position - new Vector3((prefabTileAlign / (float)2),
														(prefabTileAlign / (float)2)
														,0);
										
			Gizmos.DrawRay(from,transform.localScale * prefabTileAlign);
			
		}

		void HideChildsInHierarchy()
		{
			foreach (var subRender in transform.GetComponentsInChildren<Renderer>())
			{	
				var subObject = subRender.gameObject;
				subObject.hideFlags = HideFlags.HideInHierarchy;
			}
		}
		void ShowChildsInHierarchy()
		{
			foreach (var subRender in transform.GetComponentsInChildren<Renderer>())
			{	
				var subObject = subRender.gameObject;
				subObject.hideFlags = HideFlags.None;
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(transform.position,transform.localScale * prefabTileAlign);
		}
	}	
}

