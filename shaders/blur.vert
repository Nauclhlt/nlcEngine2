#version 430

layout ( location = 0 ) in vec2 position;
layout ( location = 1 ) in vec2 texCoord;  // TEST CODE

out vec2 vTexCoord;

void main()
{
	gl_Position = vec4(position, 0, 1);
	vTexCoord = texCoord;
}