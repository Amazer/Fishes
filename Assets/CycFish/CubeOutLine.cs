using UnityEngine;
using System.Collections;
public class CubeOutLine : MonoBehaviour
{
    public Transform tr;
    public void Start()
    {
        tr = this.transform;
    }
    private void OnDrawGizmos()
    {
        if (this.enabled)
        {
            if (tr == null)
            {
                tr = this.transform;
            }
            Color c = Gizmos.color;
            Gizmos.color = Color.green;
            Matrix4x4 m = Gizmos.matrix;
            Gizmos.matrix = tr.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = m;
            Gizmos.color = c;

        }

    }
}
