#version 330

// shader input
in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)

// shader output
out vec3 outputColor;

void main()
{
	// retrieve input pixel
	outputColor = texture(pixels, uv).rgb;
	
	// vignette
	vec2 offsetFromCenter = P.xy - vec2(0.5, 0.5);
	float length = length(offsetFromCenter);
	outputColor *= vec3(1.0 - length);

	// chromatic aberration
}
// EOF