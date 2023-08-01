#version 330 core
layout (location = 0) in vec2 pos;
layout (location = 1) in vec2 texCoord;

out vec2 f_texCoord;

uniform mat4 transform;

void main(){
    f_texCoord = texCoord;
    gl_Position = vec4(pos, 0.0f, 1.0f) * transform;
}