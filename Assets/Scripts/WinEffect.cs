using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinEffect : MonoBehaviour
{
    public GameObject diamond;
    Vector3 diamondScale;

    public GameObject effect;
    float rotationSpeed = 10f;

    void Start()
    {
        diamondScale = diamond.transform.localScale;
        StartCoroutine(DiamondGetBigger());
    }

    
    void Update()
    {
        effect.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
    }

    IEnumerator DiamondGetBigger()
    {
        while (diamondScale.x < 3)
        {
            diamondScale *= 1.01f;
            diamond.transform.localScale = diamondScale;

            yield return null;
        }
    }
}
