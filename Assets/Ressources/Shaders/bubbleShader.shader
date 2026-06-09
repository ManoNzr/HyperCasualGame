Shader "Custom/UltimateSoapBubble"
{
    Properties
    {
        [Header(Style Visuel)]
        [Toggle] _UsePixel ("Activer le mode Pixel Art", Float) = 1.0
        
        [Header(Couleurs et Membrane de Savon)]
        _BaseColor ("Couleur Transparente (Centre)", Color) = (0.1, 0.4, 0.8, 0.05)
        _SoapColor1 ("Couleur Savon 1 (ex: Rose)", Color) = (1.0, 0.2, 0.8, 1.0)
        _SoapColor2 ("Couleur Savon 2 (ex: Cyan)", Color) = (0.2, 1.0, 0.8, 1.0)
        _SoapSpeed ("Vitesse du fluide", Range(0.1, 5.0)) = 1.5
        
        [Header(Lumiere Reelle et Reflets)]
        _Glossiness ("Taille du Reflet (Lisse)", Range(10, 200)) = 80
        _SpecIntensity ("Intensite du Reflet", Range(0, 5)) = 2.0
        _SmoothAlpha ("Opacite des bords (Mode Lisse)", Range(0, 1)) = 0.3
        
        [Header(Reglages Pixel Art)]
        _PixelScale ("Taille des Pixels", Range(1, 10)) = 4.0
        _DitherDensity ("Densite de la bulle", Range(0.5, 3.0)) = 1.2
        
        [Header(Physique (Script))]
        _Deformation ("Force de Deformation", Range(0, 0.1)) = 0.02
        _Velocity ("Velocite", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : NORMAL;
                float3 localPos : TEXCOORD2;
            };

            float _UsePixel;
            float4 _BaseColor;
            float4 _SoapColor1;
            float4 _SoapColor2;
            float _SoapSpeed;
            float _Glossiness;
            float _SpecIntensity;
            float _SmoothAlpha;
            float _PixelScale;
            float _DitherDensity;
            float _Deformation;
            float4 _Velocity;

            v2f vert (appdata v)
            {
                v2f o;
                o.localPos = v.vertex.xyz;

                // Inertie et déformation
                float3 localVelocity = mul(unity_WorldToObject, float4(_Velocity.xyz, 0)).xyz;
                float speed = length(localVelocity);

                if (speed > 0.01) 
                {
                    float3 dir = normalize(localVelocity);
                    float alignment = dot(v.normal, dir); 
                    v.vertex.xyz -= v.normal * abs(alignment) * speed * _Deformation;
                }
                // Respiration
                v.vertex.xyz += v.normal * sin(_Time.y * 4.0 + v.vertex.y * 2.0) * 0.015;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.screenPos = ComputeScreenPos(o.pos);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                i.viewDir = normalize(i.viewDir);

                // Lumière directionnelle principale
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                
                // Fresnel
                float fresnel = 1.0 - saturate(dot(i.viewDir, i.normal));
                
                // Effet fluide de savon
                float swirl = sin(i.localPos.x * 10.0 + _Time.y * _SoapSpeed) * cos(i.localPos.y * 8.0 - _Time.y * _SoapSpeed * 0.8);
                float soapFactor = saturate(fresnel * 0.5 + swirl * 0.3 + 0.2);
                float3 soapColor = lerp(_SoapColor1.rgb, _SoapColor2.rgb, soapFactor);

                // Reflet spéculaire
                float3 halfVector = normalize(lightDir + i.viewDir);
                float NdotH = saturate(dot(i.normal, halfVector));
                
                fixed4 finalColor = _BaseColor;
                float intensity = 0;

                if (_UsePixel > 0.5) 
                {
                    // --- MODE PIXEL ART ---
                    float2 screenUV = i.screenPos.xy / i.screenPos.w;
                    float2 pixelPos = screenUV * _ScreenParams.xy;
                    
                    int x = (int)fmod(floor(pixelPos.x / _PixelScale), 4.0);
                    int y = (int)fmod(floor(pixelPos.y / _PixelScale), 4.0);

                    float dither[16] = {
                         0.0,  8.0,  2.0, 10.0,
                        12.0,  4.0, 14.0,  6.0,
                         3.0, 11.0,  1.0,  9.0,
                        15.0,  7.0, 13.0,  5.0
                    };
                    float ditherValue = dither[x + y * 4] / 16.0;

                    float specPixel = step(0.95, NdotH);
                    intensity = pow(fresnel, 2.0) * _DitherDensity + specPixel * _SpecIntensity + 0.05;
                    
                    clip(intensity - ditherValue);
                    
                    finalColor.rgb = lerp(soapColor, fixed3(1,1,1), specPixel);
                    finalColor.a = 1.0;
                }
                else 
                {
                    // --- MODE LISSE AVEC TRANSPARENCE AJUSTABLE ---
                    float specSmooth = pow(NdotH, _Glossiness);
                    
                    finalColor.rgb = lerp(_BaseColor.rgb, soapColor, fresnel);
                    finalColor.rgb += specSmooth * _SpecIntensity * fixed3(1,1,1);
                    
                    // _SmoothAlpha contrôle l'impact des reflets colorés sur l'opacité globale
                    finalColor.a = saturate(_BaseColor.a + (pow(fresnel, 3.0) * _SmoothAlpha) + (specSmooth * 0.4));
                }

                return finalColor;
            }
            ENDCG
        }
    }
}
// je t'aime loic