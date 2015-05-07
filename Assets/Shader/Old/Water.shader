Shader "Querijn/TextureShift" 
{
    Properties 
    {
        g_Texture("Base (RGB)", 2D) = "white" {}
        g_Direction("Direction", Range(0, 359)) = 45
        g_TextureStrength("Texture Strength", Range(0, 1)) = 0.2
        g_Texture2("Base (RGB)", 2D) = "white" {}
        g_Direction2("Direction", Range(0, 359)) = 135
        g_TextureStrength2("Texture Strength", Range(0, 1)) = 0.2
        g_Speed("Speed", float) = 1
    }
    
    SubShader 
    {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 200
        
        CGPROGRAM
        #pragma surface SurfaceShader Lambert
        
        sampler2D g_Texture;
        sampler2D g_Texture2;
        float     g_Direction;
        float     g_Direction2;
        float     g_TextureStrength;
        float     g_TextureStrength2;
        float     g_Speed;
        
        struct Input 
        {
            float2 uvg_Texture;
            float2 uvg_Texture2;
        };
        
        void SurfaceShader(Input a_Input, inout SurfaceOutput a_Output) 
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
            a_Output.Albedo = t_Colour.rgb;
			a_Output.Alpha = 0.5;	
        }
        ENDCG
    }
    FallBack "Transparent/VertexLit"
}