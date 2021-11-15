using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;


namespace SharpEngine
{
    public class Triangle
    {
        Vertex[] vertices;
        uint vertexArray;
        uint vertexBuffer;

        public float CurrentScale { get; private set; }

        public Material material;
            
        public Triangle(Vertex[] vertices, Material material) {
            this.vertices = vertices;
            this.material = material;
            LoadTriangleIntoBuffer();
            this.CurrentScale = 1f;
        }
        
        unsafe void LoadTriangleIntoBuffer() {
            vertexArray = glGenVertexArray();
            vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glBindVertexArray(0);
        }

        public void Move(Vector direction)
        {
                
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += direction;
            }
        }
            
        public unsafe void Render() {
            this.material.Use();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, this.vertexBuffer);
            fixed (Vertex* vertex = &this.vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length);
            glBindVertexArray(0);
        }

        public Vector GetMinBounds()
        {
            var min = vertices[0].position;
            for (int i = 1; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].position);
            }

            return min;
        }

        public Vector GetMaxBounds()
        {
            var max = vertices[0].position;
            for (int i = 1; i < vertices.Length; i++)
            {
                max = Vector.Max(max, vertices[i].position);
            }

            return max;
        }

        public Vector GetCenter()
        {
            return (GetMaxBounds() + GetMinBounds()) / 2;
        }
        public void Scale(float multiplier)
        {
            var center = GetCenter();

            Move(center*-1);
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].position *= multiplier;
            }
            Move(center);

            this.CurrentScale *= multiplier;

        }
        
        public void Rotate(float rotation) {
            var center = GetCenter();
            Move(center*-1);
            for (int i = 0; i < this.vertices.Length; i++) {
                var currentRotation = Vector.Angle(this.vertices[i].position);
                var distance = vertices[i].position.GetMagnitude();
                var newX = MathF.Cos(currentRotation + rotation);
                var newY = MathF.Sin(currentRotation + rotation);
                vertices[i].position = new Vector(newX, newY) * distance;
            }
            Move(center);
        }    


    }
}