using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] EScenes scene;
    [SerializeField] TMP_Text text;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float minAlpha = 0.5f;
    [SerializeField] private float maxAlpha = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneChanger.Instance.MoveScene(scene);
        }

        float t = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }
}
