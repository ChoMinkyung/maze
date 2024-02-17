using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOperation : MonoBehaviour
{
    bool _hasDiamond = false;
    public GameObject diamond;
    public Animator animator;
    public ChangeScene changeScene;


    public void SetDiamond(bool hasDiamond)
    {
        _hasDiamond = hasDiamond;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetBool("Open", true);
            collision.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        if (_hasDiamond)
        {
            diamond.SetActive(true);
            StartCoroutine(GrowDiamond());
        }
        else
        {
            StartCoroutine(ChestDestroy());
        }

        IEnumerator GrowDiamond()
        {
            // 원하는 크기로 점점 커지도록 보간
            Vector3 targetScale = new Vector3(2f, 2f, 2f); // 원하는 크기 설정
            float duration = 3f; // 커지는 데 걸리는 시간 설정
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                diamond.transform.localScale = Vector3.Lerp(Vector3.one, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 최종 크기 설정
            diamond.transform.localScale = targetScale;
            changeScene.SceneChange("99_WinScene");
        }

    }

    IEnumerator ChestDestroy()
    {
        float time = 0;

        while (time < 5)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }

}
