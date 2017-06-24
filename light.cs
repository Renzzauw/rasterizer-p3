using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Light
    {
        public Vector3 position, color;
        public Matrix4 localTransform;

        public Light(Vector3 position, Vector3 color)
        {
            this.position = position;
            this.color = color;

            localTransform = Matrix4.CreateTranslation(position.X, position.Y, position.Z);
            //localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathhelper.ToRadians(localRotation.X));
            //localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), Mathhelper.ToRadians(localRotation.Y));
            //localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Mathhelper.ToRadians(localRotation.Z));


        }
    }
}
