using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
namespace moogle.SmartTile2D{
	public class TileJsonReader: TileJsonBase
	{
		public static bool ReadJson(string textasset,out TextureInfo tileDatas)
		{
			tileDatas = new TextureInfo();
			try
			{
				tileDatas = JsonConvert.DeserializeObject<TextureInfo>(textasset);
				if (tileDatas == null){
					return false;
				}
				return true;
			}
			catch (Exception ex){
				Console.WriteLine("Error: {0}",ex.ToString());
				//Debug.LogException(ex);
		}
		return false;
	}

		public static string WriteJSON(System.Object tileDatas)
		{
			string result = "";
			result = JsonConvert.SerializeObject(tileDatas,Formatting.Indented);
			return result;
		}
	}

}
