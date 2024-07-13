using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierUtils
{
    /// <summary>
    /// 贝塞尔控制点对象
    /// </summary>
    public class BezierPoint
    {
        private readonly Vector3 _zero;

        private Vector3 _position;

        /// <summary>
        /// 曲线控制点的坐标
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (_position != null)
                {
                    return _position;
                }
                else
                {
                    return _zero;
                }
            }

            set
            {
                _position = value;
            }
        }

        private Vector3 _previousposition;

        /// <summary>
        /// 左控制点
        /// </summary>
        public Vector3 PreviousPosition
        {
            get
            {
                if (_previousposition != null)
                {
                    return _previousposition;
                }
                else
                {
                    return _zero;
                }
            }

            set
            {
                _previousposition = value;
            }
        }

        private Vector3 _latterposition;

        /// <summary>
        /// 右控制点
        /// </summary>
        public Vector3 LatterPosition
        {
            get 
            {
                if (_latterposition != null)
                {
                    return _latterposition;
                }
                else
                {
                    return _zero;
                }
            }

            set
            {
                _latterposition = value;
            }
        }

        /// <summary>
        /// 左（前）偏移目标点
        /// </summary>
        public Vector3 GetLeftHandle
        {
            get
            {
                Vector3 forward_direction = LatterPosition - PreviousPosition;
                forward_direction = forward_direction.normalized;

                //控制偏移量大小
                return Position - forward_direction * BezierManager.Instance.OffsetScale;
            }
        }

        /// <summary>
        /// 右（后）偏移目标点
        /// </summary>
        public Vector3 GetRightHandle
        {
            get
            {
                //左右偏移目标点 暂时整体镜像
                return -1.0f * GetLeftHandle;
            }
        }
    }
}
