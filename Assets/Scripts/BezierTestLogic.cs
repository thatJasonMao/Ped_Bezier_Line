using BezierUtils;
using System.Collections.Generic;
using UnityEngine;

public class BezierTestLogic : MonoBehaviour
{
    public static Vector3 DP1 = new Vector3 (0, 0, 0);
    public static Vector3 DP2 = new Vector3 (5, 0, 5);
    public static Vector3 DP3 = new Vector3 (0, 0, 10);
    public static Vector3 DP4 = new Vector3 (5, 0, 15);

    List<Vector3> DPs = new List<Vector3> { DP1, DP2, DP3, DP4 };
    List<Vector3> TestResults = new List<Vector3> ();

    void Start()
    {
        BezierManager.Instance.SetBezierSourcePoints(DPs, 50, 1f);
        TestResults = BezierManager.Instance.GenerateBezierCurvePoints();
    }

    void FixUpdate()
    {
        
    }

    protected virtual void OnDrawGizmos()
    {
        if (TestResults.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 v in DPs)
            {
                Gizmos.DrawSphere(v, 0.25f);
            }

            Gizmos.color= Color.green;
            for (int i = 0; i < TestResults.Count - 1; i++)
            {
                Gizmos.DrawLine(TestResults[i], TestResults[i + 1]);
            }

            Gizmos.color = Color.blue;
            for (int i = 0; i < BezierManager.SourcePoints.Count; i++)
            { 
                BezierPoint bp = BezierManager.SourcePoints[i];
                Gizmos.DrawLine(bp.GetLeftHandle, bp.GetRightHandle);
            }
        }
    }
}
