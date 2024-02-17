using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDrop : MonoBehaviour
{
    float time = 0;
    public GameObject textGAMEOVER;
    public GameObject popUP;
    bool isAnim = false;

    private Vector3 targetPos = new Vector3(0, 10, 80);
    private void Start()
    {
        popUP.SetActive(false);
    }
    void Update()
    {

        if(isAnim)
        {
            if (time < 0.4f)
            {
                textGAMEOVER.transform.position = new Vector3(0, 32 - 80 * time, 0) + targetPos;
            }
            else if (time < 0.5f)
            {
                textGAMEOVER.transform.position = new Vector3(0, 12 * (time - 0.4f), 0) + targetPos;
            }
            else if (time < 0.6f)
            {
                textGAMEOVER.transform.position = new Vector3(0, 12 * (0.6f - time), 0) + targetPos;
            }
            else if (time < 0.7f)
            {
                textGAMEOVER.transform.position = new Vector3(0, (time - 0.6f) * 4, 0)+ targetPos;
            }
            else if (time < 0.8f)
            {
                textGAMEOVER.transform.position = new Vector3(0, 0.05f - (time - 0.7f) * 4, 0) + targetPos;
            }
            else if(time < 2)
            {
                textGAMEOVER.transform.position = targetPos;
            }
            else
            {
                textGAMEOVER.gameObject.SetActive(false);
                popUP.gameObject.SetActive(true);

            }

            time += Time.deltaTime / 1.2f;
        }
        else
        {
            time += Time.deltaTime;

            if(time > 1.5f)
            {
                isAnim = true;
                resetAnim();
            }
        }
        
    }

    public void resetAnim()
    {
        time = 0;
    }
}
