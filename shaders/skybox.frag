#version 430

uniform samplerCube pTexture;

in vec3 vTexCoord;

out vec4 fragColor;

void main()
{
    fragColor = texture(pTexture, vTexCoord);
}