using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    //[SerializeField] private Vector3 _offset; 
    // [SerializeField] private float _viewRadius;
    // [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask _detectableAgentMask, _obstacleMask;
    
    [SerializeField] private GameObject _fovObj;
    private FieldOfView _fieldOfView;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _fov, _viewDistance;
    
    private bool _onPlayerDetect = false;
    private bool _onCatDetect = false;
    private bool _onHamsterDetect = false;

    void Start()
    {
        EventManager.Instance.Subscribe("OnPunchPlayer", OnCatchPlayer);
        _fieldOfView = Instantiate(_fovObj, null).GetComponent<FieldOfView>();

        _fieldOfView.SetFoV(_fov);
        _fieldOfView.SetViewDistance(_viewDistance);
    }
    void Update()
    {
        _fieldOfView.SetAimDirection(_camera.up);
        _fieldOfView.SetOrigin(_camera.transform.position);

        FieldOfView();
    }
    void FieldOfView()
    {
        //Creamos un overlapsphere con el radio de nuestra vision
        var targetsInViewRadius = Physics2D.OverlapCircleAll(_camera.position, _viewDistance, _detectableAgentMask);
        foreach (var item in targetsInViewRadius)
        {
            Vector3 dirToTarget = (item.transform.position - _camera.transform.position);
        
            //preguntamos el angulo entre nuestro forward a la dir del target
            if (Vector3.Angle(_camera.transform.right, dirToTarget.normalized) < _fov / 2)
            {
                //Checkeo si no hay un obstaculo entre medio
                if (InSight(_camera.transform.position, item.transform.position))
                {
                    Debug.DrawLine(_camera.transform.position, item.transform.position, Color.blue);
                    if (item.gameObject.GetComponent<CatChar>())_onCatDetect = true;
                    else if(item.gameObject.GetComponent<HamsterChar>()) _onHamsterDetect = true;
                }
                else
                {
                    if (item.gameObject.GetComponent<CatChar>())_onCatDetect = false;
                    else if(item.gameObject.GetComponent<HamsterChar>()) _onHamsterDetect = false;
                }
            }
        }
    
        if (_onCatDetect == false && _onHamsterDetect == false || targetsInViewRadius.Length == 0)
        {
            _onPlayerDetect = false;
            _onCatDetect = false;
            _onHamsterDetect = false;
        }
        else
        {
            _onPlayerDetect = true;
        }
    }

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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(_camera.position, _viewDistance);
    
        Vector3 lineA = DirFromAngle(_fov / 2 + _camera.eulerAngles.z);
        Vector3 lineB = DirFromAngle(-_fov / 2 + _camera.eulerAngles.z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_camera.position, _camera.position + lineA * _viewDistance);
        Gizmos.DrawLine(_camera.position, _camera.position + lineB * _viewDistance);
    }
}
