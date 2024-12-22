using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        /// <param name="t">curve progress</param>
        /// <param name="p0">start position</param>
        /// <param name="p1">control position</param>
        /// <param name="p2">end position</param>
        public static Vector3 BezierCurvePos(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
        }

        public static float ApproxBezierCurveLength(int segments, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float length = 0;
            Vector3 previousPoint = p0;

            for (int i = 1; i <= segments; i++)
            {
                float t = (float)i / segments;
                Vector3 point = BezierCurvePos(t, p0, p1, p2);
                length += Vector3.Distance(previousPoint, point);
                previousPoint = point;
            }
            
            return length;
        }
    }
}