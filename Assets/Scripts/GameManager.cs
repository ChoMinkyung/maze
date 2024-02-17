using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public PlayerController player;

    public GameObject chestsParent;
    public ChestOperation[] chests;
    public int chestNumber;

    public TMP_Text chestText;

    public Image[] heartImages;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    public ChangeScene changeScene;

    public GameObject pause;

    public Camera mainCamera;

    private void Start()
    {
        pause.SetActive(false);
        UpdateHP();

        mazeGenerator.OnChestGeneration += SetChest;
        player.OnPlayerMonsterCollision += PlayerMonsterCollision;
        player.OnPlayerChestCollision += PlayerChestCollision;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnTogglePauseButton();
        }
    }

    void SetChest()
    {
        chests = chestsParent.GetComponentsInChildren<ChestOperation>();
        chestNumber = chests.Length;

        UpdateChest();
    }

    void PlayerMonsterCollision()
    {
        player.ResetPosition();
        player.HP = player.HP - 1;
        UpdateHP();
    }

    void PlayerChestCollision()
    {
        StartCoroutine(ChangeCamera());
    }

    void UpdateHP()
    {
        for(int i=0; i<3; i++)
        {
            if(i < player.HP)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }

        if (player.HP == 0)
        {
            changeScene.SceneChange("99_LooseScene");
        }

    }

    public void OnTogglePauseButton()
    {
        if (Time.timeScale == 0)
        {
            pause.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pause.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void UpdateChest()
    {
        chestText.text = chestNumber.ToString() + " X ";
    }

    IEnumerator ChangeCamera()
    {
        float time = 0;
        player.gameObject.SetActive(false);
        while (time < 5)
        {
            time += Time.deltaTime;
            yield return null;
        }

        player.gameObject.SetActive(true);

        chestNumber--;
        UpdateChest();

        player.ChestAnim = false;
    }


}
