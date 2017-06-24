#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 worldPos;				// worldPosition
uniform sampler2D pixels;		// texture sampler
uniform vec3 ambientColor;		// ambient color
uniform vec3 light1Pos, light2Pos, light3Pos, light4Pos;				// the positions for the lights
uniform vec3 light1Color, light2Color, light3Color, light4Color;		// the colors of the lights

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
    vec3 L1 = light1Pos - worldPos.xyz;						// vector L1 represents the vector between a point in the scene to light 1
    vec3 L2 = light2Pos - worldPos.xyz;						// idem for light 2
    vec3 L3 = light3Pos - worldPos.xyz;						// idem for light 3 
    vec3 L4 = light4Pos - worldPos.xyz;						// idem for light 4
	vec3 R1 = reflect(-L1, normal.xyz);						// vector R1 is the vector reflected from L1 over the normal of the point in space
	vec3 R2 = reflect(-L2, normal.xyz);						// idem for light 2
	vec3 R3 = reflect(-L3, normal.xyz);						// idem for light 3
	vec3 R4 = reflect(-L4, normal.xyz);						// idem for light 4
	vec3 viewDir = normalize(-worldPos.xyz);				// the viewdirection is the -worldposition normalized
	float dist1 = L1.length();								// dist1 is the length of vector L1, used for the attenuation
	float dist2 = L2.length();								// idem for L2
	float dist3 = L3.length();								// idem for L3
	float dist4 = L4.length();								// idem for L4
	L1 = normalize(L1);										// normalize L1
	L2 = normalize(L2);										// normalize L2
	L3 = normalize(L3);										// normalize L3
	L4 = normalize(L4);										// normalize L4
	R1 = normalize(R1);										// normalize R1
	R2 = normalize(R2);										// normalize R2
	R3 = normalize(R3);										// normalize R3
	R4 = normalize(R4);										// normalize R4
	vec3 materialColor = texture(pixels, uv).xyz;			// get the materialColor from the texture
	float attenuation1 = 1.0f / (dist1 * dist1);			// calculate the attenuation for light 1
	float attenuation2 = 1.0f / (dist2 * dist2);			// idem for light 2
	float attenuation3 = 1.0f / (dist3 * dist3);			// idem for light 3
	float attenuation4 = 1.0f / (dist4 * dist4);			// idem for light 4
	float lambertian1 = max(0.0f, dot(L1, normal.xyz));		// calculate the lambertian value for light 1 (n dot l)
	float lambertian2 = max(0.0f, dot(L2, normal.xyz));		// idem for light 2
	float lambertian3 = max(0.0f, dot(L3, normal.xyz));		// idem for light 3
	float lambertian4 = max(0.0f, dot(L4, normal.xyz));		// idem for light 4
	float specular1 = 0.0f;									// instantiate the specular value for light 1
	float specular2 = 0.0f;									// idem for light 2
	float specular3 = 0.0f;									// idem for light 3
	float specular4 = 0.0f;									// idem for light 4
	if (lambertian1 >= 0.0f)								// if the point receives light from light 1, we want to calculate the specular value
	{
		float specAngle = max(dot(R1, viewDir), 0.0f);		// calculate the specular value using R1 and the view direction
		specular1 = pow(specAngle, 1000.0f);				// calculate the specular value by doing the angle to the power of a constance
	}
	if (lambertian2 >= 0.0f)								// idem for light 2
	{
		float specAngle = max(dot(R2, viewDir), 0.0f);
		specular2 = pow(specAngle, 1000.0f);
	}
	if (lambertian3 >= 0.0f)								// idem for light 3
	{
		float specAngle = max(dot(R3, viewDir), 0.0f);
		specular3 = pow(specAngle, 1000.0f);
	}
	if (lambertian4 >= 0.0f)								// idem for light 4
	{
		float specAngle = max(dot(R4, viewDir), 0.0f);
		specular4 = pow(specAngle, 1000.0f);
	}
	outputColor = vec4(ambientColor, 1);					// start by setting the outputcolor to the ambientcolor
	outputColor += vec4(materialColor * lambertian1 * attenuation1 * light1Color + specular1 * vec3(1, 1, 1) * light1Color, 1);		// add the lambertian and specular colors from light 1 to the outputcolor
	outputColor += vec4(materialColor * lambertian2 * attenuation2 * light2Color + specular2 * vec3(1, 1, 1) * light2Color, 1);		// idem for light 2
	outputColor += vec4(materialColor * lambertian3 * attenuation3 * light3Color + specular3 * vec3(1, 1, 1) * light3Color, 1);		// idem for light 3
	outputColor += vec4(materialColor * lambertian4 * attenuation4 * light4Color + specular4 * vec3(1, 1, 1) * light4Color, 1);		// idem for light 4
}