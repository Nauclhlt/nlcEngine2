#version 430

layout ( location = 0 ) uniform mat4 modelMatrix;
layout ( location = 1 ) uniform mat4 viewMatrix;
layout ( location = 2 ) uniform mat4 projMatrix;

layout ( location = 0 ) in vec3 position;
layout ( location = 1 ) in vec4 color;
layout ( location = 2 ) in vec3 normal;
layout ( location = 3 ) in vec2 texCoord;

out vec4 vColor;
out vec2 vTexCoord;

void main()
{
    gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(position, 1);  // matrix transformation
    vColor = color;
    vTexCoord = texCoord;
}