#version 430

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec3 normal;
layout (location = 3) in vec2 texCoord;

layout (location = 0) uniform mat4 modelMatrix;
layout (location = 1) uniform mat4 viewMatrix;
layout (location = 2) uniform mat4 projMatrix;

out vec3 vFragPos;
out vec3 vNormal;
out vec4 vColor;
out vec2 vTexCoord;

void main()
{
    vec4 worldPos = modelMatrix * vec4(position, 1);
    vFragPos = worldPos.xyz;
    vTexCoord = texCoord;

    mat3 normalMatrix = transpose(inverse(mat3(modelMatrix)));
    vNormal = normalMatrix * normal;
    vColor = color;

    gl_Position = projMatrix * viewMatrix * worldPos;
}