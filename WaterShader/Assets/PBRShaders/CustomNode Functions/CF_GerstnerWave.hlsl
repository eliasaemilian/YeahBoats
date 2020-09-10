static float TAU = 6.28318530718f;
float3 GerstnerWave(float Time, float4 wave, float3 p, inout float3 tangent, inout float3 bitangent, inout float3 normal)
{
    float steepness = wave.z;
    float wavelength = wave.w;
    float k = TAU / wavelength;
    float a = steepness / k;

    float c = sqrt(9.8 / k);
    float2 d = normalize(wave.xy);
    float f = k * (dot(d, p.xz) - c * Time);

    float3 _bitangent = float3(
                    1 - (d.x * d.x * steepness * cos(f)),
                    d.x * steepness * sin(f),
                    -(d.x * d.y * steepness * cos(f))
                    );
    float3 _tangent = float3(
                    -(d.x * d.y * steepness * cos(f)),
                    d.y * steepness * sin(f),
                    1 - (d.y * d.y * steepness * cos(f))
                    );

    _bitangent = normalize(_bitangent);
    _tangent = normalize(_tangent);

    float3 _normal = cross(_tangent, _bitangent);
    normal += _normal;

    bitangent += _bitangent;
    tangent += _tangent;
    return float3(
                    d.x * (a * sin(f)),
                    a * cos(f),
                    d.y * (a * sin(f))
                    );
}




void GerstnerWave_float (float Time, float3 PositionOS, float4 WaveA, float4 WaveB, float4 WaveC, out float3 Position, out float3 Normal, out float3 Tangent)
{
    float3 gridPoint = PositionOS;
    float3 tangent = 0;
    float3 binormal = 0;
    float3 normal = 0;
    float3 p = gridPoint;

    p += GerstnerWave(Time, WaveA, gridPoint, tangent, binormal, normal);
    p += GerstnerWave(Time, WaveB, gridPoint, tangent, binormal, normal);
    p += GerstnerWave(Time, WaveC, gridPoint, tangent, binormal, normal);

    Position = p;
    Normal = normal;
    Tangent = tangent;
}