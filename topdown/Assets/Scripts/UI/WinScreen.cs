using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killsText;
    public Transform allHolder;

    private FloorGlobal floorGlobal;

    void Start()
    {
        floorGlobal = FloorGlobal.Instance;
        timeText.text = floorGlobal.playTimer.timeText.text;
        killsText.text = "Kills: " + floorGlobal.kills.ToString();
        foreach(Transform sprite in floorGlobal.playerController.itemInventoryHolder)
        {
            GameObject newSprite = Instantiate(sprite.gameObject);
            newSprite.transform.SetParent(allHolder);
        }
        foreach (Transform sprite in floorGlobal.playerController.gunInventoryHolder)
        {
            GameObject newSprite = Instantiate(sprite.gameObject);
            newSprite.transform.SetParent(allHolder);
        }
    }

    public void BackHome()
    {
        floorGlobal.DestroyAllDontDestroyOnLoad();
        SceneManager.LoadScene(0);
    }

}
