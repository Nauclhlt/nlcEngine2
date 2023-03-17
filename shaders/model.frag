#version 430

uniform sampler2D pTexture;
uniform bool textured;

in vec3 vNormal;
in vec2 vTexCoord;
in vec4 vColor;

out vec4 fragColor;

void main()
{
    if (textured)
    {
        fragColor = texture(pTexture, vTexCoord);
    }
    else
    {
        fragColor = vColor;
    }
}