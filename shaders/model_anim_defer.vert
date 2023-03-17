#version 430

layout ( location = 0 ) in vec3 position;
layout ( location = 1 ) in vec4 color;
layout ( location = 2 ) in vec3 normal;
layout ( location = 3 ) in vec2 texCoord;
layout ( location = 4 ) in ivec4 boneIds;
layout ( location = 5 ) in vec4 weights;

layout ( location = 0 ) uniform mat4 modelMatrix;
layout ( location = 1 ) uniform mat4 viewMatrix;
layout ( location = 2 ) uniform mat4 projMatrix;

uniform mat4 finalBoneMatrices[100];

out vec3 FragPos;
out vec2 TexCoords;
out vec3 Normal;
out vec4 Color;

void main()
{
    vec4 totalPosition = vec4(0.0);
    for (int i = 0; i < 4; i++)
    {
        if (boneIds[i] == -1)
        { 
            continue;
        }

        if (boneIds[i] >= 100)
        {
            totalPosition = vec4(position, 1.0);
            break;
        }

        vec4 localPosition = finalBoneMatrices[boneIds[i]] * vec4(position,1.0);
        totalPosition += localPosition * weights[i];
        vec3 localNormal = mat3(finalBoneMatrices[boneIds[i]]) * normal;
    }

    FragPos = (modelMatrix * totalPosition).xyz;
    TexCoords = texCoord;
    mat3 normalMatrix = transpose(inverse(mat3(modelMatrix)));
    Normal = normalMatrix * normal;
    Color = color;
    
    mat4 viewModel = viewMatrix * modelMatrix;
    gl_Position =  projMatrix * viewModel * totalPosition;
}