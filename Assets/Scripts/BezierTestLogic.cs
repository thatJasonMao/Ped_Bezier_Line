using BezierUtils;
using System.Collections.Generic;
using UnityEngine;

public class BezierTestLogic : MonoBehaviour
{
    public static Vector3 DP1 = new Vector3 (0, 0, 0);
    public static Vector3 DP2 = new Vector3 (0, 0, 5);
    public static Vector3 DP3 = new Vector3 (0, 0, 10);
    public static Vector3 DP4 = new Vector3 (0, 0, 15);
    List<Vector3> DPs = new List<Vector3> { DP1, DP2, DP3, DP4 };

    List<Vector3> TestResults = new List<Vector3> ();

    public float FittingStrength = 1.0f;
    public float ShakeScale = 0.1f;
    public float ShakeSpeed = 2.0f;

    public int FittingAccuracy = 50;

    public bool isDynamicDebug;


    void Start()
    {
        BezierManager.Instance.SetBezierSourcePoints(DPs, FittingAccuracy, FittingStrength);
        TestResults = BezierManager.Instance.GenerateBezierCurvePoints();
        isDynamicDebug = true;
    }

    void FixedUpdate()
    {
        if (isDynamicDebug)
        {
            DPs[1] = DPs[1] + new Vector3 (ShakeScale * Mathf.Sin(ShakeSpeed * Time.time) ,0, 0);
            DPs[2] = DPs[2] - new Vector3 (ShakeScale * Mathf.Sin(ShakeSpeed * Time.time), 0, 0);
            BezierManager.Instance.SetBezierSourcePoints(DPs, FittingAccuracy, FittingStrength);
            TestResults = BezierManager.Instance.GenerateBezierCurvePoints();
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (TestResults.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 v in DPs)
            {
                Gizmos.DrawSphere(v, 0.35f);
            }

            Gizmos.color= Color.green;
            for (int i = 0; i < TestResults.Count - 1; i++)
            {
                Gizmos.DrawLine(TestResults[i], TestResults[i + 1]);
            }

            for (int i = 0; i < BezierManager.SourcePoints.Count; i++)
            {
                Gizmos.color = Color.blue;
                BezierPoint bp = BezierManager.SourcePoints[i];
                Gizmos.DrawLine(bp.GetLeftHandle, bp.GetRightHandle);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(bp.GetLeftHandle, 0.15f);
                Gizmos.DrawSphere(bp.GetRightHandle, 0.15f);
            }
        }
    }
}
