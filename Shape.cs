using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;


namespace SharpEngine
{
    public class Shape
    {
        Vertex[] vertices;
        uint vertexArray;
        uint vertexBuffer;

        public Transform Transform { get; }
        public float CurrentScale { get; set; }
        public Vector velocity;
        public Vector linearForce;
        public float gravityScale =1;
        float mass = 1;
        float massInverse = 1;
        public float Mass {
            get => this.mass;
            set {
                this.mass = value;
                this.massInverse = float.IsPositiveInfinity(value) ? 0f : 1f / value;
            }
        }
        public float MassInverse => this.massInverse;
        public Material material;
            
        public Shape(Vertex[] vertices, Material material) {
            this.vertices = vertices;
            this.material = material;
            LoadTriangleIntoBuffer();
            this.Transform = new Transform();
        }

        public void SetColor(Color color)
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].color = color;
            }
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

        
            
        public unsafe void Render() {
            this.material.Use();
            this.material.SetTransform(this.Transform.Matrix);
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
            var min = this.Transform.Matrix * vertices[0].position;
            for (int i = 1; i < vertices.Length; i++)
            {
                min = Vector.Min(min,this.Transform.Matrix * vertices[i].position);
            }

            return min;
        }

        public Vector GetMaxBounds()
        {
            var max = this.Transform.Matrix * vertices[0].position;
            for (int i = 1; i < vertices.Length; i++)
            {
                max = Vector.Max(max, this.Transform.Matrix * vertices[i].position);
            }

            return max;
        }

        public Vector GetCenter()
        {
            return (GetMaxBounds() + GetMinBounds()) / 2;
        }

    }
}