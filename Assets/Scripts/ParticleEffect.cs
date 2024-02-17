using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public float minScale = 0.5f; // 최소 크기
    public float maxScale = 2f; // 최대 크기
    public float scaleSpeed = 1f; // 크기 변경 속도

    private bool isGrowing = true; // 현재 오브젝트가 커지고 있는지 여부
    float randomValue;

    private void Start()
    {
        randomValue = Random.Range(0f, 1f);
        if (randomValue > 0.5f)
        {
            isGrowing = true;
        }
        else
        {
            isGrowing = false;
        }
    }

    void Update()
    {
        Vector3 currentScale = transform.localScale;

        if (currentScale.x <= minScale || currentScale.x >= maxScale)
        {
            isGrowing = !isGrowing;
        }

        float scaleFactor = isGrowing ? 1f : -1f;
        float newScale = currentScale.x + scaleFactor * scaleSpeed * Time.deltaTime;

        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
