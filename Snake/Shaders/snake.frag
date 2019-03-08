#version 330 core
in vec2 fragPos;

out vec4 color;

uniform vec2 direction;


void main(){
    color = vec4(0.196078431f, 0.196078431f, 0.784313725f, 1.0f);
    float width = 1 - .9f;
    if ((direction.x == 1 || direction.x == -1) && //E || W
        fragPos.x <= width || fragPos.x >= 1-width) // y = [width;1-width]
    {
        discard;
    }
    if ((direction.y == 1 || direction.y == -1) && //S || N
        fragPos.x <= width || fragPos.x >= 1-width) // x = [width;1-width]
    {
        discard;
    }
}
