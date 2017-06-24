using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Input;

namespace Template_P3
{
// mesh and loader based on work by JTalton; http://www.opentk.com/node/642
    public class Mesh
    {
	    // data members
	    public ObjVertex[] vertices;			// vertex positions, model space
	    public ObjTriangle[] triangles;			// triangles (3 vertex indices)
	    public ObjQuad[] quads;					// quads (4 vertex indices)
	    int vertexBufferId;						// vertex buffer
	    int triangleBufferId;					// triangle buffer
	    int quadBufferId;                       // quad buffer
        public Mesh parent;
        public string modelName;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Matrix4 localTransform;
        public Matrix4 localToParent;
        public Texture texture;
        public bool canRotate;
        public Vector3 rotateSpeed;
        public Vector3 orbitSpeed;
        public Vector3 localOrbit;
        public List<Mesh> children = new List<Mesh>();
        
        // constructor
        public Mesh( string fileName, string modelName, Mesh parent, Vector3 localPosition, Vector3 localRotation, Texture texture, bool canRotate, Vector3 rotateSpeed, Vector3 orbitSpeed, bool isSkyBox = false)
	    {
		    MeshLoader loader = new MeshLoader();
		    loader.Load( this, fileName );
            this.parent = parent;
            this.localRotation = localRotation;
            this.modelName = modelName;
            if (parent != null)
            {
                parent.children.Add(this);
            }

            Console.WriteLine("Added " + modelName + " to the scenegraph");
            this.localPosition = localPosition;
            this.texture = texture;
            this.canRotate = canRotate;
            this.rotateSpeed = rotateSpeed;
            this.orbitSpeed = orbitSpeed;
            if (isSkyBox)
            {
                Game.skyboxList.Add(this);
            }
            else
            {
                Game.meshes.Add(this);
            }
            
            if (parent != null)
            {
                localTransform = Matrix4.CreateTranslation(localPosition);
            }
            else
            {
                localTransform = Matrix4.CreateTranslation(localPosition);
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Mathhelper.ToRadians(0.01f));
            }
            
        }

        // initialization; called during first render
        public void Prepare( Shader shader )
	    {
            if (vertexBufferId != 0) return; // already taken care of

            // generate interleaved vertex data (uv/normal/position (total 8 floats) per vertex)
		    GL.GenBuffers( 1, out vertexBufferId );
		    GL.BindBuffer( BufferTarget.ArrayBuffer, vertexBufferId );
		    GL.BufferData( BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf( typeof( ObjVertex ) )), vertices, BufferUsageHint.StaticDraw );

		    // generate triangle index array
		    GL.GenBuffers( 1, out triangleBufferId );
		    GL.BindBuffer( BufferTarget.ElementArrayBuffer, triangleBufferId );
		    GL.BufferData( BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf( typeof( ObjTriangle ) )), triangles, BufferUsageHint.StaticDraw );

		    // generate quad index array
		    GL.GenBuffers( 1, out quadBufferId );
		    GL.BindBuffer( BufferTarget.ElementArrayBuffer, quadBufferId );
		    GL.BufferData( BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf( typeof( ObjQuad ) )), quads, BufferUsageHint.StaticDraw );
	    }

	    // render the mesh using the supplied shader and matrix
	    public void Render( Shader shader, Matrix4 transform, Matrix4 toWorld)
	    {
            
            /*if (canRotate)
            {
                localRotation.X += rotateSpeed.X;
                localRotation.Y += rotateSpeed.Y;
                localRotation.Z += rotateSpeed.Z;
            }*/
            if (Game.skyboxList.Contains(this))
            {
                localTransform = Matrix4.CreateTranslation(Game.camPosition);
                //localTransform.Invert();
            }

            else if (parent == null)
            {
                /*localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Mathhelper.ToRadians(0.01f));*/
                //localOrbit.X += orbitSpeed.X;
                //localOrbit.Y += orbitSpeed.Y;
                //localOrbit.Z += orbitSpeed.Z;
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathhelper.ToRadians(localOrbit.X));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), Mathhelper.ToRadians(localOrbit.Y));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Mathhelper.ToRadians(localOrbit.Z));
            }

            else if (parent != null)
            {
                /*
                // rotate the object in global space
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Mathhelper.ToRadians(0.01f));


                // position the object looking at both local and parent position
                localTransform *= Matrix4.CreateTranslation(localPosition.X, localPosition.Y, localPosition.Z);

                // Orbit the object in parent space
                //localOrbit.X += orbitSpeed.X;
                //localOrbit.Y += orbitSpeed.Y;
                //localOrbit.Z += orbitSpeed.Z;
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), Mathhelper.ToRadians(0.01f));
                localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Mathhelper.ToRadians(0.01f));
                */

                //localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), Mathhelper.ToRadians(0.01f));
                //localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), Mathhelper.ToRadians(0.01f));
                //localTransform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Mathhelper.ToRadians(0.01f));

                //localTransform *= parent.localTransform;
            }


            // on first run, prepare buffers
            Prepare( shader );

		    // enable texture
		    int texLoc = GL.GetUniformLocation( shader.programID, "pixels" );
		    GL.Uniform1( texLoc, 0 );
		    GL.ActiveTexture( TextureUnit.Texture0 );
		    GL.BindTexture( TextureTarget.Texture2D, texture.id );

		    // enable shader
		    GL.UseProgram( shader.programID );

		    // pass transform to vertex shader
		    GL.UniformMatrix4( shader.uniform_mview, false, ref transform );
            GL.UniformMatrix4( shader.uniform_2wrld, false, ref toWorld );

		    // bind interleaved vertex data
		    GL.EnableClientState( ArrayCap.VertexArray );
		    GL.BindBuffer( BufferTarget.ArrayBuffer, vertexBufferId );
		    GL.InterleavedArrays( InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf( typeof( ObjVertex ) ), IntPtr.Zero );

		    // link vertex attributes to shader parameters 
		    GL.VertexAttribPointer( shader.attribute_vuvs, 2, VertexAttribPointerType.Float, false, 32, 0 );
		    GL.VertexAttribPointer( shader.attribute_vnrm, 3, VertexAttribPointerType.Float, true, 32, 2 * 4 );
		    GL.VertexAttribPointer( shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, 32, 5 * 4 );

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray( shader.attribute_vpos );
            GL.EnableVertexAttribArray( shader.attribute_vnrm );
            GL.EnableVertexAttribArray( shader.attribute_vuvs );

		    // bind triangle index data and render
		    GL.BindBuffer( BufferTarget.ElementArrayBuffer, triangleBufferId );
		    GL.DrawArrays( PrimitiveType.Triangles, 0, triangles.Length * 3 );

		    // bind quad index data and render
		    if (quads.Length > 0)
		    {
			    GL.BindBuffer( BufferTarget.ElementArrayBuffer, quadBufferId );
			    GL.DrawArrays( PrimitiveType.Quads, 0, quads.Length * 4 );
		    }

		    // restore previous OpenGL state
		    GL.UseProgram( 0 );
	    }


	    // layout of a single vertex
	    [StructLayout(LayoutKind.Sequential)] public struct ObjVertex
	    {
		    public Vector2 TexCoord;
		    public Vector3 Normal;
		    public Vector3 Vertex;
	    }

	    // layout of a single triangle
	    [StructLayout(LayoutKind.Sequential)] public struct ObjTriangle
	    {
		    public int Index0, Index1, Index2;
	    }

	    // layout of a single quad
	    [StructLayout(LayoutKind.Sequential)] public struct ObjQuad
	    {
		    public int Index0, Index1, Index2, Index3;
	    }
    }
} // namespace Template_P3