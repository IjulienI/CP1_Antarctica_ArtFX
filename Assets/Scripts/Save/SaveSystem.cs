using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public GameInfo gameInfo;
    public GameObject cheatScreen;

    private void Start()
    {
        instance = this;
        Load();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/data.save"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/data.save");
            gameInfo = JsonUtility.FromJson<GameInfo>(json);

            if (File.GetLastWriteTime(Application.persistentDataPath + "/data.save").ToString(CultureInfo.CurrentCulture) == gameInfo.modificationDate)
            {
                LoadLevel();
                LoadPlayer();
                LoadDoors();
            }
            else
            {
                LoadLevel();
                LoadPlayer();
                LoadDoors();
                //File.Delete(Application.persistentDataPath + "/data.save");
                //if (cheatScreen != null) cheatScreen.SetActive(true);
                //else Debug.Log("Don't Cheat !");
            }
        }
    }
    public void Save()
    {
        SavePlayer();
        SaveDoors();
        SaveLevel();

        gameInfo.modificationDate = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

        string json = JsonUtility.ToJson(gameInfo);
        if (!File.Exists(Application.persistentDataPath + "/data.save"))
        {
            File.Create(Application.persistentDataPath + "/data.save").Dispose();
        }
        File.WriteAllText(Application.persistentDataPath + "/data.save", json);
    }

    //PLAYER :
    //---------------------------------------------------------------------------------------
    private void SavePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            gameInfo.player.pos = new Vector2(player.transform.position.x + 2, player.transform.position.y);
        }
    }

    private void LoadPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = gameInfo.player.pos;
        }
    }

    //LEVER DOORS :
    //---------------------------------------------------------------------------------------
    public void SaveDoors()
    {
        LeverDoor[] doors = FindObjectsOfType<LeverDoor>();

        gameInfo.doors.Clear();
        for(int i = 0; i < doors.Length; i++)
        {
            gameInfo.doors.Add(new Doors()
            {
                index = doors[i].index,
                isOpenned = doors[i].doorIsOpen
            });
        }
    }

    public void LoadDoors()
    {
        LeverDoor[] doors = FindObjectsOfType<LeverDoor>();
        int index = 0;
        while(index != doors.Length)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].index == gameInfo.doors[i].index)
                {
                    doors[i].doorIsOpen = gameInfo.doors[i].isOpenned;
                    index++;
                }
            }
        }
        Debug.Log(index);
    }

    //LEVEL GESTION :
    //---------------------------------------------------------------------------------------
    public void SaveLevel()
    {
        gameInfo.levelName = SceneManager.GetActiveScene().name;
    }

    public void LoadLevel()
    {
        if(gameInfo.levelName != null)
        {
            if (SceneManager.GetActiveScene().name == gameInfo.levelName)
            {
                SceneManager.LoadScene(gameInfo.levelName);
            }
            else SceneManager.LoadScene("Level1");
        }
    }
}
