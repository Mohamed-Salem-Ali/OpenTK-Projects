#version 330 core
layout (location = 0) in vec3 v_position;
layout (location = 1) in vec2 v_texCoord;

out vec2 f_texCoord;

uniform mat4 u_transform;
uniform mat4 u_view;
uniform mat4 u_projection;

void main()
{
    gl_Position = vec4(v_position, 1.0) * u_transform * u_view * u_projection;
    f_texCoord = v_texCoord;
}