Shader "SupGames/Mobile/Blur"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "" {}
		_MaskTex("Base (RGB)", 2D) = "white" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata 
	{
		fixed4 pos : POSITION;
		fixed2 uv : TEXCOORD0;
	};

	struct v2f
	{
		fixed4 pos : SV_POSITION;
		fixed2  uv : TEXCOORD0;
	};

	struct v2fb
	{
		fixed4 pos : SV_POSITION;
		fixed4  uv : TEXCOORD0;
#if defined(KERNEL)
		fixed4  uv1 : TEXCOORD1;
#endif
	};

	sampler2D _MainTex;
	sampler2D _MaskTex;
	sampler2D _BlurTex;
	fixed4 _MainTex_TexelSize;
	fixed _BlurAmount;

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.pos);
		o.uv = v.uv;
		return o;
	}

	v2fb vertb(appdata v)
	{
		v2fb o;
		o.pos = UnityObjectToClipPos(v.pos);
		fixed2 offset = (_MainTex_TexelSize.xy) * _BlurAmount;
		o.uv = fixed4(v.uv - offset, v.uv + offset);
#if defined(KERNEL)
		offset *= 2.0h;
		o.uv1 = fixed4(v.uv - offset, v.uv + offset);
#endif
		return o;
	}

	fixed4 fragb(v2fb i) : COLOR
	{
		fixed4 result = tex2D(_MainTex, i.uv.xy);
		result += tex2D(_MainTex, i.uv.xw);
		result += tex2D(_MainTex, i.uv.zy);
		result += tex2D(_MainTex, i.uv.zw);
#if defined(KERNEL)
		result += tex2D(_MainTex, i.uv1.xy);
		result += tex2D(_MainTex, i.uv1.xw);
		result += tex2D(_MainTex, i.uv1.zy);
		result += tex2D(_MainTex, i.uv1.zw);
		return result * 0.125h;;
#endif
		return result * 0.25h;
	}

	fixed4 frag(v2f i) : COLOR
	{
		fixed4 c = tex2D(_MainTex, i.uv);
		fixed4 b = tex2D(_BlurTex, i.uv);
		fixed4 m = tex2D(_MaskTex, i.uv);
		return lerp(c, b, m.r);
	}

	ENDCG

	Subshader
	{
		Pass //0
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			CGPROGRAM
			#pragma shader_feature KERNEL
			#pragma vertex vertb
			#pragma fragment fragb
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
		Pass //1
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
	}
	Fallback off
}