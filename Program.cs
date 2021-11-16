using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using GLFW;
using Microsoft.VisualBasic.CompilerServices;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program {
        static float Lerp(float from, float to, float t) {
            return from + (to - from) * t;
        }

        static float GetRandomFloat(Random random, float min = 0, float max = 1) {
            return Lerp(min, max, (float)random.Next() / int.MaxValue);
        }
        
        static void Main(string[] args) {
            
            var window = new Window();
            var material = new Material("Shaders/world-position-color.vert", "Shaders/vertex-color.frag");
            var scene = new Scene();
            window.Load(scene);

            var shape = new Triangle(material);
            shape.Transform.CurrentScale = new Vector(0.5f, 1f, 1f);
            scene.Add(shape);

            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumberPerSecond = 30;
            const float FixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const float MovementSpeed = 0.5f;
            double previousFixedStep = 0.0;
            while (window.IsOpen()) {
                while (Glfw.Time > previousFixedStep + FixedDeltaTime) {
                    previousFixedStep += FixedDeltaTime;
                    var WalkingDirection = new Vector();
                    if (window.GetKey(Keys.W))
                    {
                        WalkingDirection += new Vector(0,1);
                    }
                    if (window.GetKey(Keys.A))
                    {
                        WalkingDirection += new Vector(-1,0f);
                    }
                    if (window.GetKey(Keys.S))
                    {
                        WalkingDirection += new Vector(0, -1);
                    }
                    if (window.GetKey(Keys.D))
                    {
                        WalkingDirection += new Vector(1,0);
                    }

                    WalkingDirection = WalkingDirection.Normalize();
                    shape.Transform.Position += WalkingDirection * MovementSpeed * FixedDeltaTime;
                }
                window.Render();
            }
        }
    }
}
