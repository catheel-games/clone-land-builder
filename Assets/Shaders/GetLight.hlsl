void GetLight_float(float3 WorldPosition, out float3 Direction, out float3 Color, out float Attenuation)
{
    #if defined(SHADERGRAPH_PREVIEW)
        Direction = 1;
        Color = 1;
        Attenuation = 1;
    #else
        float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
        Light mainLight = GetMainLight(shadowCoord);

        Direction = mainLight.direction;
        Color = mainLight.color;
        // Attenuation = mainLight.shadowAttenuation;
        ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
        float shadowStrength = GetMainLightShadowStrength();
        Attenuation = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowSamplingData, shadowStrength, false);
        // Attenuation = MainLightShadow(shadowCoord, WorldPosition, mainLight.shadowMask, mainLight.occlusionProbeChannels);
    #endif
}