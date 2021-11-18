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
            
            var random = new Random();
            var window = new Window();
            var material = new Material("Shaders/world-position-color.vert", "Shaders/vertex-color.frag");
            var scene = new Scene();
            var physics = new Physics(scene);
            window.Load(scene);

            for (var i = 0; i < 30; i++) {
                var circle = new Circle(material);
                var radius = GetRandomFloat(random, 0.3f);
                circle.Transform.CurrentScale = new Vector(radius, radius, 1f);
                circle.Transform.Position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1), 0f);
                circle.velocity = -circle.Transform.Position.Normalize() * GetRandomFloat(random, 0.15f, 0.3f);
                circle.Mass = MathF.PI * radius * radius;
                scene.Add(circle);
            }
            
            // var ground = new Rectangle(material);
            // ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            // ground.Transform.Position = new Vector(0f, -1f);
            // ground.gravityScale = 0;
            // ground.Mass = 1f;
            // scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumberPerSecond = 60;
            const float FixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const float MovementSpeed = 0.5f;
            const int maxStepsPerFrame = 5;
            var previousFixedStep = 0.0;
            while (window.IsOpen()) {
                var stepCount = 0;
                while (Glfw.Time > previousFixedStep + FixedDeltaTime && stepCount++ < maxStepsPerFrame) {
                    previousFixedStep += FixedDeltaTime;
                    physics.Update(FixedDeltaTime);
                   
                }
                window.Render();
            }
        }
    }
}
