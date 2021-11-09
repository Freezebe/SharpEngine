using System;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
                Glfw.Init();
               

                var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
                Glfw.MakeContextCurrent(window);
                Import(Glfw.GetProcAddress);

                float[] verticies = new float[]
                {
                    -.5f, -.5f, 0f,
                    .5f, -.5f, 0f,
                     0f, .5f, 0f

                };

                var vertexArray = glGenVertexArray();
                var vertexBuffer = glGenBuffer();
                glBindVertexArray(vertexArray);
                glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
                unsafe
                {
                    fixed (float* vertex = &verticies[0])
                    {
                        glBufferData(GL_ARRAY_BUFFER, sizeof(float) * verticies.Length, vertex, GL_STATIC_DRAW);
                    } 
                    glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), NULL);
                }
                glEnableVertexAttribArray(0);
                
                while (!Glfw.WindowShouldClose(window))
                {
                    Glfw.PollEvents();
                    glDrawArrays(GL_TRIANGLES,0,3);
                    Glfw.SwapBuffers(window);
                }
            
        }
    }
}
