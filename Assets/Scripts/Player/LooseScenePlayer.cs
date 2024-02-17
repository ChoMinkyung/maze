using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseScenePlayer : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator.SetBool("isDead", true);
        animator.speed = 0.5f;
    }
}
