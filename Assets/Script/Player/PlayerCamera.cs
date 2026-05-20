using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] Transform _player;
    [SerializeField] Vector3 _cameraOffset = new Vector3 (0, 3, -10);

    private float _maxX;
    private float _minX;
    private float _maxY;
    private float _minY;

    private void Awake()
    {
        SetCam();
    }

    void LateUpdate()
    {
        if (_cam == null)
        {
            _cam = Camera.main;

            if (_cam == null)
            {
                return;
            }
        }
        LookPlayer();
    }

    void LookPlayer()
    {
        Vector3 pos = _player.position + _cameraOffset;

        float height = _cam.orthographicSize;
        float width = height * _cam.aspect;

        pos.x = Mathf.Clamp(pos.x, _minX + width, _maxX - width);
        pos.y = Mathf.Clamp(pos.y, _minY + height, _maxY - height);
        pos.z = _cameraOffset.z;
        _cam.transform.position = pos;
    }

    public void SetBound(BoxCollider2D limit)
    {
        Bounds bounds = limit.bounds;
        _maxX = bounds.max.x;
        _minX = bounds.min.x;
        _maxY = bounds.max.y;
        _minY = bounds.min.y;
    }

    void SetCam()
    {
        _cam = Camera.main;
    }
}
