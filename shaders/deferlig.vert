#version 430

layout ( location = 0 ) in vec3 position;
layout ( location = 1 ) in vec2 texCoord;

out vec2 vTexCoord;

void main()
{
    vTexCoord = texCoord;
    gl_Position = vec4(position, 1);
}