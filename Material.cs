using System.IO;
using static OpenGL.Gl;

namespace SharpEngine {
    public class Material
    {
        readonly uint program;
        
        public Material(string vertexShaderPath, string fragmentShaderPath) {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("Shaders/position-color.vert.glsl"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("Shaders/vertex-color.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
            glDeleteShader(vertexShader);
            glDeleteShader(fragmentShader);
        }

        public unsafe void SetTransform(Matrix matrix)
        {
            int transformlocation = glGetUniformLocation(this.program, "transform");
           
            glUniformMatrix4fv(transformlocation,1,true, &matrix.m11);
        }

        public void Use() {
            glUseProgram(program);
        }

    }
}
