#version 330

// shader input
in vec2 vUV;				// uv coordinates
in vec3 vPosition;			// untransformed vertex position

// shader output
out vec2 uv;				// uv pass-through
out vec2 position;			// position pass-through

// vertex shader
void main()
{
	uv = vUV;
	position = vec2(vPosition) * 0.5 + vec2(0.5);
	gl_Position = vec4(vPosition, 1);
}

// EOF