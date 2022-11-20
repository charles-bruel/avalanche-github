using System;
using System.Collections.Generic;
using UnityEngine;

namespace VarietyPack
{
    public class Utils
    {
        public static Vector3 GetCaternery(Vector2 p1, Vector2 p2, float sag)
        {
            float totalDistance = (p1 - p2).magnitude;
            float a, b, c;//Answer goes here
            Vector2 midpoint = (p2 - p1) / 2;
            float L = totalDistance * (1 + 0.01f * sag);
            Vector2 d = (p2 - p1);
            //Setup done, do math now

            float r = Mathf.Sqrt(L * L - d.y * d.y) / d.x;
            //no real way to do this next step, newtons method to the rescue!
            double a0;
            if (r < 3)
            {
                a0 = Math.Sqrt(6 * (r - 1));
            }
            else
            {
                a0 = Math.Log(2 * r, Math.E) + Math.Log(Math.Log(2 * r, Math.E), Math.E);//Super ugly
            }
            double an = a0;
            double an1;
            for (int i = 0; i < 5; i++)
            {
                an1 = an - (Math.Sinh(an) - r * an) / (Math.Cosh(an) - r);
                an = an1;
            }
            float A = (float)an;

            a = d.x / (2 * A);
            b = midpoint.x - a * (float)ATanh(d.y / L);
            c = midpoint.y - L / (2 * (float)Math.Tanh(A));

            return new Vector3(a, b, c);
        }

        private static double ATanh(double x)
        {
            return (Math.Log(1 + x) - Math.Log(1 - x)) / 2;
        }

        public static double EvalCaternery(double a, double b, double c, double x)
        {
            return a * Math.Cosh((x - b) / a) + c;
        }

        public static double EvalCaterneryPrime(double a, double b, double c, double x)
        {
            return Math.Sinh((x - b) / a);//Analytical derivative
        }
    }
}
