Shader "Custom/HyperCasualBubble"
{
    Properties
    {
        _Color ("Couleur Principale", Color) = (0.4, 0.8, 1.0, 0.3)
        _RimColor ("Couleur du Contour", Color) = (1.0, 1.0, 1.0, 1.0)
        _RimPower ("Épaisseur du Contour", Range(0.1, 8.0)) = 2.5
        _Deformation ("Force de Déformation", Range(0, 0.1)) = 0.02
        _Velocity ("Vélocité (Géré par le script)", Vector) = (0,0,0,0)
    }
    SubShader
    {
        // Configuration pour la transparence
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
                float3 viewDir : TEXCOORD0;
                float3 normal : NORMAL;
            };

            float4 _Color;
            float4 _RimColor;
            float _RimPower;
            float _Deformation;
            float4 _Velocity;

            v2f vert (appdata v)
            {
                v2f o;

                // 1. Convertir la vélocité globale en vélocité locale
                float3 localVelocity = mul(unity_WorldToObject, float4(_Velocity.xyz, 0)).xyz;
                float speed = length(localVelocity);

                // 2. Déformation liée au momentum (inertie)
                if (speed > 0.01) 
                {
                    float3 dir = normalize(localVelocity);
                    // On écrase légèrement l'avant et l'arrière de la bulle pendant le mouvement
                    float alignment = dot(v.normal, dir); 
                    v.vertex.xyz -= v.normal * abs(alignment) * speed * _Deformation;
                }

                // 3. Petit effet "Idle Wobble" (la bulle respire même à l'arrêt)
                v.vertex.xyz += v.normal * sin(_Time.y * 4.0 + v.vertex.y * 2.0) * 0.015;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                i.viewDir = normalize(i.viewDir);

                // Calcul de l'effet Fresnel (les bords sont plus visibles que le centre)
                float rim = 1.0 - saturate(dot(i.viewDir, i.normal));
                rim = pow(rim, _RimPower);

                // Mélange des couleurs
                fixed4 col = _Color;
                col.rgb += _RimColor.rgb * rim; // Ajoute la brillance sur les bords
                col.a = saturate(_Color.a + rim); // Rend les bords plus opaques

                return col;
            }
            ENDCG
        }
    }
}