using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    // mesh and loader based on work by JTalton; http://www.opentk.com/node/642
    public class Mathhelper
    {
        // data members
        public const float PI = 3.1415926535f;          // PI

        // Method to calculate the amount of radians from degrees
        public static float ToRadians( float degrees)
        {
            return degrees * PI / 180;
        }
    }
} // namespace Template_P3