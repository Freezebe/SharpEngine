using System;
using System.IO;
using System.Numerics;
using GLFW;
using Microsoft.VisualBasic.CompilerServices;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    
    struct Vector
    {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector Max(Vector a, Vector b)
        {
            return new Vector(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y), MathF.Max(a.z, b.z));
        }
        public static Vector Min(Vector a, Vector b)
        {
            return new Vector(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y), MathF.Min(a.z, b.z));
        }

        public static Vector operator *(Vector v, float f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }
        
        public static Vector operator +(Vector v, Vector u)
        {
            return new Vector(v.x + u.x, v.y + u.y, v.z + u.z);
        }
        
        public static Vector operator -(Vector v, Vector u)
        {
            return new Vector(v.x - u.x, v.y - u.y, v.z - u.z);
        }
        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }
        
        

    }

    
    class Program
    {
        
        static Vector[] vertices = new Vector[]
        {
            new Vector(-.1f, -.1f),
            new Vector(.1f, -.1f),
            new Vector(0f, .1f),
            //new Vector(.4f, .4f),
            //new Vector(.6f, .4f),
            //new Vector(.5f, .6f)
        };

        private const int VertexX = 0;
        private const int VertexY = 1;
        private const int VertexSize = 3;
        
        static void Main(string[] args)
        {
           
             // initialize and configure
             var window = CreateWindow();

             LoadTriangleIntoBuffer();

             CreateShaderProgram();

            // engine rendering loop
            var direction = new Vector(0.002f, 0.001f);
            var scale = 1f;
            var multiplier = 0.999f;
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
              

                var min = vertices[0];
                for (int i = 1; i < vertices.Length; i++)
                {
                    min = Vector.Min(min, vertices[i]);
                }
                var max = vertices[0];
                for (int i = 1; i < vertices.Length; i++)
                {
                    max = Vector.Max(max, vertices[i]);
                }

                var center = (max + min) / 2;
                
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i] -= center;
                }
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i] *= multiplier;
                }
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += center;
                }

                scale *= multiplier;

                if (scale <= 0.5f)
                {
                    multiplier = 1.001f;
                }

                if (scale >= 2)
                {
                    multiplier = 0.999f;
                }
                
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += direction;
                }

                for (var i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i].x >= 1 && direction.x > 0|| vertices[i].x <= -1 && direction.x < 0)
                    {
                        direction.x *= -1.01f;
                        break;
                    }
                }
                
                
                for (var i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i].y >= 1 && direction.y > 0 || vertices[i].y <=-1 && direction.y < 0)
                    {
                        direction.y *= -1.01f;
                        break;
                    }
                }
                
                
                UpdateTriangleBuffer();
            }
        }

        static void DownMovement()
        {
            for (var i = VertexY; i < vertices.Length; i += VertexSize)
            {
                vertices[i].y -= 0.001f;
            }
        }
        
        static void RightMovement()
        {
            for (var i = VertexX; i < vertices.Length; i += VertexSize)
            {
                vertices[i].x += 0.001f;
            }
        }

        static void Shrink()
        {
            for (int iteration = 0; iteration < vertices.Length; iteration++)
            {
                vertices[iteration].x *= 0.9999f;
                vertices[iteration].y *= 0.9999f;
            }
            
            
        }
        
        static void ScaleUp()
        {
            for (int iteration = 0; iteration < vertices.Length; iteration ++)
            {
                vertices[iteration].x *= 1.001f;
                vertices[iteration].y *= 1.001f;
            }
            
            
        }
        
        static void ScaleUpY()
        {
            for (int iteration = VertexY; iteration < vertices.Length; iteration += VertexSize)
            {
                vertices[iteration].y *= 1.0001f;
            }
            
            
        }

        static void Render(Window window)
        {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            Glfw.SwapBuffers(window);
            //glFlush();
        }

        private static void ClearScreen()
        {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        static unsafe void LoadTriangleIntoBuffer()
        {
            // load the vertices into a buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            
                fixed (Vector* vertex = &vertices[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_STATIC_DRAW);
                }

                glVertexAttribPointer(0, VertexSize, GL_FLOAT, false,  sizeof(Vector), NULL);
                glEnableVertexAttribArray(0);
        }
        static unsafe void UpdateTriangleBuffer() {
            fixed (Vector* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }

        private static void CreateShaderProgram()
        {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("Shaders/screen-coordinates.vert.glsl"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("Shaders/green.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        static Window CreateWindow()
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}
