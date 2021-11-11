using OpenGL;

namespace SharpEngine
{
    public class Triangle
    {
        private Vertex[] vertices;
        public float CurrentScale ;
        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
            CurrentScale = 1f;
        }

        public void Move(Vector direction)
        {
                
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += direction;
            }
        }
            
        public unsafe void Render()
        {
                
            fixed (Vertex* vertex = &vertices[0]) {
                Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, Gl.GL_STATIC_DRAW);
            }
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, vertices.Length);
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


    }
}