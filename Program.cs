using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using GLFW;
using Microsoft.VisualBasic.CompilerServices;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
      
        private static Triangle triangle = new Triangle(
            new Vertex[]
            {
                new Vertex(new Vector(-.1f, -.1f), Color.Red),
                new Vertex(new Vector(.1f, -.1f), Color.Green),
                new Vertex(new Vector(0f, .1f), Color.Blue),

            }

        );
        

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
            float multiplier = 0.999f;
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);


                triangle.Scale(multiplier);

                if (triangle.CurrentScale <= 0.5f)
                {
                    multiplier = 1.001f;
                }

                if (triangle.CurrentScale >= 2)
                {
                    multiplier = 0.999f;
                }
                //moves the triangle
                triangle.Move(direction);
                
                
                
                    
                if(triangle.GetMaxBounds().x >=1 && direction.x >0 || triangle.GetMinBounds().x <= -1 && direction.x<0)
                {
                    direction.x *= -1.01f;
                        
                    
                }
                
                
                if(triangle.GetMaxBounds().y >=1 && direction.y >0 || triangle.GetMinBounds().y <= -1 && direction.y <0)
                {
                    direction.y *= -1.01f;
                    
                    
                }
                
                
                
            }
        }

        

        static void Render(Window window)
        {
            triangle.Render();
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
            
                glVertexAttribPointer(0, 3, GL_FLOAT, false,  sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
                glVertexAttribPointer(1, 4, GL_FLOAT, false,  sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
                glEnableVertexAttribArray(0);
                glEnableVertexAttribArray(1);
        }
        
        
       

        private static void CreateShaderProgram()
        {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("Shaders/position-color.vert.glsl"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("Shaders/vertex-color.frag"));
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
