#version 330 core
layout (location = 0) in vec2 pos;

out vec2 fragPos;

uniform mat4 transform;

void main(){
    gl_Position = vec4(pos, 0.0f, 1.0f) * transform;
    
    vec2 vec = pos;
    if (vec.x == -1){
        vec.x = 0;
    }
    if (vec.y == -1){
        vec.y  = 0;
    }
    fragPos = vec;
}