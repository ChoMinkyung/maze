using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScenePlayer : MonoBehaviour
{
    public Animator animator;
   
    void Start()
    {
         animator.SetBool("isJump", true);
    }


}
