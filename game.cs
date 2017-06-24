using System.Diagnostics;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{
    public class Game
    {
	    // member variables
	    public Surface screen;                          // background surface for printing etc.
        public Mesh dickbuttspinner, floor, letterD, ak47, ump, pyr1, pyr2, cube, sun , aP, bP, cP, dP;    // multiple meshes that we will render.
        public Light l1, l2, l3, l4;                    // multiple lights
        public string selectedLight;                    // for moving the lights
        public const float PI = 3.1415926535f;          // PI
        public Stopwatch timer;                         // timer for measuring frame duration
        public Shader shader;                           // shader to use for rendering
        public Shader postproc;                         // shader to use for post processing
        public Shader skyboxShader;
        public Texture wood, ak47t;                     // texture to use for rendering
        public RenderTarget target;                     // intermediate render target
        public ScreenQuad quad;						    // screen filling quad for post processing
	    public bool useRenderTarget = true;
        KeyboardState keyboard;                         // create a keyboard state checking for input
        public static List<Mesh> meshes = new List<Mesh>();
        public static List<Mesh> skyboxList = new List<Mesh>();

        // member variables: world space
        float xRot = 0;							        // world rotation angle around the y-axis
        float yRot = 0;                                 // world rotation angle around the x-axis
        float zRot = 0;                                 // world rotation angle around the z-axis
        public static Matrix4 transform;                // matrix for transformation of the world space
        public static Matrix4 toWorld;                  // matrix for translation to world space

        // member variables: camera 
        public static Vector3 camPosition = new Vector3(0f, -4f, -100f); // camera starting position
        public float moveSpeed = 0.2f;                  // camera movement speed
        public float rotationSpeed = 1f;                // camera rotation speed
        public static Matrix4 cameraTransform;


        // member variables: skybox (cube map)
        public Mesh minX, posX, minY, posY, minZ, posZ;             // planes for each side of the cube map
        public Texture tminX, tminY, tminZ, tposX, tposY, tposZ, sunT, aT, bT, cT, dT;    // textures for each side of the cube map
        public bool enableSkybox = true;                           // turn the skybox on or off

        // initialize
        public void Init()
	    {
            // load textures
            wood = new Texture("../../assets/wood.jpg");
            ak47t = new Texture("../../assets/ak-47/ak-47_diff.jpg");
            tminX = new Texture("../../assets/skybox/negx.png");
            tminY = new Texture("../../assets/skybox/negy.png");
            tminZ = new Texture("../../assets/skybox/negz.png");
            tposX = new Texture("../../assets/skybox/posx.png");
            tposY = new Texture("../../assets/skybox/posy.png");
            tposZ = new Texture("../../assets/skybox/posz.png");
            aT = new Texture("../../assets/planets/plantex1.png");
            bT = new Texture("../../assets/planets/plantex2.png");
            cT = new Texture("../../assets/planets/plantex3.png");
            dT = new Texture("../../assets/planets/plantex5.png");
            sunT = new Texture("../../assets/planets/plantex4.jpg");

            // load meshes          
            sun = new Mesh("../../assets/planets/sun2.obj", "sun", null, new Vector3(100f, 0, 0), new Vector3(45f, 0f, 0f), sunT, true, new Vector3(0.1f, 0, 0), new Vector3(0, 0, 0));
            //aP = new Mesh("../../assets/planets/planetbigger.obj", "aP", sun, new Vector3(-100f, 0, 0), new Vector3(0f, 0f, 0f), aT, false, new Vector3(0.5f, 0, 0), new Vector3(-0.5f, 0, 0));
            //bP = new Mesh("../../assets/planets/planetbig.obj", "bP", sun, new Vector3(0, 1f, -35f), new Vector3(45f, 0f, 0f), bT, true, new Vector3(1f, 0, 0), new Vector3(0.1f, 0, 0));
            //cP = new Mesh("../../assets/planets/planetmedium.obj", "cP", sun, new Vector3(0 , 0, 60f), new Vector3(45f, 0f, 0f), cT, true, new Vector3(0.01f, 0, 0), new Vector3(0.3f, 0, 0));
            //dP = new Mesh("../../assets/planets/planetsmall.obj", "dP", cP, new Vector3(0, 0f, 65f), new Vector3(45f, 0f, 0f), dT, true, new Vector3(0.3f, 0f, 0), new Vector3(0.1f, 0f, 0));
            //ak47 = new Mesh("../../assets/AK-47/AK-47v3.obj", "ak", sun, new Vector3(0, 0, 200f), new Vector3(45f, 0f, 0f), wood, true, new Vector3(0.3f, 0.3f, 0), new Vector3(0.8f, 0.8f, 0.8f));

            if (enableSkybox)
            {
                minX = new Mesh("../../assets/skybox/-x.obj", "-x", null, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), tminX, false, new Vector3(0, 0, 0), new Vector3(0, 0, 0), true);
                minY = new Mesh("../../assets/skybox/-y.obj", "-y", null, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), tminY, false, new Vector3(0, 0, 0), new Vector3(0, 0, 0), true);
                minZ = new Mesh("../../assets/skybox/-z.obj", "-z", null, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), tminZ, false, new Vector3(0, 0, 0), new Vector3(0, 0, 0), true);
                posX = new Mesh("../../assets/skybox/+x.obj", "+x", null, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), tposX, false, new Vector3(0, 0, 0), new Vector3(0, 0, 0), true);
                posY = new Mesh("../../assets/skybox/+y.obj", "+y", null, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), tposY, false, new Vector3(0, 0, 0), new Vector3(0, 0, 0), true);
                posZ = new Mesh("../../assets/skybox/+z.obj", "+z", null, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), tposZ, false, new Vector3(0, 0, 0), new Vector3(0, 0, 0), true);
            }
            // initialize stopwatch
            timer = new Stopwatch();
		    timer.Reset();
		    timer.Start();
		    // create shaders
		    shader = new Shader( "../../shaders/vs.glsl", "../../shaders/fs.glsl" );
            skyboxShader = new Shader("../../shaders/vs.glsl", "../../shaders/fs_skybox.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            // create the render target
            target = new RenderTarget( screen.width, screen.height );
		    quad = new ScreenQuad();
            // load lights
            l1 = new Light(new Vector3(-50, 50, 50), new Vector3(50, 50, 50));
            l2 = new Light(new Vector3(-50, -50, -50), new Vector3(50, 50, 50));
            l3 = new Light(new Vector3(50, 50, -50), new Vector3(50, 50, 50));
            l4 = new Light(new Vector3(50, -50, 50), new Vector3(50, 50, 50));
            PassDataOfLightsToShader(true);
            CreateRandomValuesForPostprocessingNoise();

            foreach (Mesh m in meshes)
            {
                if (m.parent == null)
                {
                    SceneGraph.parents.Add(m);
                }
            }
            cameraTransform = Matrix4.CreateTranslation(camPosition);
        }

        // tick for background surface
        public void Tick()
	    {
            screen.Clear( 0 );
            ControlCamera();
            ControlLights();
        }

        // tick for OpenGL rendering code
        public void RenderGL()
	    {
		    // measure frame duration
		    float frameDuration = timer.ElapsedMilliseconds;
		    timer.Reset();
		    timer.Start();
            camPosition = cameraTransform.ExtractTranslation();
            toWorld = Matrix4.Identity;
            PassDataOfLightsToShader(false);
            CreateRandomValuesForPostprocessingNoise();
            target.Bind();
            SceneGraph.Render(shader, skyboxShader);
            target.Unbind();
            quad.Render(postproc, target.GetTextureID());
        }

        public void PassDataOfLightsToShader(bool init)
        {
            GL.UseProgram(shader.programID);
            int light1PosID = GL.GetUniformLocation(shader.programID, "light1Pos");
            GL.Uniform3(light1PosID, l1.position);
            int light1ColorID = GL.GetUniformLocation(shader.programID, "light1Color");
            GL.Uniform3(light1ColorID, l1.color);
            int light2PosID = GL.GetUniformLocation(shader.programID, "light2Pos");
            GL.Uniform3(light2PosID, l2.position);
            int light2ColorID = GL.GetUniformLocation(shader.programID, "light2Color");
            GL.Uniform3(light2ColorID, l2.color);
            int light3PosID = GL.GetUniformLocation(shader.programID, "light3Pos");
            GL.Uniform3(light3PosID, l3.position);
            int light3ColorID = GL.GetUniformLocation(shader.programID, "light3Color");
            GL.Uniform3(light3ColorID, l3.color);
            int light4PosID = GL.GetUniformLocation(shader.programID, "light4Pos");
            GL.Uniform3(light4PosID, l4.position);
            int light4ColorID = GL.GetUniformLocation(shader.programID, "light4Color");
            GL.Uniform3(light4ColorID, l4.color);

            if (init)
            {
                int ambientColorID = GL.GetUniformLocation(shader.programID, "ambientColor");
                GL.Uniform3(ambientColorID, 0.00f, 0.08f, 0.04f);
            }

            GL.UseProgram(0);
        }

        public void CreateRandomValuesForPostprocessingNoise()
        {
            GL.UseProgram(postproc.programID);
            Random r = new Random();
            Vector2 rnd = new Vector2((float)r.NextDouble(), (float)r.NextDouble());
            int rndID = GL.GetUniformLocation(postproc.programID, "rnd");
            GL.Uniform2(rndID, rnd);
            GL.UseProgram(0);
        }

        // Check for move input
        public void ControlCamera()
        {
            keyboard = Keyboard.GetState();
            // Move the camera
            if (keyboard[Key.W])
            {
                cameraTransform *= Matrix4.CreateTranslation(new Vector3(0f, -moveSpeed, 0f));
            }
            if (keyboard[Key.A])
            {
                cameraTransform *= Matrix4.CreateTranslation(new Vector3(moveSpeed, 0f, 0f));
            }
            if (keyboard[Key.S])
            {
                cameraTransform *= Matrix4.CreateTranslation(new Vector3(0f, moveSpeed, 0f));
            }
            if (keyboard[Key.D])
            {
                cameraTransform *= Matrix4.CreateTranslation(new Vector3(-moveSpeed, 0f, 0f));
            }
            if (keyboard[Key.Z])
            {
                cameraTransform *= Matrix4.CreateTranslation(new Vector3(0f, 0f, -moveSpeed));
            }
            if (keyboard[Key.X])
            {
                cameraTransform *= Matrix4.CreateTranslation(new Vector3(0f, 0f, moveSpeed));
            }

            // Rotate the camera
            if (keyboard[Key.Left])
            {
                cameraTransform *= Matrix4.CreateRotationY(-0.01f);
            }
            if (keyboard[Key.Right])
            {
                cameraTransform *= Matrix4.CreateRotationY(0.01f);
            }
            if (keyboard[Key.Down])
            {
                cameraTransform *= Matrix4.CreateRotationX(0.01f);
            }
            if (keyboard[Key.Up])
            {
                cameraTransform *= Matrix4.CreateRotationX(-0.01f);
            }
            if (keyboard[Key.Q])
            {
                cameraTransform *= Matrix4.CreateRotationZ(-0.01f);
            }
            if (keyboard[Key.E])
            {
                cameraTransform *= Matrix4.CreateRotationZ(0.01f);
            }
        }

        void ControlLights()
        {
            keyboard = Keyboard.GetState();

            if (keyboard[Key.Number1])
            {
                selectedLight = "l1";
            }
            else if (keyboard[Key.Number2])
            {
                selectedLight = "l2";
            }
            else if (keyboard[Key.Number3])
            {
                selectedLight = "l3";
            }
            else if (keyboard[Key.Number4])
            {
                selectedLight = "l4";
            }
            else if (keyboard[Key.BackSpace])
            {
                selectedLight = null;
            }

            if (selectedLight == "l1")
            {
                ChangeLight(l1);
            }
            else if (selectedLight == "l2")
            {
                ChangeLight(l2);
            }
            else if (selectedLight == "l3")
            {
                ChangeLight(l3);
            }
            else if (selectedLight == "l4")
            {
                ChangeLight(l4);
            }
        }

        void ChangeLight(Light light)
        {
            if (keyboard[Key.T])
            {
                light.position += new Vector3(0.0f, 0.5f, 0.0f);
            }
            if (keyboard[Key.F])
            {
                light.position += new Vector3(-0.5f, 0.0f, 0.0f);
            }
            if (keyboard[Key.G])
            {
                light.position += new Vector3(0.0f, -0.5f, 0.0f);
            }
            if (keyboard[Key.H])
            {
                light.position += new Vector3(0.5f, 0.0f, 0.0f);
            }
            if (keyboard[Key.V])
            {
                light.position += new Vector3(0.0f, 0.0f, 0.5f);
            }
            if (keyboard[Key.B])
            {
                light.position += new Vector3(0.0f, 0.0f, -0.5f);
            }
            if (keyboard[Key.I])
            {
                light.color += new Vector3(0.15f, 0.0f, 0.0f);
            }
            if (keyboard[Key.J])
            {
                light.color += new Vector3(-0.15f, 0.0f, 0.0f);
                if (light.color.X < 0.0f)
                {
                    light.color.X = 0;
                }
            }
            if (keyboard[Key.O])
            {
                light.color += new Vector3(0.0f, 0.15f, 0.0f);
            }
            if (keyboard[Key.K])
            {
                light.color += new Vector3(0.0f, -0.15f, 0.0f);
                if (light.color.Y < 0.0f)
                {
                    light.color.Y = 0;
                }
            }
            if (keyboard[Key.P])
            {
                light.color += new Vector3(0.0f, 0.0f, 0.15f);
            }
            if (keyboard[Key.L])
            {
                light.color += new Vector3(0.0f, 0.0f, -0.15f);
                if (light.color.Z < 0.0f)
                {
                    light.color.Z = 0;
                }
            }
        }
    }
}// namespace Template_P3