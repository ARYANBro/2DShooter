shader_type canvas_item;

uniform vec4 color : hint_color;
uniform vec4 hitColor : hint_color;
uniform bool hit;

void fragment()
{
	if (hit)
		COLOR = hitColor;
	else 
		COLOR = color;
}