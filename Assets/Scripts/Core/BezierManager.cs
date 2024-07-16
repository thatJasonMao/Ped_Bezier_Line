using System.Collections.Generic;
using UnityEngine;

namespace BezierUtils
{
    /// <summary>
    /// 贝塞尔曲线接口
    /// </summary>
    public class BezierManager
    {
        private static BezierManager instance;

        /// <summary>
        /// 单例 用于全局接口
        /// </summary>
        public static BezierManager Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    return new BezierManager();
                }
            }
        }

        /// <summary>
        /// 曲线采样密度
        /// </summary>
        public static int Sampling;

        /// <summary>
        /// 拟合偏移量
        /// </summary>
        public static float OffsetScale;

        /// <summary>
        /// 贝塞尔曲线控制点集合
        /// </summary>
        public static List<BezierPoint> SourcePoints;

        /// <summary>
        /// 设置贝塞尔曲线控制点的接口
        /// </summary>
        /// <param name="v3_points"></param>
        public void SetBezierSourcePoints(List<Vector3> v3_points, int i_sampling, float f_offsetscale)
        {
            //暂时仅考虑三个点以上的case
            if (v3_points.Count > 3)
            {
                Sampling = i_sampling;
                OffsetScale = f_offsetscale;

                SourcePoints = new List<BezierPoint>(v3_points.Count);

                for (int i = 0; i < v3_points.Count; i++)
                { 
                    BezierPoint new_bp = new BezierPoint();
                    new_bp.Position = v3_points[i];

                    if (i == 0)
                    {
                        new_bp.LatterPosition = v3_points[i + 1];
                        
                        //对于队列中首个点 给它镜像一个
                        Vector3 latter_offset = new_bp.LatterPosition - new_bp.Position;
                        Vector3 mirror_point = new_bp.Position - latter_offset;
                        new_bp.PreviousPosition = mirror_point;
                    }

                    else if (i == v3_points.Count - 1)
                    {
                        new_bp.PreviousPosition = v3_points[i - 1];

                        //对于队列中最后一个点 给它镜像一个
                        Vector3 previous_offset = new_bp.PreviousPosition - new_bp.Position;
                        Vector3 mirror_point = new_bp.Position - previous_offset;
                        new_bp.LatterPosition = mirror_point;
                    }

                    else 
                    {
                        new_bp.LatterPosition = v3_points[i + 1];
                        new_bp.PreviousPosition = v3_points[i - 1];
                    }

                    SourcePoints.Add(new_bp);
                }
            }
        }

        public void ClearBezierSourcePoints()
        {
            SourcePoints.Clear();
        }

        /// <summary>
        /// 返回贝塞尔曲线插值点
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GenerateBezierCurvePoints()
        {
            List<Vector3> results = new List<Vector3>();

            for (int i = 0; i < Sampling + 1; i++)
            {
                float time = i / (float)Sampling;

                Vector3 CurvePoint = BezierLine.Instance.GetPoint(time);
                results.Add(CurvePoint);
            }

            return results;
        }

        public int GetBezierPointCount()
        { 
            return SourcePoints.Count; 
        }
    }
}
