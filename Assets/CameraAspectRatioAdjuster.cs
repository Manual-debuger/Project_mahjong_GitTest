using UnityEngine;

public class CameraAspectRatioAdjuster : MonoBehaviour
{
    public float targetAspectRatio = 18f / 9f; // 以18:9為目標長寬比
    public bool isSafeAreaInViewport = true;
    private Camera mainCamera = null;
    public GameObject safeArea = null;
    RectTransform Panel;

    private void Awake()
    {
        // originFOV = cam.fieldOfView;
        mainCamera = GetComponent<Camera>();
        safeArea = GameObject.Find("SafeArea");
        Panel = safeArea.GetComponent<RectTransform>();
    }   

    void Update()
    {
        var camera = mainCamera;
        float currentAspectRatio = (float)Screen.width / Screen.height;
        float scaleFactor = currentAspectRatio / targetAspectRatio;
        
        //Debug.Log("scaleFactor: " + scaleFactor);
        if (scaleFactor < 1f)
        {
            Rect rect = camera.rect;

            rect.x = 0;
            rect.y = (1.0f - scaleFactor) / 2.0f;
            rect.width = 1.0f; 
            rect.height = 1.0f - rect.y;

            if (Panel.anchorMin.y > rect.y) // If (1.0f - scaleFactor) / 2.0f < safe aera Min y
            {
                rect.width = 1.0f;
                rect.height = 1 - 2 * Panel.anchorMin.y; // 1 - 2 * safe area Min Y
                rect.x = Panel.anchorMin.x; // safe area Min X
                rect.y = Panel.anchorMin.y; // safe area Min y
                isSafeAreaInViewport = true;
            }
            else
            {
                isSafeAreaInViewport = false;
            }

            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleFactor;

            Rect rect = camera.rect;

            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            rect.width = 1.0f - rect.x;
            rect.height = 1.0f;

            if (Panel.anchorMin.x > rect.x) // If (1.0f - scaleFactor) / 2.0f < safe aera Min x
            {
                rect.width = 1 - 2 * Panel.anchorMin.x; // 1 - 2 * safe area Min X
                rect.height = 1.0f;
                rect.x = Panel.anchorMin.x; // safe area Min X
                rect.y = Panel.anchorMin.y; // safe area Min y
                isSafeAreaInViewport = true;
            }
            else
            {
                isSafeAreaInViewport = false;
            }

            camera.rect = rect;
        }
    }
}