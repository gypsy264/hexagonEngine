#version 330 core

out vec4 FragColor;
in vec2 TexCoord;

uniform sampler2D texture0;
uniform vec3 textColor;

void main()
{

    vec4 sampled = texture(texture0, TexCoord);
    FragColor = texture(texture0, TexCoord);
}