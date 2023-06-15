using UnityEngine;

public class PlayerUnstuck : MonoBehaviour
{
    [SerializeField] private CustomMovement _customMovement;
    [SerializeField] private Transform _colPos;
    [SerializeField] private Vector2 _colRange;
    [SerializeField] private LayerMask _unstuckLayer;
    [SerializeField] private bool _showGizmos;

    private void Start()
    {
        if (_colPos != null)
            _customMovement = GetComponent<CustomMovement>();
    }

    private void Update()
    {
        DontAdvance();
    }

    private void DontAdvance()
    {
        var detect = Physics2D.OverlapBox(_colPos.position, _colRange, 1, _unstuckLayer);
        if (detect)
        {
            if (PlayerDatas.faceDirection > 0)
                PlayerInput.canRightMove = false;
            else if (PlayerDatas.faceDirection < 0)
                PlayerInput.canLeftMove = false;
        }
        else
        {
            PlayerInput.canRightMove = true;
            PlayerInput.canLeftMove = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (_showGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_colPos.transform.position, _colRange);
        }
    }
}