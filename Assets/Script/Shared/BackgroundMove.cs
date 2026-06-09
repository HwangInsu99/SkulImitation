using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform[] _backgrounds;
    [SerializeField] private float _clash = 0.3f;

    private float _width;

    private void Awake()
    {
        _cam = Camera.main.transform;
        SpriteRenderer renderer = _backgrounds[0].GetComponent<SpriteRenderer>();

        _width = renderer.bounds.size.x - _clash;
    }

    void LateUpdate()
    {
        foreach (Transform bg in _backgrounds)
        {
            Vector3 pos = bg.position;
            pos.y = _cam.position.y;
            bg.position = pos;

            float diff = _cam.position.x - bg.position.x;

            if (diff > _width)
            {
                MoveRight(bg);
            }
            else if (diff < -_width)
            {
                MoveLeft(bg);
            }
        }
    }

    void MoveRight(Transform bg)
    {
        Vector3 pos = GetRightMax();

        bg.position = new Vector3(pos.x + _width, _cam.position.y, bg.position.z);
    }

    void MoveLeft(Transform bg)
    {
        Vector3 pos = GetLeftMax();

        bg.position = new Vector3(pos.x - _width, _cam.position.y, bg.position.z);
    }

    Vector3 GetRightMax()
    {
        Transform result = _backgrounds[0];

        foreach (Transform bg in _backgrounds)
        {
            if (bg.position.x > result.position.x)
            {
                result = bg;
            }
        }

        return result.position;
    }

    Vector3 GetLeftMax()
    {
        Transform result = _backgrounds[0];

        foreach (Transform bg in _backgrounds)
        {
            if (bg.position.x < result.position.x)
            {
                result = bg;
            }
        }

        return result.position;
    }
}
