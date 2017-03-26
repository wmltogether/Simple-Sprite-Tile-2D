using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moogle.SmartTile2D.TileManager;
using UnityEditor;

namespace moogle.SmartTile2D
{
	

	public class ST2D_AssistantBase : MonoBehaviour 
	{
	 	public virtual bool ComparePosition(Vector3 a, Vector2 b, float align = 1f){
			if (Mathf.RoundToInt(a.x / align) != Mathf.RoundToInt(b.x / align)) return false;
			if (Mathf.RoundToInt(a.y / align) != Mathf.RoundToInt(b.y / align)) return false;
			return true;
		}
		public virtual bool ComparePosition(Vector2 a, Vector2 b, float align = 1f){
			if (Mathf.RoundToInt(a.x / align) != Mathf.RoundToInt(b.x / align)) return false;
			if (Mathf.RoundToInt(a.y / align) != Mathf.RoundToInt(b.y / align)) return false;
			return true;
		}

		protected Vector3 FixNaN(Vector3 input)
		{
			Vector3 result = input;
			if (float.IsNaN(result.x))
			{
				result.x = 0;
			}
			if (float.IsNaN(result.y))
			{
				result.y = 0;
			}
			if (float.IsNaN(result.z))
			{
				result.z = 0;
			}
			return result;
		}

		
	}
	
}
