Shader "Querijn/Feedback" 
{
	Properties 
	{
        g_OriginalUV("DONT CHANGE PLEASE", 2D) = "white" {}
        g_Texture("Base (RGB)", 2D) = "white" {}
        g_Direction("Direction", Range(0, 359)) = 45
        g_TextureStrength("Texture Strength", Range(0, 1)) = 0.2
        g_Texture2("Base (RGB)", 2D) = "white" {}
        g_Direction2("Direction", Range(0, 359)) = 135
        g_TextureStrength2("Texture Strength", Range(0, 1)) = 0.2
        g_Speed("Speed", float) = 1
   
		g_LeftColor ("Leftside Color", Color) = (1,1,1,1)
		g_RightColor ("Rightside Color", Color) = (0,1,0,1)
		g_LeftSize("LSize", Float) = 0.0
		g_RightSize("RSize", Float) = 0.0
	}
	
	SubShader 
	{
		//Tags { "RenderType"="Transparent" }
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0
		
        sampler2D g_Texture;
        sampler2D g_OriginalUV;
        sampler2D g_Texture2;
        float     g_Direction;
        float     g_Direction2;
        float     g_TextureStrength;
        float     g_TextureStrength2;
        float     g_Speed;
		fixed4 g_LeftColor;
		fixed4 g_RightColor;
		float g_LeftSize;
		float g_RightSize;

		struct Input 
        {
            float2 uvg_OriginalUV;
            float2 uvg_Texture;
            float2 uvg_Texture2;
        };

		void surf (Input a_Input, inout SurfaceOutput o) 
		{
		    float2 t_UV = a_Input.uvg_Texture;
            float2 t_UV2 = a_Input.uvg_Texture2;
            float2 t_Dir = float2(1.0, 0.0);
            
            float t_Angle = 0.0174532925*g_Direction;
            float t_Sin = sin(t_Angle), t_Cos = cos(t_Angle);
            t_Dir.x -= 0.5; t_Dir.y -= 0.5;
            t_Dir.x = t_Dir.x * t_Cos - t_Dir.y * t_Sin;
            t_Dir.y = t_Dir.x * t_Sin + t_Dir.y * t_Cos;
            t_Dir.x += 0.5; t_Dir.y += 0.5; t_UV += t_Dir*_Time.x*g_Speed;
           
            t_Dir = float2(1.0, 0.0);
            t_Angle = 0.0174532925*g_Direction2;
            t_Sin = sin(t_Angle); t_Cos = cos(t_Angle);
            t_Dir.x -= 0.5; t_Dir.y -= 0.5;
            t_Dir.x = t_Dir.x * t_Cos - t_Dir.y * t_Sin;
            t_Dir.y = t_Dir.x * t_Sin + t_Dir.y * t_Cos;
            t_Dir.x += 0.5; t_Dir.y += 0.5; t_UV2 += t_Dir*_Time.x*g_Speed;
            
            half4 t_Colour = tex2D(g_Texture, t_UV)*g_TextureStrength+tex2D(g_Texture2, t_UV2)*g_TextureStrength2;
			
			o.Albedo = t_Colour.rgb;
			
			float2 t_Dist = a_Input.uvg_OriginalUV-float2(0.5,0.5);
			
			float t_Distance = t_Dist.x*t_Dist.x+t_Dist.y*t_Dist.y;
			float t_Size = (a_Input.uvg_OriginalUV.x<0.5)?(g_LeftSize):(g_RightSize);
			float t_MaxDist = t_Size*(0.2*0.2);
			
			if(t_Distance<t_MaxDist)
			{
				t_Distance /= t_MaxDist+0.0001f;
				
				fixed4 t_Colour2 = (a_Input.uvg_OriginalUV.x>0.5)?(g_LeftColor):(g_RightColor);
				
				//o.Albedo.r += t_Colour2.r*(1-t_Distance);
				//o.Albedo.g += t_Colour2.g*(1-t_Distance);
				//o.Albedo.b += t_Colour2.b*(1-t_Distance);
				
				
				o.Albedo.r = t_Colour2.r*(1-t_Distance)+(o.Albedo.r*t_Distance);
				o.Albedo.g = t_Colour2.g*(1-t_Distance)+(o.Albedo.g*t_Distance);
				o.Albedo.b = t_Colour2.b*(1-t_Distance)+(o.Albedo.b*t_Distance);
				
				o.Alpha = t_Colour2.a*(t_Distance);
			}
			o.Alpha = 0.5;	
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
