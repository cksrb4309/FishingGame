using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Sirenix.OdinInspector;
using UnityEditor;

public class SplineResetPosition : MonoBehaviour
{
    [Button]
    void ResetContainerPosition()
    {
        SplineContainer splineContainer = GetComponent<SplineContainer>();
        if (splineContainer == null)
        {
            Debug.LogWarning("SplineContainer component not found!");
            return;
        }

        // 현재 Transform의 위치값 저장
        Vector3 offset = splineContainer.transform.position;

        // 모든 스플라인 점 좌표 이동
        for (int i = 0; i < splineContainer.Splines.Count; i++)
        {
            var spline = splineContainer.Splines[i];
            for (int j = 0; j < spline.Count; j++)
            {
                BezierKnot knot = spline[j];
                // 위치를 오프셋만큼 이동
                knot.Position += (float3)offset;
                spline[j] = knot;
            }
        }

        // Transform을 원점으로 되돌림
        splineContainer.transform.position = Vector3.zero;

        // 에디터에 변경사항 반영
        EditorUtility.SetDirty(splineContainer);
        EditorUtility.SetDirty(splineContainer.transform);
    }
}
