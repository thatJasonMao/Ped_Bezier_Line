using UnityEngine;

namespace BezierUtils
{
    /// <summary>
    /// 贝塞尔曲线计算
    /// </summary>
    public class BezierLine
    {
        private static BezierLine instance;

        public static BezierLine Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                { 
                    return new BezierLine();
                }
            }
        }
        
        public Vector3 GetPoint(float time)
        {
            BezierPoint startPoint;
            BezierPoint endPoint;
            float timeRelativeToSegment;

            GetCubicSegment(time, out startPoint, out endPoint, out timeRelativeToSegment);

            return BezierLine.GetPointOnCubicCurve(timeRelativeToSegment, startPoint, endPoint);
        }

        public float GetApproximateLength()
        {
            float length = 0;
            int subCurveSampling = (BezierManager.Sampling / (BezierManager.Instance.GetBezierPointCount() - 1)) + 1;
            for (int i = 0; i < BezierManager.Instance.GetBezierPointCount() - 1; i++)
            {
                length += BezierLine.GetApproximateLengthOfCubicCurve(BezierManager.SourcePoints[i], BezierManager.SourcePoints[i + 1], subCurveSampling);
            }

            return length;
        }

        public void GetCubicSegment(float time, out BezierPoint startPoint, out BezierPoint endPoint, out float timeRelativeToSegment)
        {
            startPoint = null;
            endPoint = null;
            timeRelativeToSegment = 0f;

            float subCurvePercent = 0f;
            float totalPercent = 0f;
            float approximateLength = this.GetApproximateLength();
            int subCurveSampling = (BezierManager.Sampling / (BezierManager.Instance.GetBezierPointCount() - 1)) + 1;

            for (int i = 0; i < BezierManager.Instance.GetBezierPointCount() - 1; i++)
            {
                subCurvePercent = BezierLine.GetApproximateLengthOfCubicCurve(BezierManager.SourcePoints[i], BezierManager.SourcePoints[i + 1], subCurveSampling) / approximateLength;
                if (subCurvePercent + totalPercent > time)
                {
                    startPoint = BezierManager.SourcePoints[i];
                    endPoint = BezierManager.SourcePoints[i + 1];

                    break;
                }

                totalPercent += subCurvePercent;
            }

            if (endPoint == null)
            {
                startPoint = BezierManager.SourcePoints[BezierManager.Instance.GetBezierPointCount() - 2];
                endPoint = BezierManager.SourcePoints[BezierManager.Instance.GetBezierPointCount() - 1];

                totalPercent -= subCurvePercent;
            }

            timeRelativeToSegment = (time - totalPercent) / subCurvePercent;
        }

        public static Vector3 GetPointOnCubicCurve(float time, BezierPoint startPoint, BezierPoint endPoint)
        {
            return GetPointOnCubicCurve(time, startPoint.Position, endPoint.Position, startPoint.GetRightHandle, endPoint.GetLeftHandle);
        }

        public static Vector3 GetPointOnCubicCurve(float time, Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent)
        {
            float t = time;
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t3 = t2 * t;

            Vector3 result =
                (u3) * startPosition +
                (3f * u2 * t) * startTangent +
                (3f * u * t2) * endTangent +
                (t3) * endPosition;

            return result;
        }

        public static float GetApproximateLengthOfCubicCurve(BezierPoint startPoint, BezierPoint endPoint, int sampling)
        {
            return GetApproximateLengthOfCubicCurve(startPoint.Position, endPoint.Position, startPoint.GetRightHandle, endPoint.GetLeftHandle, sampling);
        }

        public static float GetApproximateLengthOfCubicCurve(Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, int sampling)
        {
            float length = 0f;
            Vector3 fromPoint = GetPointOnCubicCurve(0f, startPosition, endPosition, startTangent, endTangent);

            for (int i = 0; i < sampling; i++)
            {
                float time = (i + 1) / (float)sampling;
                Vector3 toPoint = GetPointOnCubicCurve(time, startPosition, endPosition, startTangent, endTangent);
                length += Vector3.Distance(fromPoint, toPoint);
                fromPoint = toPoint;
            }

            return length;
        }
    }
}
