Shader "Hidden/STS_red_mask_RT" {

	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	SubShader
	{
	    Tags { "RenderType"="Opaque" }
	    Cull Off
	    Lighting Off
		ZWrite On	
	    Pass 
	    {
	    Fog { Mode Off }
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		

		struct v2f 
		{
		    float4 pos : POSITION;    
		};
		
		float _STSObstDepthShift;

		float4 vert(float4 v:POSITION) : SV_POSITION {
			float4 retv = mul (UNITY_MATRIX_MVP, v);			
			return retv;
            }

		half4 frag() : COLOR 
		{ 
			return fixed4(1.0,0.0,0.0,0.0);
		}
		
		ENDCG
	    }
	}
	
	SubShader 
	{
	
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	
	Lighting Off
	Cull Off
	ZWrite Off	
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _alphaDiscard;			
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				if (col.a < _alphaDiscard) discard;
				return fixed4(1.0,0.0,0.0,0.0);
			}
		ENDCG
	}
} 

	
	SubShader 
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}		

		ZWrite On
		Lighting Off
		Cull Off		

		Pass 
		{  
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed _Cutoff;
			
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = tex2D(_MainTex, i.texcoord);
					if (col.a < _Cutoff) discard;
					return fixed4(1.0,0.0,0.0,0.0);
				}
			ENDCG
		}
	}
}