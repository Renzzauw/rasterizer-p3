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

	// vignette shader
	/*vec2 position = P.xy;
	vec2 centeroffset = position - vec2(0.5f);
	float length = centeroffset.length();
	float vignette = smoothstep(0.75f, 0.45f, length);
	outputColor = mix( outputColor, outputColor * vignette, 0.5).rgb;*/

	vec2 position = P.xy - vec2(0.5, 0.5);
	float length = length(position);
	//outputColor *= vec3(1.0 - length);
	//gl_FragColor = vec4(outputColor * (1.0 - length), 1);
}
// EOF