using ConsoleGameEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace RexMinus1
{
    public struct Triangle
    {
        public Vector3[] p;

        public int color;
        public ConsoleCharacter c;

        public Triangle(Vector3 pa, Vector3 pb, Vector3 pc)
        {
            p = new Vector3[3];
            p[0] = pa;
            p[1] = pb;
            p[2] = pc;

            color = 0;
            c = ConsoleCharacter.Null;
        }

        public Triangle(object n)
        {
            p = new Vector3[3];
            color = 0;
            c = ConsoleCharacter.Null;
        }

        public void Translate(Vector3 delta)
        {
            p[0] += delta;
            p[1] += delta;
            p[2] += delta;
        }

        public Triangle MatMul(Matrix m)
        {
            Triangle t = new Triangle(null);
            t.p[0] = Matrix.MultiplyVector(p[0], m);
            t.p[1] = Matrix.MultiplyVector(p[1], m);
            t.p[2] = Matrix.MultiplyVector(p[2], m);

            return t;
        }
    }

    public struct Mesh
    {
        public Vector3[] Vertices { get; set; }
        public Triangle[] Triangles { get; set; }

        public static Mesh LoadFromObj(string filename)
        {
            StreamReader s = new StreamReader(filename);

            List<Vector3> verts = new List<Vector3>();
            List<Triangle> tris = new List<Triangle>();

            while (!s.EndOfStream)
            {
                string line = s.ReadLine();

                if (line[0] == 'v')
                {
                    Vector3 v = new Vector3();
                    string[] str = line.Replace('.', ',').Split();
                    v.X = float.Parse(str[1]);
                    v.Y = float.Parse(str[2]);
                    v.Z = float.Parse(str[3]);
                    verts.Add(v);
                }
                if (line[0] == 'f')
                {
                    Triangle t = new Triangle();
                    string[] str = line.Split();
                    t.p = new Vector3[3];
                    t.color = 6;

                    for (int i = 1; i <= 3; i++)
                    {
                        if (str[i].Contains("//"))
                            str[i] = str[i].Substring(0, str[i].IndexOf("//"));
                    }

                    t.p[0] = verts[Convert.ToInt32(str[1]) - 1];
                    t.p[1] = verts[Convert.ToInt32(str[2]) - 1];
                    t.p[2] = verts[Convert.ToInt32(str[3]) - 1];
                    tris.Add(t);
                }
            }

            var result = new Mesh();

            result.Vertices = verts.ToArray();
            result.Triangles = tris.ToArray();

            return result;
        }
    }

    public class Matrix
    {
        public float[,] m = new float[4, 4];

        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix mat = new Matrix();
            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    mat.m[r, c] = a.m[r, 0] * b.m[0, c] + a.m[r, 1] * b.m[1, c] + a.m[r, 2] * b.m[2, c] + a.m[r, 3] * b.m[3, c];
                }
            }
            return mat;
        }

        public static Vector3 MultiplyVector(Vector3 i, Matrix m)
        {
            Vector3 v = new Vector3();
            float w = 0;
            v.X = i.X * m.m[0, 0] + i.Y * m.m[1, 0] + i.Z * m.m[2, 0] + m.m[3, 0];
            v.Y = i.X * m.m[0, 1] + i.Y * m.m[1, 1] + i.Z * m.m[2, 1] + m.m[3, 1];
            v.Z = i.X * m.m[0, 2] + i.Y * m.m[1, 2] + i.Z * m.m[2, 2] + m.m[3, 2];
            w = i.X * m.m[0, 3] + i.Y * m.m[1, 3] + i.Z * m.m[2, 3] + m.m[3, 3];

            if (w != 0.0f)
            {
                v.X /= w; v.Y /= w; v.Z /= w;
            }

            return v;
        }

        public static Matrix Translation(Vector3 t)
        {
            Matrix mat = new Matrix();
            mat.m[0, 0] = 1.0f;
            mat.m[1, 1] = 1.0f;
            mat.m[2, 2] = 1.0f;
            mat.m[3, 3] = 1.0f;
            mat.m[3, 0] = t.X;
            mat.m[3, 1] = t.Y;
            mat.m[3, 2] = t.Z;
            return mat;
        }

        public static Matrix Identity()
        {
            Matrix mat = new Matrix();
            mat.m[0, 0] = 1.0f; mat.m[0, 1] = 1.0f; mat.m[0, 2] = 1.0f; mat.m[0, 3] = 1.0f;
            mat.m[1, 0] = 1.0f; mat.m[1, 1] = 1.0f; mat.m[1, 2] = 1.0f; mat.m[1, 3] = 1.0f;
            mat.m[2, 0] = 1.0f; mat.m[2, 1] = 1.0f; mat.m[2, 2] = 1.0f; mat.m[2, 3] = 1.0f;
            mat.m[3, 0] = 1.0f; mat.m[3, 1] = 1.0f; mat.m[3, 2] = 1.0f; mat.m[3, 3] = 1.0f;
            return mat;
        }

        public static Matrix LookAtLH(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 xaxis, yaxis, zaxis;
            zaxis = Vector3.Subtract(target, eye); zaxis = Vector3.Normalize(zaxis);
            xaxis = Vector3.Cross(up, zaxis); xaxis = Vector3.Normalize(xaxis);
            yaxis = Vector3.Cross(zaxis, xaxis);

            Matrix result = Matrix.Identity();
            result.m[0, 0] = xaxis.X; result.m[1, 0] = xaxis.Y; result.m[2, 0] = xaxis.Z;
            result.m[0, 1] = yaxis.X; result.m[1, 1] = yaxis.Y; result.m[2, 1] = yaxis.Z;
            result.m[0, 2] = zaxis.X; result.m[1, 2] = zaxis.Y; result.m[2, 2] = zaxis.Z;

            result.m[3, 0] = Vector3.Dot(xaxis, eye);
            result.m[3, 1] = Vector3.Dot(yaxis, eye);
            result.m[3, 2] = Vector3.Dot(zaxis, eye);

            result.m[3, 0] = -result.m[3, 0];
            result.m[3, 1] = -result.m[3, 1];
            result.m[3, 2] = -result.m[3, 2];

            return result;
        }

        public static Matrix PointAtMatrix(Vector3 pos, Vector3 target, Vector3 up)
        {
            // ny frammåt
            Vector3 newForward = target - pos;
            newForward = Vector3.Normalize(newForward);

            // ny uppåt
            Vector3 a = newForward * Vector3.Dot(up, newForward);
            Vector3 newUp = up - a;
            newUp = Vector3.Normalize(newUp);

            Vector3 newRight = Vector3.Cross(newUp, newForward);
            Matrix mat = new Matrix();
            mat.m[0, 0] = newRight.X; mat.m[0, 1] = newRight.Y; mat.m[0, 2] = newRight.Z; mat.m[0, 3] = 0.0f;
            mat.m[1, 0] = newUp.X; mat.m[1, 1] = newUp.Y; mat.m[1, 2] = newUp.Z; mat.m[1, 3] = 0.0f;
            mat.m[2, 0] = newForward.X; mat.m[2, 1] = newForward.Y; mat.m[2, 2] = newForward.Z; mat.m[2, 3] = 0.0f;
            mat.m[3, 0] = pos.X; mat.m[2, 1] = pos.Y; mat.m[2, 2] = pos.Z; mat.m[2, 3] = 1.0f;
            return mat;
        }

        public static Matrix ProjectionMatrix(float fov, float aspect, float near, float far)
        {
            Matrix mat = new Matrix();
            float fovRad = 1.0f / (float)Math.Tan(fov * 0.5f / (180 / (float)Math.PI));
            mat.m[0, 0] = aspect * fovRad;
            mat.m[1, 1] = fovRad;
            mat.m[2, 2] = far / (far - near);
            mat.m[3, 2] = (-far * near) / (far - near);
            mat.m[2, 3] = 1.0f;
            mat.m[3, 3] = 0.0f;

            return mat;
        }

        // Endast för rotation/translations matriser
        public static Matrix QuickInverse(Matrix m)
        {
            Matrix matrix = new Matrix();
            matrix.m[0, 0] = m.m[0, 0]; matrix.m[0, 1] = m.m[1, 0]; matrix.m[0, 2] = m.m[2, 0]; matrix.m[0, 3] = 0.0f;
            matrix.m[1, 0] = m.m[0, 1]; matrix.m[1, 1] = m.m[1, 1]; matrix.m[1, 2] = m.m[2, 1]; matrix.m[1, 3] = 0.0f;
            matrix.m[2, 0] = m.m[0, 2]; matrix.m[2, 1] = m.m[1, 2]; matrix.m[2, 2] = m.m[2, 2]; matrix.m[2, 3] = 0.0f;
            matrix.m[3, 0] = -(m.m[3, 0] * matrix.m[0, 0] + m.m[3, 1] * matrix.m[1, 0] + m.m[3, 2] * matrix.m[2, 0]);
            matrix.m[3, 1] = -(m.m[3, 0] * matrix.m[0, 1] + m.m[3, 1] * matrix.m[1, 1] + m.m[3, 2] * matrix.m[2, 1]);
            matrix.m[3, 2] = -(m.m[3, 0] * matrix.m[0, 2] + m.m[3, 1] * matrix.m[1, 2] + m.m[3, 2] * matrix.m[2, 2]);
            matrix.m[3, 3] = 1.0f;
            return matrix;
        }

        public static Matrix RotationMatrixX(float fAngleRad)
        {
            Matrix mat = new Matrix();
            mat.m[0, 0] = 1.0f;
            mat.m[1, 1] = (float)Math.Cos(fAngleRad);
            mat.m[1, 2] = (float)Math.Sin(fAngleRad);
            mat.m[2, 1] = (float)-Math.Sin(fAngleRad);
            mat.m[2, 2] = (float)Math.Cos(fAngleRad);
            mat.m[3, 3] = 1.0f;
            return mat;
        }

        public static Matrix RotationMatrixY(float fAngleRad)
        {
            Matrix mat = new Matrix();
            mat.m[0, 0] = (float)Math.Cos(fAngleRad);
            mat.m[0, 2] = (float)Math.Sin(fAngleRad);
            mat.m[2, 0] = (float)-Math.Sin(fAngleRad);
            mat.m[1, 1] = 1.0f;
            mat.m[2, 2] = (float)Math.Cos(fAngleRad);
            mat.m[3, 3] = 1.0f;
            return mat;
        }

        public static Matrix RotationMatrixZ(float fAngleRad)
        {
            Matrix mat = new Matrix();
            mat.m[0, 0] = (float)Math.Cos(fAngleRad);
            mat.m[0, 1] = (float)Math.Sin(fAngleRad);
            mat.m[1, 0] = (float)-Math.Sin(fAngleRad);
            mat.m[1, 1] = (float)Math.Cos(fAngleRad);
            mat.m[2, 2] = 1.0f;
            mat.m[3, 3] = 1.0f;
            return mat;
        }
    }
}