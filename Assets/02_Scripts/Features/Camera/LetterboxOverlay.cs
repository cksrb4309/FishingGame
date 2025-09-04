using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera)), ExecuteAlways]
public class LetterboxOverlay : MonoBehaviour
{
    public float targetWidth = 16f;      // 기본 가로 기준
    public float minHeight = 9f;         // 최소 보장할 세로 유닛
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        float windowAspect = (float)Screen.width / Screen.height;

        // 가로 기준 orthographicSize 계산
        float sizeBasedOnWidth = targetWidth / windowAspect / 2f;

        // 최소 세로 보장
        float minSize = minHeight / 2f;

        // 최종 카메라 Size
        cam.orthographicSize = Mathf.Max(sizeBasedOnWidth, minSize);
    }

    void Update()
    {
        // 해상도 변경 시 대응
        UpdateCameraSize();
    }
}
