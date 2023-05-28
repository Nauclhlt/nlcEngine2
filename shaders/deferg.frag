#version 430

layout (location = 0) out vec3 gPosition;
layout (location = 1) out vec3 gNormal;
layout (location = 2) out vec4 gColorSpec;

in vec2 TexCoords;
in vec3 FragPos;
in vec3 Normal;
in vec4 Color;

uniform sampler2D pTexture;
uniform bool textured;

void main()
{
    gPosition = FragPos;
    gNormal = normalize(Normal);
    if (textured)
    {
        vec4 texel = texture(pTexture, TexCoords);
        if (texel.a == 0.0)
        {
            discard;
        }
        gColorSpec.rgb = texel.rgb;
    }
    else
    {
        gColorSpec.rgb = Color.rgb;
    }
    gColorSpec.a = 0.1;
}