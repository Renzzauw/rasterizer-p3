#version 330

// shader input
in vec2 position;				// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)

// shader output
out vec3 outputColor;

void main()
{
	outputColor = texture(pixels, uv).rgb;					// retrieve input pixel
	vec2 offsetFromCenter = position - vec2(0.5);			// calculate the offset from the center of the screen
	float lengthOfOffset = length(offsetFromCenter);		// calculate the lenght off the offset

	// chromatic aberration
	vec4 rValue = texture2D(pixels, position - 0.0101 * lengthOfOffset);  
    vec4 gValue = texture2D(pixels, position);
    vec4 bValue = texture2D(pixels, position - 0.0099 * lengthOfOffset);  
    outputColor = vec3(rValue.r, gValue.g, bValue.b);

	// vignette
	outputColor *= vec3(1.0 - lengthOfOffset);
}
// EOF