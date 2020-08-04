shader_type canvas_item;

uniform float minScale;
uniform float maxScale;

uniform sampler2D outlineTexture;
uniform sampler2D normalTexture;

uniform bool outline;

void vertex()
{
	if (outline)
		VERTEX *= (sin(TIME) + minScale) * maxScale;
}

void fragment()
{
	if (outline)
		COLOR = texture(outlineTexture, UV);
	else
		COLOR = texture(normalTexture, UV);
}