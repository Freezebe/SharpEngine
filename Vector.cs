using System;

namespace SharpEngine
{
    public struct Vector
    {
        public float x, y, z;

        public static Vector Forward => new Vector(0, 1);
        public static Vector Backward => new Vector(0, -1);
        public static Vector Right => new Vector(1, 0);
        public static Vector Left => new Vector(-1, 0);
        public static Vector Zero => new Vector();

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
        
        public static float Angle(Vector v) {
            return MathF.Atan2(v.y, v.x);
        }

        public float GetMagnitude() {
            return MathF.Sqrt(x * x + y * y + z * z);
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
        
        public static Vector operator -(Vector v) {
            return new Vector(-v.x, -v.y, -v.z);
        }
        
        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }

        public Vector Normalize()
        {
            var magnitude = GetMagnitude();
            return magnitude > 0 ? this / GetMagnitude() : this;
        }
        
        

    }
}