#version 430

layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec3 normal;
layout (location = 3) in vec2 texCoord;

layout (location = 0) uniform mat4 lightSpaceMatrix;
layout (location = 1) uniform mat4 modelMatrix;

void main()
{
    gl_Position = lightSpaceMatrix * modelMatrix * vec4(position, 1);
}