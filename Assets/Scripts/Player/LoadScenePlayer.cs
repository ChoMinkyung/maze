using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum state { first, second, third, fourth }

public class LoadScenePlayer : MonoBehaviour
{
    public Animator animator;
    public float rotationSpeed = 1f;
    public float moveSpeed = 1f;
    public GameObject exclamationMark;
    GameObject exclamationClone;
    float _beforeAngle = 90;
    float _afterAngle = 220;

    public GameObject startBtn;

    state curState = state.first;

    void Update()
    {
        if(curState == state.first)
        {
            PlayerWaking();
        }
        else if(curState == state.second)
        {
            StartCoroutine(PlayerWait());
        }
        else if(curState == state.third)
        {
            PlayerRotation(_beforeAngle, _afterAngle);
        }
        else if(curState == state.fourth)
        {
            ShowStart();
        }
    }

    void PlayerWaking()
    {
        animator.SetBool("isWalk", true);

        Vector3 movement = new Vector3(0f, 0f, 1) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

        if (transform.position.x > 3)
        {
            curState = state.second;

            animator.SetBool("isWalk", false);
            exclamationClone = Instantiate(exclamationMark);
            Vector3 newPos = transform.position;
            newPos.y += 2;
            exclamationClone.transform.position = newPos;
            exclamationClone.transform.parent = transform;
        }
    }

    IEnumerator PlayerWait()
    {

        yield return new WaitForSeconds(1.0f);

        Destroy(exclamationClone);
        curState = state.third;
        animator.SetBool("isWalk", true);

    }
   
    void PlayerRotation(float beforeAngle, float afterAngle)
    {

        transform.rotation =  Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, afterAngle, 0f), Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0f, afterAngle, 0f)) < 0.1f)
        {
            curState = state.fourth;
            animator.SetBool("isWalk", false);
            animator.SetBool("isJump", true);
        }
    }



    void ShowStart()
    {
        startBtn.gameObject.SetActive(true);

    }
}
