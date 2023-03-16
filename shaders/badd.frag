#version 430

uniform sampler2D pTexture;
uniform sampler2D destTexture;
uniform float blurFactor;
uniform float baseFactor;

in vec2 vTexCoord;

out vec4 fragColor;

void main()
{
	vec4 p = texture(pTexture, vTexCoord);
	vec4 dest = texture(destTexture, vTexCoord);

	if ( p.a == 0 )
	{
		discard;
	}

	if ( p.rgb == vec3(0, 0, 0) )
	{
		fragColor = dest;
		return;
	}

	fragColor = p * blurFactor + dest * baseFactor;
}