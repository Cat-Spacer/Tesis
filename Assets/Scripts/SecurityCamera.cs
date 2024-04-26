using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _offset; 
    [SerializeField] private float _viewRadius;
    [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask _detectableAgentMask, _obstacleMask;
    [SerializeField] private Transform _pointer;
    
    void Start()
    {
        
    }


    void Update()
    {
        FieldOfView();
    }
    void FieldOfView()
    {
        //Creamos un overlapsphere con el radio de nuestra vision
        var targetsInViewRadius = Physics2D.OverlapCircle(_pointer.position, _viewRadius, _detectableAgentMask);

        if (targetsInViewRadius != null)
        {
            Vector3 dirToTarget = (targetsInViewRadius.transform.position - _pointer.position);
            
            //preguntamos el angulo entre nuestro forward a la dir del target
            if (Vector3.Angle(_pointer.right, dirToTarget.normalized) < _viewAngle / 2)
            {
                //Checkeo si no hay un obstaculo entre medio
                if (InSight(_pointer.position, targetsInViewRadius.transform.position))
                {
                    Debug.DrawLine(_pointer.position, targetsInViewRadius.transform.position, Color.red);
                }
                else
                {

                }
            }
        }
        // foreach (var item in targetsInViewRadius)
        // {
        //     Vector3 dirToTarget = (item.transform.position - transform.position);
        //
        //     //preguntamos el angulo entre nuestro forward a la dir del target
        //     if (Vector3.Angle(transform.right, dirToTarget.normalized) < _viewAngle / 2)
        //     {
        //         //Checkeo si no hay un obstaculo entre medio
        //         if (InSight(transform.position, item.transform.position))
        //         {
        //             Debug.DrawLine(transform.position, item.transform.position, Color.red);
        //         }
        //         else
        //         {
        //
        //         }
        //     }
        // } 
    }
    bool InSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        if (!Physics2D.Raycast(start, dir, dir.magnitude, _obstacleMask)) return true;
        else return false;
    }
    Vector3 DirFromAngle(float angle)
    {
        //return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        //return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_pointer.position, _viewRadius);

        Vector3 lineA = DirFromAngle(_viewAngle / 2 + _pointer.eulerAngles.z);
        Vector3 lineB = DirFromAngle(-_viewAngle / 2 + _pointer.eulerAngles.z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_pointer.position, _pointer.position + lineA * _viewRadius);
        Gizmos.DrawLine(_pointer.position, _pointer.position + lineB * _viewRadius);
    }
}
