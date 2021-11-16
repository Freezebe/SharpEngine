﻿using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;


namespace SharpEngine
{
    public class Transform
    {
        Vertex[] vertices;
        public Vector Position { get;  set; }
        public Vector CurrentScale { get;  set; }
        public Vector Rotation { get; set; }
        public Matrix Matrix => Matrix.Translation(Position)* Matrix.Rotation(Rotation) * Matrix.Scale(CurrentScale);

        public Transform()
        {
            this.CurrentScale = new Vector(1, 1, 1);
        }

        public void Move(Vector direction)
        {
            this.Position += direction;
        }
        
        public void Scale(float multiplier)
        {
            CurrentScale *= multiplier;
            
        }

        public void Rotate(float zAngle)
        {
            var rotation = this.Rotation;
            rotation.z += zAngle;

            this.Rotation = rotation;

        }    


    }
}