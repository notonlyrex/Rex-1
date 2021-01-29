using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
    public static class CustomMath
    {

        public static double ConvertRadiansToDegrees(double radians)
        {
            return (180 / Math.PI) * radians;
        }

        public static double ConvertDegreesToRadian(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }



        public static double SimpleAngleBetweenTwoVectors(Vector3 a, Vector3 b)
        {
            
            //return MathF.Acos((float)SimpleDotProduct(a,b)/(a.Length()*b.Length()));

            float c;

            c = MathF.Acos((float)Vector3.Dot(Vector3.Normalize(a), Vector3.Normalize(b)));


    
            if (ConvertRadiansToDegrees(MathF.Atan2(b.Z - a.Z, b.X - a.X)) < 90)
                c = -c;
            
                return c;
        }

        public static Vector3 SimpleRotateVectorByDegree(Vector3 a, float r)
        {

            var tempx = a.X;
            var tempz = a.Z;

            float rr = (float)ConvertDegreesToRadian(r);

            var CameraRotation2D_X = tempx * MathF.Cos(rr) - tempz * MathF.Sin(rr);

            var CameraRotation2D_Z = tempx * MathF.Sin(rr) + tempz * MathF.Cos(rr);

            return new Vector3(CameraRotation2D_X, a.Y, CameraRotation2D_Z);

        }

        public static Vector3 SimpleRotateVectorByRadian(Vector3 a, float r)
        {

            var tempx = a.X;
            var tempz = a.Z;

            var CameraRotation2D_X = tempx * MathF.Cos(r) - tempz * MathF.Sin(r);

            var CameraRotation2D_Z = tempx * MathF.Sin(r) + tempz * MathF.Cos(r);

            return new Vector3(CameraRotation2D_X, a.Y, CameraRotation2D_Z);

        }
    }
}
