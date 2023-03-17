#version 430

layout ( location = 0 ) in vec3 position;
layout ( location = 1 ) in vec4 color;
layout ( location = 2 ) in vec3 normal;
layout ( location = 3 ) in vec2 texCoord;

layout ( location = 0 ) uniform mat4 modelMatrix;
layout ( location = 1 ) uniform mat4 viewMatrix;
layout ( location = 2 ) uniform mat4 projMatrix;

out vec3 FragPos;
out vec2 TexCoords;
out vec3 Normal;
out vec4 Color;

void main()
{
    vec4 worldPos = modelMatrix * vec4(position, 1);
    FragPos = worldPos.xyz;
    TexCoords = texCoord;

    mat3 normalMatrix = transpose(inverse(mat3(modelMatrix)));
    Normal = normalMatrix * normal;
    Color = color;

    gl_Position = projMatrix * viewMatrix * worldPos;
}