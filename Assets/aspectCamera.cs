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
        float targetAspectRatio = 2.17f; // ��?�̹���ҡA�Ҧp iPhone 11
        Rect safeArea = Screen.safeArea;

        float screenAspectRatio = (float)Screen.width / Screen.height;
        float viewportWidth = 1f;
        float viewportHeight = 1f;

        if (screenAspectRatio > targetAspectRatio)
        {
            // �p�G�̹���ҧ�?�A?���u����?��?��
            viewportHeight = targetAspectRatio / screenAspectRatio;
        }
        else
        {
            // �p�G�̹���ҧ󯶡A?���u?��?��?��
            viewportWidth = screenAspectRatio / targetAspectRatio;
        }

        // ���u�w��?��?�氾��
        float viewportX = (1f - viewportWidth) * 0.5f;
        float viewportY = (1f - viewportHeight) * 0.5f;

        // ��?�w��?�쪺?�Z
        viewportX += safeArea.x / Screen.width * viewportWidth;
        viewportY += safeArea.y / Screen.height * viewportHeight;
        viewportWidth *= safeArea.width / Screen.width;
        viewportHeight *= safeArea.height / Screen.height;

        // ?�m Camera �� Viewport Rect
        Camera.main.rect = new Rect(viewportX, viewportY, viewportWidth, viewportHeight);

    }
}