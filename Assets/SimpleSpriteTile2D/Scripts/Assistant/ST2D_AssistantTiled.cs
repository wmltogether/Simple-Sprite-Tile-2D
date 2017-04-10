using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moogle.SmartTile2D.TileManager;
using UnityEditor;

namespace moogle.SmartTile2D
{
	[RequireComponent (typeof(ST2D_ComponentTiled))]
	[ExecuteInEditMode]
	public class ST2D_AssistantTiled : ST2D_AssistantBase 
	{
		
		public float prefabTileAlign = 1.0f;

		private Vector2 oldPosition;

		private TileInfo tileInfo;

		public void UnlockChildsInHierarchy(){
			foreach (var subRender in transform.GetComponentsInChildren<Renderer>())
			{	
				var subObject = subRender.gameObject;
				if (TileBase.subIDSet.Contains(subObject.name)) 
				{
					subObject.hideFlags = HideFlags.None;
				}
			}
		}
		public void LockChildsInHierarchy(){
			foreach (var subRender in transform.GetComponentsInChildren<Renderer>())
			{	
				var subObject = subRender.gameObject;
				if (TileBase.subIDSet.Contains(subObject.name)) 
				{
					subObject.hideFlags = HideFlags.NotEditable;
				}
			}
		}
		void OnEnable()
		{
			
			oldPosition = new Vector2 (Mathf.Round (this.transform.localPosition.x), Mathf.Round (this.transform.localPosition.y));

			var comp = this.transform.gameObject.GetComponent<ST2D_ComponentTiled>();
			
			tileInfo = new TileInfo(){xy=oldPosition,tile_Component=comp,layerName = GetLayer()};
			//Debug.Log("Adding Tile to Scene (Source:OnEnable)");
			ST2D_TileManager.Instance.AddTile(tileInfo);
			LockChildsInHierarchy();
			
		}

		string GetLayer(){
			string layer = "Default";
			if (transform.parent != null){
				var panel = this.transform.parent.GetComponent<ST2D_TilePanel>();
				
				if (panel != null){
					layer = panel.sortingLayerIndex.ToString();
				}
			}
			return layer;
		}

		void OnDisable()
		{
			ST2D_TileManager.Instance.RemoveTile(tileInfo);
		}

		void OnDestroy()
		{
			ST2D_TileManager.Instance.RemoveTile(tileInfo);
		}
		// Use this for initialization
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
					//tile的位置已经移动，进行对齐操作
					//tile位置变动后自动匹配子tile的图片
					//Debug.Log(string.Format("Position changed:{0}<-{1}",transform.localPosition,oldPosition));
					oldPosition.x = transform.localPosition.x;
					oldPosition.y = transform.localPosition.y;

					DoTileUpdateByTag();	
				}
			}
		}

		
		private void DoTileUpdateByNeighbor()
		{
			//只更新周围的9宫格tile，其他不做更新

			var comp = this.transform.gameObject.GetComponent<ST2D_ComponentTiled>();
			var dstPos = new Vector2(){x=transform.localPosition.x,y=transform.localPosition.y};

			ST2D_TileManager.Instance.MoveTile(tileInfo , dstPos);
			tileInfo.xy = dstPos;
			tileInfo.layerName = GetLayer();
			ST2D_TileManager.Instance.UpdateTileByNeighbor(tileInfo.xy, comp.tileTag,prefabTileAlign);
			//ST2D_TileManager.Instance.UpdateAllTileByTag(comp.tileTag, prefabTileAlign);
		}
		private void DoTileUpdateByTag()
		{
			//更新tag下的全部tile
			var comp = transform.gameObject.GetComponent<ST2D_ComponentTiled>();
			//DO reload sprites
			var dstPos = new Vector2(){x=transform.localPosition.x,y=transform.localPosition.y};
			ST2D_TileManager.Instance.MoveTile(tileInfo , dstPos);
			tileInfo.xy = dstPos;
			tileInfo.layerName = GetLayer();
			//ST2D_TileManager.Instance.UpdateTileByNeighbor(tileInfo.xy, comp.tileTag,prefabTileAlign);
			ST2D_TileManager.Instance.UpdateAllTileByTag(comp.tileTag, prefabTileAlign);

			
		}
			
		private void PositionAlignment()
		{
			Vector3 pos = transform.localPosition;

			if (prefabTileAlign > 0) 
			{
				var dstX = prefabTileAlign * Mathf.Round (pos.x / prefabTileAlign);
				var dstY = prefabTileAlign * Mathf.Round (pos.y / prefabTileAlign);
				var dstZ = pos.z;
				Vector3 fixed_pos = new Vector3(
					dstX,dstY,dstZ
				);
				transform.localPosition = FixNaN(fixed_pos);
				

			}
		}

		
		void OnDrawGizmosSelected()
		{
			var m_position = transform.position;

			Gizmos.DrawWireCube(m_position,transform.localScale * prefabTileAlign);
			var from = m_position - new Vector3((prefabTileAlign / (float)2),
														(prefabTileAlign / (float)2)
														,0);
			//Add icon at localPosition 0,0
										
			Gizmos.DrawRay(from,transform.localScale * prefabTileAlign);
			
		}
		void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(transform.position,transform.localScale * prefabTileAlign);
		}

		
	}


}
