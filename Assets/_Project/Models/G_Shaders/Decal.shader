Shader "Custom/WallDecalShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Blend("Blend", Range(0, 1)) = 0.5
    }
        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float3 worldNormal : TEXCOORD1;
                    float3 worldTangent : TEXCOORD2;
                    float3 worldBitangent : TEXCOORD3;
                    float4 screenPos : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float _Blend;

                v2f vert(appdata v) {
                    v2f o;
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                    o.worldBitangent = cross(o.worldNormal, o.worldTangent) * v.tangent.w;
                    o.screenPos = UnityObjectToClipPos(v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float3 worldNormal = normalize(i.worldNormal);
                    float3 worldTangent = normalize(i.worldTangent - dot(i.worldTangent, worldNormal) * worldNormal);
                    float3 worldBitangent = cross(worldNormal, worldTangent);

                    float3 viewDir = normalize(UnityWorldSpaceViewDir(i.screenPos));
                    float3 reflectionDir = reflect(-viewDir, worldNormal);
                    float3 diffuseColor = tex2D(_MainTex, i.uv).rgb;

                    float3 projectedView = viewDir - dot(viewDir, worldNormal) * worldNormal;
                    float2 projectedUV = float2(dot(projectedView, worldTangent), dot(projectedView, worldBitangent));

                    return fixed4(diffuseColor * _Color.rgb, _Color.a) * lerp(0, 1, tex2D(_MainTex, projectedUV));
                }
                ENDCG
            }

           
        }
            FallBack "Diffuse"
}
