using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aspectCamera : MonoBehaviour
{
    /*public float deviceWidth;
    public float deviceHeight;
    public float aspectRatio;
    public Camera mainCamera;

    private void Awake()
    {
        aspectRatio = deviceWidth / deviceHeight;
    }*/

    private void Update()
    {
        float targetAspectRatio = 2.17f; // 目?屏幕比例，例如 iPhone 11
        Rect safeArea = Screen.safeArea;

        float screenAspectRatio = (float)Screen.width / Screen.height;
        float viewportWidth = 1f;
        float viewportHeight = 1f;

        if (screenAspectRatio > targetAspectRatio)
        {
            // 如果屏幕比例更?，?根据高度?行?放
            viewportHeight = targetAspectRatio / screenAspectRatio;
        }
        else
        {
            // 如果屏幕比例更窄，?根据?度?行?放
            viewportWidth = screenAspectRatio / targetAspectRatio;
        }

        // 根据安全?域?行偏移
        float viewportX = (1f - viewportWidth) * 0.5f;
        float viewportY = (1f - viewportHeight) * 0.5f;

        // 考?安全?域的?距
        viewportX += safeArea.x / Screen.width * viewportWidth;
        viewportY += safeArea.y / Screen.height * viewportHeight;
        viewportWidth *= safeArea.width / Screen.width;
        viewportHeight *= safeArea.height / Screen.height;

        // ?置 Camera 的 Viewport Rect
        Camera.main.rect = new Rect(viewportX, viewportY, viewportWidth, viewportHeight);

    }
}