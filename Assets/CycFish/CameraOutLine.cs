using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraOutLine : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (this.enabled)
        {
            Color c = Gizmos.color;
            Gizmos.color = Color.yellow;
            Camera cam = this.GetComponent<Camera>();
            if (cam.orthographic)
            {
                Vector3 center = Vector3.back * (cam.nearClipPlane + cam.farClipPlane) * 0.5f;
                Vector3 size = new Vector3(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2, cam.farClipPlane - cam.nearClipPlane);

                Matrix4x4 m = Gizmos.matrix;
                Gizmos.matrix = cam.cameraToWorldMatrix;
                Gizmos.DrawWireCube(center, size);
                Gizmos.matrix = m;
            }
            else
            {
                float nearHalfHeight = halfHeight(cam.fieldOfView, cam.nearClipPlane);
                float farHalfHeight = halfHeight(cam.fieldOfView, cam.farClipPlane);
                Vector3 nearPos = -Vector3.forward * cam.nearClipPlane;
                Vector3 farPos = -Vector3.forward * cam.farClipPlane;

                float aspect = cam.aspect;

                Vector3 nearTopLeft = new Vector3(nearPos.x - nearHalfHeight * aspect, nearPos.y + nearHalfHeight, nearPos.z);
                Vector3 nearTopRight = nearTopLeft + new Vector3(2 * nearHalfHeight * aspect, 0, 0);
                Vector3 nearBottomLeft = nearTopLeft - new Vector3(0, 2 * nearHalfHeight, 0);
                Vector3 nearBottomRight = nearBottomLeft + new Vector3(2 * nearHalfHeight * aspect, 0, 0);


                Vector3 farTopLeft = new Vector3(farPos.x - farHalfHeight * aspect, farPos.y + farHalfHeight, farPos.z);
                Vector3 farTopRight = farTopLeft + new Vector3(2 * farHalfHeight * aspect, 0, 0);
                Vector3 farBottomLeft = farTopLeft - new Vector3(0, 2 * farHalfHeight, 0);
                Vector3 farBottomRight = farBottomLeft + new Vector3(2 * farHalfHeight * aspect, 0, 0);


                //                nearTopLeft = transPointFromCamToWorld(cam, nearTopLeft);
                //                nearTopRight = transPointFromCamToWorld(cam, nearTopRight);
                //                nearBottomLeft = transPointFromCamToWorld(cam, nearBottomLeft);
                //                nearBottomRight = transPointFromCamToWorld(cam, nearBottomRight);
                //
                //                farTopLeft = transPointFromCamToWorld(cam, farTopLeft);
                //                farTopRight = transPointFromCamToWorld(cam, farTopRight);
                //                farBottomLeft = transPointFromCamToWorld(cam, farBottomLeft);
                //                farBottomRight = transPointFromCamToWorld(cam, farBottomRight);

                Matrix4x4 m = Gizmos.matrix;
                Gizmos.matrix = cam.cameraToWorldMatrix;

                Gizmos.DrawLine(nearTopLeft, nearTopRight);
                Gizmos.DrawLine(nearTopRight, nearBottomRight);
                Gizmos.DrawLine(nearBottomRight, nearBottomLeft);
                Gizmos.DrawLine(nearBottomLeft, nearTopLeft);

                Gizmos.DrawLine(farTopLeft, farTopRight);
                Gizmos.DrawLine(farTopRight, farBottomRight);
                Gizmos.DrawLine(farBottomRight, farBottomLeft);
                Gizmos.DrawLine(farBottomLeft, farTopLeft);

                Gizmos.DrawLine(nearTopLeft, farTopLeft);
                Gizmos.DrawLine(nearTopRight, farTopRight);
                Gizmos.DrawLine(nearBottomLeft, farBottomLeft);
                Gizmos.DrawLine(nearBottomRight, farBottomRight);
                Gizmos.matrix = m;

            }
            Gizmos.color = c;
        }
    }
    private float halfHeight(float view, float far)
    {
        float w = Mathf.Tan(view * 0.5f * Mathf.Deg2Rad) * far;
        return w;
    }
    private Vector3 transPointFromCamToWorld(Camera cam, Vector3 point)
    {
        return cam.transform.TransformPoint(point);
    }
}


