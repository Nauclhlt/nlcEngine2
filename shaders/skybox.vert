#version 430

layout (location = 0) in vec3 position;

out vec3 vTexCoord;

layout (location = 0) uniform mat4 modelMatrix;
layout (location = 1) uniform mat4 viewMatrix;
layout (location = 2) uniform mat4 projMatrix;

void main()
{
    vTexCoord = position;
    vec4 pos = projMatrix * viewMatrix * vec4(position, 1.0);
    gl_Position = pos.xyww;
}