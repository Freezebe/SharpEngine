﻿using System;
using System.IO;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static float[] vertices = new float[]
        {
            //vertex 1
            -.1f, -.1f, 0f,
            //vertex 2
            .1f, -.1f, 0f,
            //vertex 3
            0f, .1f, 0f,
            //vertex 4
            .4f, .4f, 0f,
            //vertex 5
            .6f, .4f, 0f,
            //vertex 6
            .5f, .6f, 0f
            
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
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render();
                ScaleUp();
                UpdateTriangleBuffer();
            }
        }

        static void DownMovement()
        {
            for (var i = VertexY; i < vertices.Length; i += VertexSize)
            {
                vertices[i] -= 0.0001f;
            }
        }
        
        static void RightMovement()
        {
            for (var i = VertexX; i < vertices.Length; i += VertexSize)
            {
                vertices[i] += 0.0001f;
            }
        }

        static void Shrink()
        {
            for (int iteration = 0; iteration < vertices.Length; iteration++)
            {
                vertices[iteration] *= 0.9999f;
            }
            
            
        }
        
        static void ScaleUp()
        {
            for (int iteration = 0; iteration < vertices.Length; iteration ++)
            {
                vertices[iteration] *= 1.0001f;
            }
            
            
        }
        
        static void ScaleUpY()
        {
            for (int iteration = VertexY; iteration < vertices.Length; iteration += VertexSize)
            {
                vertices[iteration] *= 1.0001f;
            }
            
            
        }

        static void Render()
        {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length/VertexSize);
            glFlush();
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
            unsafe
            {
                fixed (float* vertex = &vertices[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
                }

                glVertexAttribPointer(0, VertexSize, GL_FLOAT, false, VertexSize * sizeof(float), NULL);
            }

            glEnableVertexAttribArray(0);
        }
        static unsafe void UpdateTriangleBuffer() {
            fixed (float* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
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
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}
