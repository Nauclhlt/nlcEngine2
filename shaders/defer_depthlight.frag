#version 430

out vec4 fragColor;

in vec2 vTexCoord;

uniform sampler2D gPosition;
uniform sampler2D gNormal;
uniform sampler2D gColorSpec;
uniform sampler2D shadowMap;

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
uniform mat4 lightSpaceMatrix;
uniform vec3 lightPos;

float calcShadow(vec4 fragPosLightSpace, vec3 normal, vec3 fragPos)
{
    // perform perspective divide
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    // transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;
    // get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
    float closestDepth = texture(shadowMap, projCoords.xy).r; 
    // get depth of current fragment from light's perspective
    float currentDepth = projCoords.z;
    // calculate bias (based on depth map resolution and slope)
    vec3 lightDir = normalize(lightPos - fragPos);
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    // check whether current frag pos is in shadow
    // float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;
    // PCF
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth  ? 1.0 : 0.0;
        }    
    }
    shadow /= 9.0;
    
    // keep the shadow at 0.0 when outside the far_plane region of the light's frustum.
    if(projCoords.z > 1.0)
        shadow = 0.0;
        
    return shadow;
}

void main()
{
    vec3 FragPos = texture(gPosition, vTexCoord).rgb;
    vec3 Normal = texture(gNormal, vTexCoord).rgb;
    vec3 Diffuse = texture(gColorSpec, vTexCoord).rgb;
    vec4 FragPosLightSpace = lightSpaceMatrix * vec4(FragPos, 1);
    float Specular = texture(gColorSpec, vTexCoord).a;

    if (Diffuse.rgb == vec3(0, 0, 0) && Specular == 0f)
    {
        fragColor = vec4(backColor, 1f);
        return;
    }

    vec3 lightingResult = (Diffuse * ambientColor) * ambientIntensity;
    vec3 viewDir = normalize(viewPos - FragPos);
    float shadow = calcShadow(FragPosLightSpace, Normal, FragPos);
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
        vec3 lightDir = normalize(L_Pos - FragPos);
        vec3 diffuse = max(dot(Normal, lightDir), 0.0) * Diffuse * L_Diff;

        // specular
        vec3 halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(Normal, halfwayDir), 0.0), 16.0);
        vec3 specular = L_Spec * spec * Specular;

        // attenuation
        float distance = length(L_Pos - FragPos);
        float attenuation = pow(clamp(1.0 - distance/L_Rad, 0.0, 1.0), 2);
		
        diffuse *= attenuation;
        specular *= attenuation;
        lightingResult += ((diffuse + specular) * L_Its) * (1.0 - shadow);
    }

    lightingResult = pow(lightingResult, vec3(1.0/2.2));

    fragColor = vec4(lightingResult, 1.0);
}