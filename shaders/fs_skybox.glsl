#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// texture sampler

// shader output
out vec4 outputColor;			// output color

// fragment shader
void main()
{
	vec3 materialColor = texture(pixels, uv).xyz;			// get the materialcolor from the texture
	outputColor = vec4(materialColor, 1);					// set the outputColor to the materialColor, because this is the skybox, we don't want to do anything with lights and such
}