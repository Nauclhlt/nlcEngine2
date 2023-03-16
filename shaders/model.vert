#version 430

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texCoord;

layout (location = 0) uniform mat4 modelMatrix;
layout (location = 1) uniform mat4 viewMatrix;
layout (location = 2) uniform mat4 projMatrix;

out vec2 vTexCoord;
out vec3 vNormal;

void main()
{
    vec4 worldPos = modelMatrix * vec4(position, 1);
    vTexCoord = texCoord;

    mat3 normalMatrix = transpose(inverse(mat3(modelMatrix)));
    vNormal = normalMatrix * normal;

    gl_Position = projMatrix * viewMatrix * worldPos;
}