#version 430

uniform sampler2D pDepthTexture;

in vec2 vTexCoord;

out vec4 fragColor;

void main()
{
    float depth = texture(pDepthTexture, vTexCoord).r;
    fragColor = vec4(vec3(depth), 1.0);
}