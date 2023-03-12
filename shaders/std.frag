#version 430

uniform sampler2D pTexture;
uniform bool textured;

in vec4 vColor;
in vec2 vTexCoord;

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