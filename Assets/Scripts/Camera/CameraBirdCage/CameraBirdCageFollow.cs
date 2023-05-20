using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBirdCageFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float maxCameraRange;
    public float minCameraRange;
    public Vector3 edgeRange;
    public float smoothFactor;
    public LayerMask playerLayerMask;
    public Vector3 worldPosition;
    Plane plane = new Plane(Vector3.forward, 0);



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        MousePosition();
        EdgeScrolling();
    }
    void EdgeScrolling()
    {
        //float distance = Vector3.Distance(new Vector3 (worldPosition.x, worldPosition.y, 0), new Vector3(player.position.x, player.position.y, 0));
        float distance = Vector3.Distance(worldPosition,player.position);
        if (distance > maxCameraRange)
        {
            Vector3 fromOriginToObject = worldPosition - player.position;
            fromOriginToObject *= maxCameraRange / distance;
            worldPosition = player.position + fromOriginToObject;
        }
        var newPos = Vector3.Lerp(transform.position, worldPosition, smoothFactor * Time.deltaTime);
        newPos.z = offset.z;
        transform.position = newPos;
    }
    void MousePosition()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
            worldPosition.z = offset.z;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(player.position, maxCameraRange);
        Gizmos.DrawWireSphere(player.position, minCameraRange);
    }
}
