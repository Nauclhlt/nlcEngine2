#version 430

uniform sampler2D pTexture;
uniform float applyMin;

in vec2 vTexCoord;

out vec4 fragColor;

void main()
{
	
	vec4 texel = texture(pTexture, vTexCoord);
	vec4 c = vec4(0.0);
	float brightness = dot(texel.rgb, vec3(0.2126, 0.7152, 0.0722));
    if(brightness > applyMin)
	{
		c = vec4(texel.rgb, 1);
	}
	else
	{
		c = vec4(0, 0, 0, 0);
	}
	fragColor = c;

	//fragColor = texture(pTexture, vTexCoord);
}