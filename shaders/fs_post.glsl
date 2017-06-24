#version 330

// shader input
in vec2 position;				// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)
uniform vec2 rnd;

// shader output
out vec3 outputColor;

float rand(vec2 position)
{
	vec2 co = vec2(position.x * rnd.x, position.y * rnd.y);
	return 0.5 + 0.5 * fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()
{
	// retrieve input pixel
	outputColor = texture(pixels, uv).rgb;
	
	// offset and length of the offset
	vec2 offsetFromCenter = position.xy - vec2(0.5, 0.5);
	float length = length(offsetFromCenter);

	// chromatic abberation
	vec4 rValue = texture2D(pixels, position - offsetFromCenter * 0.01);
	vec4 gValue = texture2D(pixels, position);
	vec4 bValue = texture2D(pixels, position + offsetFromCenter * 0.01);
	outputColor = vec3(rValue.r, gValue.g, bValue.b);

	// vignette
	outputColor *= vec3(1.0 - length);

	// noise
	vec3 noise = vec3(rand(position));
	outputColor = mix(outputColor, noise, 0.1f);
}
// EOF