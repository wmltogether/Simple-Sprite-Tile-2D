Shader "ST2DShader/2DDiffuseShader" {
	Properties {
		
		_MainTex ("Texture2D", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_BumpIntensity ("NormalMap Intensity", Range (-1, 2)) = 1
        _BumpIntensity ("NormalMap Intensity", Float) = 1
		_Alpha ("Alpha", Range(0,1)) = 1

	}
	SubShader {
		Tags { 
			"Queue" = "AlphaTest"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
			 }
		LOD 300
		Cull Off
		Zwrite Off
		BlendOp Add
		Blend Off
		Fog { Mode Off }

		CGPROGRAM

		#pragma surface surf Lambert vertex:vert nofog keepalpha addshadow fullforwardshadows
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed _BumpIntensity;
		fixed4 _Color;
		fixed _Alpha;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			fixed4 color;
		};

		void vert(inout appdata_full v, out Input o)
		{
			v.vertex = UnityPixelSnap(v.vertex);
			v.normal = float3(0,0,-1);
            v.tangent =  float4(1, 0, 0, 1);
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color;
		}

		void surf(Input input, inout SurfaceOutput output)
		{
			float2 p = input.uv_MainTex;
			fixed4 c = tex2D(_MainTex, p) * input.color;
			c.a = c.a - _Alpha;
			output.Albedo = c.rgb ;
			output.Normal = UnpackNormal(tex2D(_BumpMap, input.uv_BumpMap));
            _BumpIntensity = 1 / _BumpIntensity;
            output.Normal.z = output.Normal.z * _BumpIntensity;
            output.Normal = normalize((half3)output.Normal);
		}
		ENDCG
	}
	FallBack "Sprites/Default"
}
