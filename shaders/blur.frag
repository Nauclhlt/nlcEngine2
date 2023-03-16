#version 430

uniform sampler2D pTexture;

in vec2 vTexCoord;

out vec4 fragColor;

uniform int samples;
uniform float weights[64];
uniform bool vertical;

void main()
{
	vec2 tex_offset = 1.0 / textureSize(pTexture, 0);
	vec4 color = vec4(0);
	int halfSamples = samples / 2;
	if ( vertical )
	{
		// vertical blur

		for ( int offset = -halfSamples; offset <= halfSamples; offset++ )
		{
			color += texture(pTexture, vec2(vTexCoord.x, vTexCoord.y + tex_offset.y*offset)) * weights[abs(offset)];
		}
	}
	else
	{
		// horizontal blur

		for ( int offset = -halfSamples; offset <= halfSamples; offset++ )
		{
			color += texture(pTexture, vec2(vTexCoord.x + tex_offset.x*offset, vTexCoord.y)) * weights[abs(offset)];
		}
	}

	if ( color.rgb != vec3(0, 0, 0) )
	{
		color.a = 1;
	}

	fragColor = color;
}