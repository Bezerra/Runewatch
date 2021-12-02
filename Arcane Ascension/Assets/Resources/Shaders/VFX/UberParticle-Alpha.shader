// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "D4/Particles/UberParticle-Alpha"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin][Toggle]_UseDepth("Use Depth", Float) = 1
		_DepthDistance("Depth Distance", Float) = 1
		[Header(Main Texture)]_MainTexture("Main Texture", 2D) = "white" {}
		_MainTextureSpeed("Main Texture Speed", Vector) = (0,0,0,0)
		[HDR]_Color("Color", Color) = (1,1,1,1)
		[Header(Distort)]_DistortTexture("Distort Texture", 2D) = "white" {}
		_DistortSpeed("Distort Speed", Vector) = (0,0,0,0)
		_DistortStrength("Distort Strength", Float) = 0
		[Header(Dissolve)]_DissolveTexture("Dissolve Texture", 2D) = "black" {}
		[Toggle]_DissolveSmooth("Dissolve Smooth", Float) = 0
		_DissolveMask("Dissolve Mask", 2D) = "black" {}
		[Toggle]_InvertDissolveMask("Invert Dissolve Mask", Float) = 0
		_DissolveSpeed("Dissolve Speed", Vector) = (0,0,0,0)
		[Toggle]_UseCustomDissolveTex0z("Use Custom Dissolve (Tex0.z)", Float) = 0
		_DissolveAmount("Dissolve Amount", Range( 0 , 1)) = 0
		[Header(Mask)]_MaskTexture("Mask Texture", 2D) = "white" {}
		_MaskSpeed("Mask Speed", Vector) = (0,0,0,0)
		[Toggle]_UseMaskAlpha("Use Mask Alpha", Float) = 0
		[Toggle]_InvertMask("Invert Mask", Float) = 0
		[Header(Vertex Offset)]_VertexOffsetTexture("Vertex Offset Texture", 2D) = "white" {}
		_VertexOffsetSpeed("Vertex Offset Speed", Vector) = (0,0,0,0)
		_OffsetStrength("Offset Strength", Float) = 0
		_OffsetMaskTexture("Offset Mask Texture", 2D) = "white" {}
		_OffsetMaskSpeed("Offset Mask Speed", Vector) = (0,0,0,0)
		[Toggle]_UseOffsetMaskAlpha("Use Offset Mask Alpha", Float) = 0
		[ASEEnd][Toggle]_InvertOffsetMask("Invert Offset  Mask", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull Back
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 2.0

		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS

		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 110000
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_COLOR


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DissolveTexture_ST;
			float4 _VertexOffsetTexture_ST;
			float4 _MaskTexture_ST;
			float4 _DissolveMask_ST;
			float4 _DistortTexture_ST;
			float4 _OffsetMaskTexture_ST;
			float4 _Color;
			float4 _MainTexture_ST;
			float2 _DistortSpeed;
			float2 _MainTextureSpeed;
			float2 _OffsetMaskSpeed;
			float2 _MaskSpeed;
			float2 _DissolveSpeed;
			float2 _VertexOffsetSpeed;
			float _UseMaskAlpha;
			float _InvertMask;
			float _DissolveAmount;
			float _UseCustomDissolveTex0z;
			float _DistortStrength;
			float _DissolveSmooth;
			float _UseDepth;
			float _UseOffsetMaskAlpha;
			float _InvertOffsetMask;
			float _OffsetStrength;
			float _InvertDissolveMask;
			float _DepthDistance;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _VertexOffsetTexture;
			sampler2D _OffsetMaskTexture;
			sampler2D _MainTexture;
			sampler2D _DistortTexture;
			sampler2D _DissolveMask;
			sampler2D _DissolveTexture;
			sampler2D _MaskTexture;
			uniform float4 _CameraDepthTexture_TexelSize;


						
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 uv_VertexOffsetTexture = v.ase_texcoord * _VertexOffsetTexture_ST.xy + _VertexOffsetTexture_ST.zw;
				float2 panner2_g121 = ( 1.0 * _Time.y * _VertexOffsetSpeed + uv_VertexOffsetTexture);
				float2 uv_OffsetMaskTexture = v.ase_texcoord.xy * _OffsetMaskTexture_ST.xy + _OffsetMaskTexture_ST.zw;
				float2 panner2_g124 = ( 1.0 * _Time.y * _OffsetMaskSpeed + uv_OffsetMaskTexture);
				float4 temp_output_105_0_g1 = tex2Dlod( _OffsetMaskTexture, float4( ( panner2_g124 + float2( 0,0 ) ), 0, 0.0) );
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord3 = v.ase_texcoord;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( v.ase_normal * (( ( tex2Dlod( _VertexOffsetTexture, float4( ( panner2_g121 + float2( 0,0 ) ), 0, 0.0) ) * _OffsetStrength ) * (( _InvertOffsetMask )?( ( 1.0 - (( _UseOffsetMaskAlpha )?( temp_output_105_0_g1.a ):( (temp_output_105_0_g1).r )) ) ):( (( _UseOffsetMaskAlpha )?( temp_output_105_0_g1.a ):( (temp_output_105_0_g1).r )) )) )).r );
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				float2 uv_MainTexture = IN.ase_texcoord3.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float2 panner2_g126 = ( 1.0 * _Time.y * _MainTextureSpeed + uv_MainTexture);
				float2 uv_DistortTexture = IN.ase_texcoord3.xy * _DistortTexture_ST.xy + _DistortTexture_ST.zw;
				float2 panner2_g127 = ( 1.0 * _Time.y * _DistortSpeed + uv_DistortTexture);
				float2 temp_cast_0 = (( (tex2D( _DistortTexture, ( panner2_g127 + float2( 0,0 ) ) )).r * _DistortStrength )).xx;
				float4 temp_output_9_0_g125 = ( IN.ase_color * _Color * tex2D( _MainTexture, ( panner2_g126 + temp_cast_0 ) ) );
				
				float2 uv_DissolveMask = IN.ase_texcoord3.xy * _DissolveMask_ST.xy + _DissolveMask_ST.zw;
				float4 tex2DNode77_g1 = tex2D( _DissolveMask, uv_DissolveMask );
				float2 uv_DissolveTexture = IN.ase_texcoord3.xy * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
				float2 panner2_g122 = ( 1.0 * _Time.y * _DissolveSpeed + uv_DissolveTexture);
				float clampResult85_g1 = clamp( ( (( _InvertDissolveMask )?( ( 1.0 - tex2DNode77_g1.r ) ):( tex2DNode77_g1.r )) + (tex2D( _DissolveTexture, ( panner2_g122 + float2( 0,0 ) ) )).r ) , 0.0 , 0.99 );
				float4 texCoord56_g1 = IN.ase_texcoord3;
				texCoord56_g1.xy = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult91_g1 = clamp( ( clampResult85_g1 - (-1.0 + ((( _UseCustomDissolveTex0z )?( texCoord56_g1.z ):( _DissolveAmount )) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
				float Dissolve31_g1 = (( _DissolveSmooth )?( clampResult91_g1 ):( step( ( 1.0 - clampResult85_g1 ) , ( 1.0 - (( _UseCustomDissolveTex0z )?( texCoord56_g1.z ):( _DissolveAmount )) ) ) ));
				float2 uv_MaskTexture = IN.ase_texcoord3.xy * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
				float2 panner2_g123 = ( 1.0 * _Time.y * _MaskSpeed + uv_MaskTexture);
				float4 temp_output_57_0_g1 = tex2D( _MaskTexture, ( panner2_g123 + float2( 0,0 ) ) );
				float Mask29_g1 = (( _InvertMask )?( ( 1.0 - (( _UseMaskAlpha )?( temp_output_57_0_g1.a ):( (temp_output_57_0_g1).r )) ) ):( (( _UseMaskAlpha )?( temp_output_57_0_g1.a ):( (temp_output_57_0_g1).r )) ));
				float4 screenPos = IN.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth114_g1 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth114_g1 = saturate( abs( ( screenDepth114_g1 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _DepthDistance ) ) );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = (temp_output_9_0_g125).rgb;
				float Alpha = ( temp_output_9_0_g125.a * Dissolve31_g1 * Mask29_g1 * (( _UseDepth )?( distanceDepth114_g1 ):( 1.0 )) );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 110000
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DissolveTexture_ST;
			float4 _VertexOffsetTexture_ST;
			float4 _MaskTexture_ST;
			float4 _DissolveMask_ST;
			float4 _DistortTexture_ST;
			float4 _OffsetMaskTexture_ST;
			float4 _Color;
			float4 _MainTexture_ST;
			float2 _DistortSpeed;
			float2 _MainTextureSpeed;
			float2 _OffsetMaskSpeed;
			float2 _MaskSpeed;
			float2 _DissolveSpeed;
			float2 _VertexOffsetSpeed;
			float _UseMaskAlpha;
			float _InvertMask;
			float _DissolveAmount;
			float _UseCustomDissolveTex0z;
			float _DistortStrength;
			float _DissolveSmooth;
			float _UseDepth;
			float _UseOffsetMaskAlpha;
			float _InvertOffsetMask;
			float _OffsetStrength;
			float _InvertDissolveMask;
			float _DepthDistance;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _VertexOffsetTexture;
			sampler2D _OffsetMaskTexture;
			sampler2D _MainTexture;
			sampler2D _DistortTexture;
			sampler2D _DissolveMask;
			sampler2D _DissolveTexture;
			sampler2D _MaskTexture;
			uniform float4 _CameraDepthTexture_TexelSize;


			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float2 uv_VertexOffsetTexture = v.ase_texcoord * _VertexOffsetTexture_ST.xy + _VertexOffsetTexture_ST.zw;
				float2 panner2_g121 = ( 1.0 * _Time.y * _VertexOffsetSpeed + uv_VertexOffsetTexture);
				float2 uv_OffsetMaskTexture = v.ase_texcoord.xy * _OffsetMaskTexture_ST.xy + _OffsetMaskTexture_ST.zw;
				float2 panner2_g124 = ( 1.0 * _Time.y * _OffsetMaskSpeed + uv_OffsetMaskTexture);
				float4 temp_output_105_0_g1 = tex2Dlod( _OffsetMaskTexture, float4( ( panner2_g124 + float2( 0,0 ) ), 0, 0.0) );
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord2 = v.ase_texcoord;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( v.ase_normal * (( ( tex2Dlod( _VertexOffsetTexture, float4( ( panner2_g121 + float2( 0,0 ) ), 0, 0.0) ) * _OffsetStrength ) * (( _InvertOffsetMask )?( ( 1.0 - (( _UseOffsetMaskAlpha )?( temp_output_105_0_g1.a ):( (temp_output_105_0_g1).r )) ) ):( (( _UseOffsetMaskAlpha )?( temp_output_105_0_g1.a ):( (temp_output_105_0_g1).r )) )) )).r );
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;

				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_MainTexture = IN.ase_texcoord2.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float2 panner2_g126 = ( 1.0 * _Time.y * _MainTextureSpeed + uv_MainTexture);
				float2 uv_DistortTexture = IN.ase_texcoord2.xy * _DistortTexture_ST.xy + _DistortTexture_ST.zw;
				float2 panner2_g127 = ( 1.0 * _Time.y * _DistortSpeed + uv_DistortTexture);
				float2 temp_cast_0 = (( (tex2D( _DistortTexture, ( panner2_g127 + float2( 0,0 ) ) )).r * _DistortStrength )).xx;
				float4 temp_output_9_0_g125 = ( IN.ase_color * _Color * tex2D( _MainTexture, ( panner2_g126 + temp_cast_0 ) ) );
				float2 uv_DissolveMask = IN.ase_texcoord2.xy * _DissolveMask_ST.xy + _DissolveMask_ST.zw;
				float4 tex2DNode77_g1 = tex2D( _DissolveMask, uv_DissolveMask );
				float2 uv_DissolveTexture = IN.ase_texcoord2.xy * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
				float2 panner2_g122 = ( 1.0 * _Time.y * _DissolveSpeed + uv_DissolveTexture);
				float clampResult85_g1 = clamp( ( (( _InvertDissolveMask )?( ( 1.0 - tex2DNode77_g1.r ) ):( tex2DNode77_g1.r )) + (tex2D( _DissolveTexture, ( panner2_g122 + float2( 0,0 ) ) )).r ) , 0.0 , 0.99 );
				float4 texCoord56_g1 = IN.ase_texcoord2;
				texCoord56_g1.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult91_g1 = clamp( ( clampResult85_g1 - (-1.0 + ((( _UseCustomDissolveTex0z )?( texCoord56_g1.z ):( _DissolveAmount )) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
				float Dissolve31_g1 = (( _DissolveSmooth )?( clampResult91_g1 ):( step( ( 1.0 - clampResult85_g1 ) , ( 1.0 - (( _UseCustomDissolveTex0z )?( texCoord56_g1.z ):( _DissolveAmount )) ) ) ));
				float2 uv_MaskTexture = IN.ase_texcoord2.xy * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
				float2 panner2_g123 = ( 1.0 * _Time.y * _MaskSpeed + uv_MaskTexture);
				float4 temp_output_57_0_g1 = tex2D( _MaskTexture, ( panner2_g123 + float2( 0,0 ) ) );
				float Mask29_g1 = (( _InvertMask )?( ( 1.0 - (( _UseMaskAlpha )?( temp_output_57_0_g1.a ):( (temp_output_57_0_g1).r )) ) ):( (( _UseMaskAlpha )?( temp_output_57_0_g1.a ):( (temp_output_57_0_g1).r )) ));
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth114_g1 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth114_g1 = saturate( abs( ( screenDepth114_g1 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _DepthDistance ) ) );
				
				float Alpha = ( temp_output_9_0_g125.a * Dissolve31_g1 * Mask29_g1 * (( _UseDepth )?( distanceDepth114_g1 ):( 1.0 )) );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 110000
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DissolveTexture_ST;
			float4 _VertexOffsetTexture_ST;
			float4 _MaskTexture_ST;
			float4 _DissolveMask_ST;
			float4 _DistortTexture_ST;
			float4 _OffsetMaskTexture_ST;
			float4 _Color;
			float4 _MainTexture_ST;
			float2 _DistortSpeed;
			float2 _MainTextureSpeed;
			float2 _OffsetMaskSpeed;
			float2 _MaskSpeed;
			float2 _DissolveSpeed;
			float2 _VertexOffsetSpeed;
			float _UseMaskAlpha;
			float _InvertMask;
			float _DissolveAmount;
			float _UseCustomDissolveTex0z;
			float _DistortStrength;
			float _DissolveSmooth;
			float _UseDepth;
			float _UseOffsetMaskAlpha;
			float _InvertOffsetMask;
			float _OffsetStrength;
			float _InvertDissolveMask;
			float _DepthDistance;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _VertexOffsetTexture;
			sampler2D _OffsetMaskTexture;
			sampler2D _MainTexture;
			sampler2D _DistortTexture;
			sampler2D _DissolveMask;
			sampler2D _DissolveTexture;
			sampler2D _MaskTexture;
			uniform float4 _CameraDepthTexture_TexelSize;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 uv_VertexOffsetTexture = v.ase_texcoord * _VertexOffsetTexture_ST.xy + _VertexOffsetTexture_ST.zw;
				float2 panner2_g121 = ( 1.0 * _Time.y * _VertexOffsetSpeed + uv_VertexOffsetTexture);
				float2 uv_OffsetMaskTexture = v.ase_texcoord.xy * _OffsetMaskTexture_ST.xy + _OffsetMaskTexture_ST.zw;
				float2 panner2_g124 = ( 1.0 * _Time.y * _OffsetMaskSpeed + uv_OffsetMaskTexture);
				float4 temp_output_105_0_g1 = tex2Dlod( _OffsetMaskTexture, float4( ( panner2_g124 + float2( 0,0 ) ), 0, 0.0) );
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord2 = v.ase_texcoord;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( v.ase_normal * (( ( tex2Dlod( _VertexOffsetTexture, float4( ( panner2_g121 + float2( 0,0 ) ), 0, 0.0) ) * _OffsetStrength ) * (( _InvertOffsetMask )?( ( 1.0 - (( _UseOffsetMaskAlpha )?( temp_output_105_0_g1.a ):( (temp_output_105_0_g1).r )) ) ):( (( _UseOffsetMaskAlpha )?( temp_output_105_0_g1.a ):( (temp_output_105_0_g1).r )) )) )).r );
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_MainTexture = IN.ase_texcoord2.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float2 panner2_g126 = ( 1.0 * _Time.y * _MainTextureSpeed + uv_MainTexture);
				float2 uv_DistortTexture = IN.ase_texcoord2.xy * _DistortTexture_ST.xy + _DistortTexture_ST.zw;
				float2 panner2_g127 = ( 1.0 * _Time.y * _DistortSpeed + uv_DistortTexture);
				float2 temp_cast_0 = (( (tex2D( _DistortTexture, ( panner2_g127 + float2( 0,0 ) ) )).r * _DistortStrength )).xx;
				float4 temp_output_9_0_g125 = ( IN.ase_color * _Color * tex2D( _MainTexture, ( panner2_g126 + temp_cast_0 ) ) );
				float2 uv_DissolveMask = IN.ase_texcoord2.xy * _DissolveMask_ST.xy + _DissolveMask_ST.zw;
				float4 tex2DNode77_g1 = tex2D( _DissolveMask, uv_DissolveMask );
				float2 uv_DissolveTexture = IN.ase_texcoord2.xy * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
				float2 panner2_g122 = ( 1.0 * _Time.y * _DissolveSpeed + uv_DissolveTexture);
				float clampResult85_g1 = clamp( ( (( _InvertDissolveMask )?( ( 1.0 - tex2DNode77_g1.r ) ):( tex2DNode77_g1.r )) + (tex2D( _DissolveTexture, ( panner2_g122 + float2( 0,0 ) ) )).r ) , 0.0 , 0.99 );
				float4 texCoord56_g1 = IN.ase_texcoord2;
				texCoord56_g1.xy = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult91_g1 = clamp( ( clampResult85_g1 - (-1.0 + ((( _UseCustomDissolveTex0z )?( texCoord56_g1.z ):( _DissolveAmount )) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
				float Dissolve31_g1 = (( _DissolveSmooth )?( clampResult91_g1 ):( step( ( 1.0 - clampResult85_g1 ) , ( 1.0 - (( _UseCustomDissolveTex0z )?( texCoord56_g1.z ):( _DissolveAmount )) ) ) ));
				float2 uv_MaskTexture = IN.ase_texcoord2.xy * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
				float2 panner2_g123 = ( 1.0 * _Time.y * _MaskSpeed + uv_MaskTexture);
				float4 temp_output_57_0_g1 = tex2D( _MaskTexture, ( panner2_g123 + float2( 0,0 ) ) );
				float Mask29_g1 = (( _InvertMask )?( ( 1.0 - (( _UseMaskAlpha )?( temp_output_57_0_g1.a ):( (temp_output_57_0_g1).r )) ) ):( (( _UseMaskAlpha )?( temp_output_57_0_g1.a ):( (temp_output_57_0_g1).r )) ));
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth114_g1 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth114_g1 = saturate( abs( ( screenDepth114_g1 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _DepthDistance ) ) );
				
				float Alpha = ( temp_output_9_0_g125.a * Dissolve31_g1 * Mask29_g1 * (( _UseDepth )?( distanceDepth114_g1 ):( 1.0 )) );
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18912
1920;0;1920;1029;673.7664;546.8093;1;True;True
Node;AmplifyShaderEditor.FunctionNode;119;98.87307,-54.73675;Inherit;False;UberParticle-Master;0;;1;ce31370a03f90a146a4c77d4e374ba66;0;0;3;FLOAT3;0;FLOAT;24;FLOAT3;109
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;469.5933,-38.16187;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;D4/Particles/UberParticle-Alpha;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;  Blend;0;Two Sided;1;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;DOTS Instancing;0;Meta Pass;0;Extra Pre Pass;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;5;False;True;True;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;1;2;119;0
WireConnection;1;3;119;24
WireConnection;1;5;119;109
ASEEND*/
//CHKSM=89B54171E1C2A43203A2E878BE9118394C2318C5