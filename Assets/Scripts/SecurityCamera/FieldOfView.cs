using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //[SerializeField] private float _fov, angle, _viewDistance;
    //[SerializeField] private int _rayCount;
    [SerializeField] private LayerMask _playerLayerMask;
    Vector3 origin = Vector3.zero;
    private Mesh _mesh;
    float fov;
    private float startingAngle;
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        fov = 90f;
    }

    void LateUpdate()
    {
        int rayCount = 30;
        float angle = 36f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 25f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int [rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2d = Physics2D.Raycast(origin, GetVectorFormAngle(angle), viewDistance, _playerLayerMask);
            if (raycastHit2d.collider == null)
            {
                vertex = origin + GetVectorFormAngle(angle) * viewDistance;
            }
            else
            {
                vertex = raycastHit2d.point;
            }
            
            vertices[vertexIndex] = vertex;
            
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }
        
        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFormVectorFloat(aimDirection) - fov / 2f;
    }
    private Vector3 GetVectorFormAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    float GetAngleFormVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
