Shader "Effect/Eff_MultiplyDouble"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend One OneMinusSrcAlpha 
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
	
		// ---- Single texture cards (not entirely correct)
		SubShader
		{
			Pass
			{
				SetTexture [_MainTex]
				{
					combine texture * primary
				}
				SetTexture [_MainTex]
				{
					constantColor [_TintColor]
					combine constant * previous
				}
			}
		}
	}
}
