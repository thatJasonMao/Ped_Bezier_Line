using UnityEngine;

namespace BezierUtils
{
    /// <summary>
    /// 贝塞尔控制点对象
    /// </summary>
    public class BezierPoint
    {
        /// <summary>
        /// 曲线控制点的坐标
        /// </summary>
        public Vector3 Position
        {
            get; set;
        }

        /// <summary>
        /// 左控制点
        /// </summary>
        public Vector3 PreviousPosition
        {
            get; set;
        }

        /// <summary>
        /// 右控制点
        /// </summary>
        public Vector3 LatterPosition
        {
            get; set;
        }

        /// <summary>
        /// 左（前）偏移目标点
        /// </summary>
        public Vector3 GetLeftHandle
        {
            get
            {
                return Position - ForwardDirection() * BezierManager.OffsetScale;
            }
        }

        /// <summary>
        /// 右（后）偏移目标点
        /// </summary>
        public Vector3 GetRightHandle
        {
            get
            {
                return Position + ForwardDirection() * BezierManager.OffsetScale;
            }
        }

        public Vector3 ForwardDirection()
        {
            return (LatterPosition - PreviousPosition).normalized;
        }
    }
}
