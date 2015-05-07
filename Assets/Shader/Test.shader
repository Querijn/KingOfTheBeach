Shader "Querijn/Test" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
         #include "UnityCG.cginc"

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
		    float3 rgb = clamp( abs(fmod(IN.uv_MainTex.x*1.6*6.0+float3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );
		    
			//rgb = rgb*rgb*(3.0-2.0*rgb); // cubic smoothing	
			
			half4 c = half4(rgb.x, rgb.y, rgb.z, 1.0f);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
