Shader "ST2D/SpriteLightShader" 
{
	Properties 
	{
		_MainTex("Diffuse Texture", 2D) = "white" {}
		_NormalDepth ("Normal Map", 2D) = "bump" {}
	}
	SubShader 
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		AlphaTest NotEqual 0.0
		Pass
		{
			CGPROGRAM
			#pragma fragment frag 
			#pragma vertex vert_img
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform sampler2D _MainTex;
			
			fixed4 frag(v2f_img i) : SV_Target
			{
				fixed4 input = tex2D(_MainTex, i.uv);
				return input;
			}

		ENDCG 
		}
	}
	Fallback "Transparent/Diffuse"
}
