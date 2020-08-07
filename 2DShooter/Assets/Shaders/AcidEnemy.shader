shader_type canvas_item;

uniform vec4 color : hint_color;
uniform vec4 hitColor : hint_color;
uniform bool hit;

uniform float glowSpeed;
uniform float glow;

void fragment()
{
	if (hit)
	{
		COLOR = hitColor;
	}
	else { 
		COLOR.r = color.r;
		COLOR.g = color.g * clamp(sin(TIME * glowSpeed) + glow / 2.0, 1.2, 2.0); 
		COLOR.b = color.b;
	}
}