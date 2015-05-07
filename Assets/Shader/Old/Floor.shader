Shader "Querijn/FeedbackOLD" 
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		g_LeftColor ("Leftside Color", Color) = (1,1,1,1)
		g_RightColor ("Rightside Color", Color) = (0,1,0,1)
		g_LeftSize("LSize", Float) = 0.0
		g_RightSize("RSize", Float) = 0.0
		g_Tiling("Tiling", Float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 g_LeftColor;
		fixed4 g_RightColor;
		float g_LeftSize;
		float g_RightSize;
		float g_Tiling;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			o.Albedo = c.rgb;
			
			float2 t_Dist = IN.uv_MainTex-float2(0.5*g_Tiling,0.5*g_Tiling);
			
			float t_Distance = t_Dist.x*t_Dist.x+t_Dist.y*t_Dist.y;
			float t_Size = (IN.uv_MainTex.x<0.5*g_Tiling)?(g_LeftSize):(g_RightSize);
			float t_MaxDist = t_Size*(0.2*0.2)*g_Tiling*4;
			
			if(t_Distance<t_MaxDist)
			{
				t_Distance /= t_MaxDist+0.0001f;
				
				fixed4 t_Color = (IN.uv_MainTex.x>0.5*g_Tiling)?(g_LeftColor):(g_RightColor);
				
				//o.Albedo.r += t_Color.r*(1-t_Distance);
				//o.Albedo.g += t_Color.g*(1-t_Distance);
				//o.Albedo.b += t_Color.b*(1-t_Distance);
				
				o.Albedo.r = t_Color.r*(1-t_Distance)+(o.Albedo.r*t_Distance);
				o.Albedo.g = t_Color.g*(1-t_Distance)+(o.Albedo.g*t_Distance);
				o.Albedo.b = t_Color.b*(1-t_Distance)+(o.Albedo.b*t_Distance);
				o.Alpha = c.a*(t_Distance);
			}
			else o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
