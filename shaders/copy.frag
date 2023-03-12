#version 430

uniform sampler2D pTexture;

in vec2 vTexCoord;

out vec4 fragColor;

void main()
{
    fragColor = texture(pTexture, vTexCoord);
    //fragColor = vec4(1, 1, 1, 1);
}