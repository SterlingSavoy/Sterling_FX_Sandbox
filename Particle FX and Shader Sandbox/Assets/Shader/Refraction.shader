// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33741,y:32670,varname:node_3138,prsc:2|emission-6665-RGB;n:type:ShaderForge.SFN_SceneColor,id:6665,x:33547,y:32834,varname:node_6665,prsc:2|UVIN-7670-OUT;n:type:ShaderForge.SFN_NormalVector,id:9850,x:32380,y:32714,prsc:2,pt:False;n:type:ShaderForge.SFN_Negate,id:1721,x:32557,y:32714,varname:node_1721,prsc:2|IN-9850-OUT;n:type:ShaderForge.SFN_Transform,id:1772,x:32760,y:32714,varname:node_1772,prsc:2,tffrom:1,tfto:3|IN-1721-OUT;n:type:ShaderForge.SFN_ComponentMask,id:668,x:32946,y:32714,varname:node_668,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-1772-XYZ;n:type:ShaderForge.SFN_ScreenPos,id:1048,x:33003,y:32938,varname:node_1048,prsc:2,sctp:2;n:type:ShaderForge.SFN_Add,id:7670,x:33212,y:32906,varname:node_7670,prsc:2|A-1138-OUT,B-1048-UVOUT;n:type:ShaderForge.SFN_Multiply,id:1138,x:33212,y:32697,varname:node_1138,prsc:2|A-668-OUT,B-3673-OUT;n:type:ShaderForge.SFN_Slider,id:522,x:32157,y:33020,ptovrint:False,ptlb:Freshnel Exponent,ptin:_FreshnelExponent,varname:node_522,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.9370514,max:10;n:type:ShaderForge.SFN_OneMinus,id:6505,x:32633,y:33006,varname:node_6505,prsc:2|IN-6447-OUT;n:type:ShaderForge.SFN_Fresnel,id:6447,x:32472,y:33006,varname:node_6447,prsc:2|EXP-522-OUT;n:type:ShaderForge.SFN_Power,id:3673,x:32813,y:33006,varname:node_3673,prsc:2|VAL-6505-OUT,EXP-755-OUT;n:type:ShaderForge.SFN_ValueProperty,id:755,x:32236,y:32923,ptovrint:False,ptlb:Value,ptin:_Value,varname:node_755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;proporder:522-755;pass:END;sub:END;*/

Shader "Shader Forge/Refraction" {
    Properties {
        _FreshnelExponent ("Freshnel Exponent", Range(0, 10)) = 0.9370514
        _Value ("Value", Float ) = 2
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _FreshnelExponent;
            uniform float _Value;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 projPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float3 emissive = tex2D( _GrabTexture, ((UnityObjectToViewPos( float4((-1*i.normalDir),0) ).xyz.rgb.rg*pow((1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_FreshnelExponent)),_Value))+sceneUVs.rg)).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
