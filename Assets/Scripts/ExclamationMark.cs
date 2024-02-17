using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    public GameObject UpCube;
    private Vector3 originScale;
    private float curScale;
    private bool isAdd = false;
    private bool isOrigin = false;
    private bool isMove = true;

    void Start()
    {
        originScale = UpCube.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if (!isAdd)
            {
                curScale = UpCube.transform.localScale.y * 0.99f;

                if (curScale < originScale.y / 2f)
                {
                    isAdd = true;
                }

                if (isOrigin && curScale < originScale.y)
                {
                    isOrigin = false;
                    isMove = false;
                }

            }
            else
            {
                curScale = UpCube.transform.localScale.y * 1.01f;

                if (curScale > originScale.y * 1.5f)
                {
                    isOrigin = true;
                    isAdd = false;
                }
            }

            UpCube.transform.localScale = new Vector3(originScale.x, curScale, originScale.z);

        }
    }
}
