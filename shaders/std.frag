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
        vec4 texel = texture(pTexture, vTexCoord);
        fragColor = vec4(texel.rgb * vColor.rgb, texel.a);
    }
    else
    {
        fragColor = vColor;
    }
}