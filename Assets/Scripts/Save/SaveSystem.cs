using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public GameInfo gameInfo;

    private void Start()
    {
        instance = this;
        Invoke(nameof(Load), 0.1f);
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/data.save"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/data.save");
            gameInfo = JsonUtility.FromJson<GameInfo>(json);

            LoadLevel();
            if(SceneManager.GetActiveScene().name == gameInfo.levelName)
            {
                LoadPlayer();
                LoadAi();
                LoadDoors();
            }
            if (File.GetLastWriteTime(Application.persistentDataPath + "/data.save").ToString(CultureInfo.CurrentCulture) != gameInfo.modificationDate)
            {
                //File.Delete(Application.persistentDataPath + "/data.save");
                //if (cheatScreen != null) cheatScreen.SetActive(true);
                //else Debug.Log("Don't Cheat !");
            }
        }
        else if (SceneManager.GetActiveScene().name != "Level 1")
        {
            SceneManager.LoadScene("IntroScene");
        }
    }
    public void Save()
    {
        SavePlayer();
        SaveAi();
        SaveDoors();
        SaveLevel();

        gameInfo.modificationDate = DateTime.ParseExact(DateTime.Now.ToString("U"), "U", CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);

        string json = JsonUtility.ToJson(gameInfo);
        if (!File.Exists(Application.persistentDataPath + "/data.save"))
        {
            File.Create(Application.persistentDataPath + "/data.save").Dispose();
        }
        File.WriteAllText(Application.persistentDataPath + "/data.save", json);
        Debug.Log(Application.persistentDataPath + "/data.save");
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

    //AI :
    //---------------------------------------------------------------------------------------
    private void SaveAi()
    {
        GameObject ai = GameObject.FindGameObjectWithTag("Alien");

        if (ai != null)
        {
            gameInfo.ai.pos = new Vector2(ai.transform.position.x + 2, ai.transform.position.y);
        }
    }

    private void LoadAi()
    {
        GameObject ai = GameObject.FindGameObjectWithTag("Alien");

        if (ai != null)
        {
            ai.transform.position = gameInfo.ai.pos;
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
            if(SceneManager.GetActiveScene().name == "SaveSystem")
            {
                SceneManager.LoadScene(gameInfo.levelName);
            }
            if (SceneManager.GetActiveScene().name != gameInfo.levelName)
            {
                gameInfo.levelName = SceneManager.GetActiveScene().name;
            }
        }
    }
}