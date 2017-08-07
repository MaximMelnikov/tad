using UnityEngine;

public class TAScroll : MonoBehaviour
{
    public static TAScroll Instance;
    [HideInInspector]
    public float topBound;
    [HideInInspector]
    public float downBound;
    [SerializeField]
    private float slowdownCoeffitient = 0.6f;

    private float velocity;
    private bool dragging;
    private float transformHorPos;
    private float clickPos;
    private float dragDistance;
    private float previousFramePos;
    private RectTransform rectTransform;

    private const int framerate = 20;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Application.targetFrameRate = framerate;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            dragging = true;
            transformHorPos = rectTransform.anchoredPosition.y;
            clickPos = Input.mousePosition.y;
            previousFramePos = clickPos;
        }
        if (dragging && Input.GetMouseButtonUp(0)) {
            dragging = false;
            velocity = clickPos - previousFramePos;
        }
        if ( dragging ) {
            Application.targetFrameRate = 60;
            previousFramePos = Input.mousePosition.y;
            if (rectTransform.anchoredPosition.y >= downBound && rectTransform.anchoredPosition.y <= topBound) {
                dragDistance = previousFramePos - clickPos;
            }
            rectTransform.anchoredPosition = new Vector3(0, Mathf.Clamp(transformHorPos - dragDistance, downBound, topBound), -999);
        }
        else if( velocity > 0.01f ) {
            Application.targetFrameRate = 60;
            if (rectTransform.anchoredPosition.y >= downBound && rectTransform.anchoredPosition.y <= topBound) {
                velocity = velocity * slowdownCoeffitient;
            }
            rectTransform.anchoredPosition = new Vector3(0, Mathf.Clamp(rectTransform.anchoredPosition.y + velocity, downBound, topBound), -999);
        }
        else {
            Application.targetFrameRate = framerate;
        }
    }
}