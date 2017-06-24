using System.Diagnostics;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Template_P3
{
    public class SceneGraph
    {
        public static List<Mesh> parents = new List<Mesh>();
        
        static void RenderScene(Shader shader, List<Mesh> meshes, Matrix4 parentTransform)
        {
            //check if a mesh has no children
            if (meshes.Count == 0) return;
            // for each parent, render the parent and recursively its children
            for (int i = 0; i < meshes.Count; i++)
            {
                RenderScene(shader, meshes[i].children, meshes[i].localTransform);
                Matrix4 transform = parentTransform * meshes[i].localTransform;
                Matrix4 toWorld = transform;
                transform *= Game.cameraTransform;
                transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 2000);
                meshes[i].Render(shader, transform, toWorld);
            }
        }

        public static void Render(Shader shader, Shader skyboxShader)
        {
            // Render the skybox
            RenderScene(skyboxShader, Game.skyboxList, Matrix4.Identity);

            // Render the other meshes
            RenderScene(shader, parents, Matrix4.Identity);
        }       
    }
}
