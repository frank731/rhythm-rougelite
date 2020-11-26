using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    public int currentGunIndex = 0;
    public List<GameObject> guns = new List<GameObject>();
    public List<Item> items = new List<Item>();
    public List<int> gunIndexes;
    public List<int> itemIndexes;
    public float health = 6;
    public float iFrames = 1f;
    public float heartContainers = 3;
    protected List<GameObject> hearts = new List<GameObject>();
    protected int heartEmptyIndex = 0;
    public GameObject currentGun;
    public UnityEvent loadPlayerData = new UnityEvent();
    //public ES3File playerData = new ES3File("playerData.es3");

    protected void OnLevelChanged()
    {
        Debug.Log("Saving");
        ES3.Save("currentHealth", health);
        ES3.Save("heartContainers", heartContainers);
        ES3.Save("heartEmptyIndex", heartEmptyIndex);
        ES3.Save("itemIndexes", itemIndexes);
        ES3.Save("gunIndexes", gunIndexes);
        ES3.Save("currentGun", currentGun);

        transform.position = new Vector3(0, 0, 0); //reset the players position to the starting room when the new level is loaded
    }

    public void LoadPlayerData()
    {
        Debug.Log("loading");
        health = ES3.Load<float>("currentHealth", 6);
        heartContainers = ES3.Load<float>("heartContainers", 3);
        heartEmptyIndex = ES3.Load("heartEmptyIndex", 0);
        gunIndexes = ES3.Load("gunIndexes", new List<int>());
        itemIndexes = ES3.Load("itemIndexes", new List<int>());
        gunIndexes = ES3.Load("gunIndexes", new List<int>());
        currentGun = ES3.Load("currentGun", guns[0]);
        loadPlayerData.Invoke();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnLevelChanged();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }
    void OnApplicationQuit()
    {
        Debug.Log("cleared");
        ES3.DeleteFile();
    }
}