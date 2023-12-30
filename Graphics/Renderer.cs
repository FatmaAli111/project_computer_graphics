﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(0.5f, 0.5f, 0.4f, 0.5f); // 0 , 0 , 0.4 , 0 
            
            float[] triangleVertices= { 
		       
                   // Triangle 1

             1.0f*30 , 0.0f*30 ,0.0f, 
                           0.0f , 0.0f , 0.0f ,
             0.9f*30 , 0.1f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,
             0.85f*30 , -0.1f*30 ,0.0f, 
                           0.0f , 0.0f , 0.0f ,

                   // Triangle 2

              0.9f*30 , 0.1f*30 ,0.0f,
                            0.0f , 0.0f , 0.0f ,
              0.67f*30 , -0.06f*30 ,0.0f,
                            0.0f , 0.0f , 0.0f ,
             0.85f*30 , -0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,

                   // Triangle 3

             1.0f*30 ,0.0f*30 ,0.0f ,
                            0.0f , 0.0f , 0.0f ,
             0.85f*30 , -0.1f*30 ,0.0f,
                            0.0f , 0.0f , 0.0f ,
             0.85f*30 , -0.2f*30 ,0.0f,
                            0.0f , 0.0f , 0.0f ,
 
                   // Triangle 4

             0.67f*30 , -0.06f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.85f*30 , -0.2f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.85f*30 , -0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,

                   // Triangle 5

             0.9f*30 , 0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.8f*30 , 0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.67f *30, -0.06f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f , 

                     // Triangle 6

             0.9f*30 , 0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.8f*30 , 0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.8f *30, 0.2f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                  // Triangle 7

             0.72f*30 , 0.25f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.75f*30 , 0.15f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.8f*30 , 0.2f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                   // Triangle 8

             0.72f*30 , 0.25f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.8f*30 , 0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.67f*30 , -0.06f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,
            
                  // Triangle 9

             0.72f*30 , 0.25f *30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.63f*30 , 0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.67f*30 , -0.06f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 10

             0.72f*30 , 0.25f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.25f*30 , 0.35f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.63f *30, 0.1f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 11

             0.37f*30 , -0.08f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.25f*30 , 0.35f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.63f*30 , 0.1f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 12

             0.37f*30 , -0.08f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.52f*30 , -0.41f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.63f *30, 0.1f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                  // Triangle 13

             0.57f*30 , -0.11f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.67f *30, -0.06f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.63f *30, 0.1f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,
 
                   // Triangle 14

             0.57f *30, -0.11f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.67f *30, -0.06f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.52f *30, -0.41f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                  // Triangle 15

             0.85f *30, -0.2f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.67f*30 , -0.06f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.52f *30, -0.41f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 16

             0.37f *30, -0.08f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.52f *30, -0.41f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.36f *30, -0.39f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 17

             0.37f*30 , -0.08f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.24f *30, -0.1f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.36f *30, -0.4f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 18

             0.37f *30, -0.08f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.25f *30, 0.35f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.11f*30 , 0.34f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 19

              0.24f *30, -0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.36f *30, -0.39f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.3f*30 , -0.36f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,              

                // Triangle 20

             0.37f*30 , -0.08f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.24f*30 , -0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.11f *30, 0.34f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 21

             -0.23f *30, 0.16f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.24f *30, -0.1f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.11f*30 , 0.34f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 22

             -0.23f*30 , 0.16f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.24f*30 , -0.1f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.3f*30 , -0.36f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 23

             -0.23f*30 , 0.16f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.11f*30 , 0.34f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.25f*30 , 0.33f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 24

             -0.23f*30 , 0.16f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.7f*30 , 0.17f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.25f*30 , 0.33f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 25

             -0.23f*30 , 0.16f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.7f*30 , 0.17f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.3f*30 , -0.36f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 26

             -0.67f*30 , -0.33f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.7f*30 , 0.17f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.3f*30 , -0.36f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 27                      

             -0.67f*30 , -0.33f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.7f*30 , 0.17f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.82f*30 , -0.35f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 28

             -1.0f*30 , 0.35f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.7f*30 , 0.17f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.82f*30 , -0.35f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 29

             -0.67f*30 , -0.33f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.9f*30 , -0.7f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.82f *30, -0.35f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 30

             0.52f *30, -0.41f *30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.36f*30 , -0.39f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.4f*30 , -0.9f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                  // Triangle 31

             0.52f *30, -0.41f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.4f *30, -0.9f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.43f*30 , -1.0f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 32

             0.25f*30 , 0.35f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.11f*30 , 0.34f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.01f*30 , 0.6f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 33

             0.25f*30 , 0.35f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.1f*30 , 0.7f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.01f*30 , 0.6f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 34

             0.0f *30, 0.8f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             0.1f *30 , 0.7f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.01f *30, 0.6f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 35
     
             -0.3f *30, -0.36f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.4f*30 , -0.34f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.35f*30 , -0.45f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                 // Triangle 36
     
             -0.6f *30 , -0.32f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.5f*30 , -0.33f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.58f *30, -0.4f *30,0.0f,
                          0.0f , 0.0f , 0.0f ,

                // Triangle 37
     
             -0.6f *30 , 0.2f*30 ,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.55f *30 , 0.3f *30,0.0f,
                           0.0f , 0.0f , 0.0f ,
             -0.5f*30 , 0.23f*30 ,0.0f,
                          0.0f , 0.0f , 0.0f ,
            }; 
            // Triangle Center = (10, 7, -5)
            
            triangleCenter = new vec3(10, 7, -5);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f, 
		        0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z  
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,  
		        0.0f, 0.0f, -100.0f,
                0.0f, 0.0f, 1.0f,  
            };


            triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(50, 50, 50), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated Triangle
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 111);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        

        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1),  triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
