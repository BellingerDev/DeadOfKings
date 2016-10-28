Shader "Custom/SimpleCardShader"
{
	Properties
	{
		_Diffuse ("Diffuse", 2D) = "white" {}
	}

	SubShader
	{
		Tags 
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
			"LightMode" = "ForwardBase" 
		}

		LOD 100

		Pass
		{
			Blend One OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
		
			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 diff : COLOR0;
				float2 uv0 : TEXCOORD0;
			};

			sampler2D _Diffuse;
			float4 _Diffuse_ST;
			
			v2f vert (appdata_full i)
			{
				v2f o;

				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				o.uv0 = TRANSFORM_TEX(i.texcoord, _Diffuse);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 diffuse = tex2D(_Diffuse, i.uv0);

				fixed4 final;

				final.rgb = diffuse.rgb;
				final.a = diffuse.a;

				return final;
			}
			ENDCG
		}
	}
}
