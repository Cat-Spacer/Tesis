using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private Vector3 _offset; 
    [SerializeField] private float _viewRadius;
    [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask _detectableAgentMask, _obstacleMask;
    [SerializeField] private Transform _pointer;

    private bool _onPlayerDetect = false;
    private bool _onCatDetect = false;
    private bool _onHamsterDetect = false;

    void Start()
    {
        EventManager.Instance.Subscribe("OnPunchPlayer", OnCatchPlayer);
    }
    void Update()
    {
        _fov.SetAimDirection(_pointer.forward);
        _fov.SetOrigin(_pointer.position);
        
        //FieldOfView();
    }
    // void FieldOfView()
    // {
    //     //Creamos un overlapsphere con el radio de nuestra vision
    //     var targetsInViewRadius = Physics2D.OverlapCircleAll(_pointer.position, _viewRadius, _detectableAgentMask);
    //     foreach (var item in targetsInViewRadius)
    //     {
    //         Vector3 dirToTarget = (item.transform.position - _pointer.transform.position);
    //     
    //         //preguntamos el angulo entre nuestro forward a la dir del target
    //         if (Vector3.Angle(_pointer.transform.right, dirToTarget.normalized) < _viewAngle / 2)
    //         {
    //             //Checkeo si no hay un obstaculo entre medio
    //             if (InSight(_pointer.transform.position, item.transform.position))
    //             {
    //                 Debug.DrawLine(_pointer.transform.position, item.transform.position, Color.red);
    //                 if (item.gameObject.GetComponent<CatChar>())_onCatDetect = true;
    //                 else if(item.gameObject.GetComponent<HamsterChar>()) _onHamsterDetect = true;
    //             }
    //             else
    //             {
    //                 if (item.gameObject.GetComponent<CatChar>())_onCatDetect = false;
    //                 else if(item.gameObject.GetComponent<HamsterChar>()) _onHamsterDetect = false;
    //             }
    //         }
    //     }
    //
    //     if (_onCatDetect == false && _onHamsterDetect == false || targetsInViewRadius.Length == 0)
    //     {
    //         _onPlayerDetect = false;
    //         _onCatDetect = false;
    //         _onHamsterDetect = false;
    //     }
    //     else
    //     {
    //         _onPlayerDetect = true;
    //     }
    // }

    void OnCatchPlayer(object[] obj)
    {
        var _onHit = (bool) obj[0];
        if (_onPlayerDetect && _onHit)
        {
            LiveCamera.instance.ChangePeace(-1);
        }
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
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.white;
    //     Gizmos.DrawWireSphere(_pointer.position, _viewRadius);
    //
    //     Vector3 lineA = DirFromAngle(_viewAngle / 2 + _pointer.eulerAngles.z);
    //     Vector3 lineB = DirFromAngle(-_viewAngle / 2 + _pointer.eulerAngles.z);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(_pointer.position, _pointer.position + lineA * _viewRadius);
    //     Gizmos.DrawLine(_pointer.position, _pointer.position + lineB * _viewRadius);
    // }
}
