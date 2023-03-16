#version 430

out vec4 fragColor;

in vec2 vTexCoord;
in vec3 vFragPos;
in vec3 vNormal;
in vec4 vColor;

uniform sampler2D pTexture;

struct Light
{
    vec3 Position;
    vec3 Color;
    vec3 SpecColor;
    float Radius;
    float Intensity;
};

layout(std430, binding=0) buffer SSBO
{
    float data[];
};

uniform int lightCount;
uniform vec3 viewPos;
uniform float ambientIntensity;
uniform vec3 ambientColor;
uniform vec3 backColor;
uniform bool textured;

void main()
{
    vec3 Diffuse;
    if (textured)
    {
        Diffuse = texture(pTexture, vTexCoord).rgb;
    }
    else
    {
        Diffuse = vColor.rgb;
    }
    float Specular = 0.3;

    vec3 lightingResult = (Diffuse * ambientColor) * ambientIntensity;
    vec3 viewDir = normalize(viewPos - vFragPos);
    for (int i = 0; i < lightCount; i++)
    {
        int head = i * 12;
        vec3 L_Diff = vec3(data[head], data[head + 1], data[head + 2]);
        vec3 L_Spec = vec3(data[head + 3], data[head + 4], data[head + 5]);
        vec3 L_Pos = vec3(data[head + 6], data[head + 7], data[head + 8]);
        float L_Rad = data[head + 9];
        float L_Att = data[head + 10];
        float L_Its = data[head + 11];

        // diffuse
        vec3 lightDir = normalize(L_Pos - vFragPos);
        vec3 diffuse = max(dot(vNormal, lightDir), 0.0) * Diffuse * L_Diff;

        // specular
        vec3 halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(vNormal, halfwayDir), 0.0), 16.0);
        vec3 specular = L_Spec * spec * Specular;

        // attenuation
        float distance = length(L_Pos - vFragPos);
        float attenuation = pow(clamp(1.0 - distance/L_Rad, 0.0, 1.0), 2);
		
        diffuse *= attenuation;
        specular *= attenuation;
        lightingResult += (diffuse + specular) * L_Its;
    }

    fragColor = vec4(lightingResult, 1.0);
}