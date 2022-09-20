using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour
{

    Player player;

    GameManager myGameManager;

    List<CoinData> coins = new List<CoinData>();
    List<MovableObjectData> movableObj = new List<MovableObjectData>();
    Coin[] availableCoins;
    MovableObject[] movableObject;

    //Handle Save
    [SerializeField] private TMP_InputField saveName;
    List<string> saveFiles = new List<string>();
    string saveDir;

    [SerializeField] Canvas LoadScreen;
    [SerializeField] Transform LoadGridContent;
    [SerializeField] Canvas SaveScreen;
    [SerializeField] Transform SaveGridContent;
    [SerializeField] GameObject button;
    [SerializeField] GameObject deleteButton;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        myGameManager = GetComponent<GameManager>();
        
        saveDir = Application.dataPath + "/Saves";
    }

    private void UpdateCoinData()
    {
        availableCoins = FindObjectsOfType<Coin>();
        coins.Clear();
        foreach (var obj in availableCoins)
        {
            CoinData data = new CoinData(obj.Coin_ID, obj.IsDestroyed);
            coins.Add(data);
        }
    }

    private void UpdateMovableObjData()
    {
        movableObject = FindObjectsOfType<MovableObject>();
        movableObj.Clear();
        foreach (var obj in movableObject)
        {
            float[] obj_position = {obj.transform.position.x, obj.transform.position.y, obj.transform.position.z};
            MovableObjectData data = new MovableObjectData(obj.Obj_ID, obj_position);
            movableObj.Add(data);
        }
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveData(string dataPath)
    {
        UpdateCoinData();
        UpdateMovableObjData();
        SaveSystem.SaveData(dataPath, player, coins, movableObj);
    }

    public void saveNewFile()
    {
        //check for similar naming
        GetSaveFiles();
        foreach(var file in saveFiles)
        {
            if(file.Contains(saveName.text))
            {
                Debug.Log("File Exist");
                saveName.text = "";
                return;
            }
        }

        string newSaveFile = saveDir + "/" + saveName.text;

        SaveData(newSaveFile);

        SaveScreen.gameObject.SetActive(!SaveScreen.isActiveAndEnabled);
        if(!SaveScreen.isActiveAndEnabled) player.isMovable = true;

        saveName.text = "";
    }

    public void LoadData(string dataPath)
    {
        availableCoins = FindObjectsOfType<Coin>();
        coins.Clear();

        movableObject = FindObjectsOfType<MovableObject>();
        movableObj.Clear();
        
        SaveData data = SaveSystem.LoadData(dataPath);
        data.LoadData(player,ref coins, ref movableObj);

        List<string> coin_id = new List<string>();

        foreach(var obj in coins)
        {
            coin_id.Add(obj.coin_id);
        }
        
        foreach(var obj in availableCoins)
        {
            if(!coin_id.Contains(obj.Coin_ID)) obj.IsDestroyed = true;
        }

        foreach(var data_obj in movableObj)
        {
            foreach(var obj in movableObject)
            {
                if(obj.Obj_ID == data_obj.obj_id)
                {
                    Vector3 position = new Vector3(data_obj.obj_position[0],data_obj.obj_position[1],data_obj.obj_position[2]);
                    obj.transform.position = position;
                }
            }
        }
    }

    public void GetSaveFiles()
    {
        saveFiles.Clear();

        if(!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        string[] saves = Directory.GetFiles(saveDir);

        foreach(var file in saves)
        {
            if(!file.Contains("meta"))
            {
                saveFiles.Add(file);
            }
        }
    }

    public void saveButton()
    {
        displaySaveFiles(SaveScreen, "save");
    }

    public void LoadButton()
    {
        displaySaveFiles(LoadScreen, "load");
    }

    public void displaySaveFiles(Canvas screen, string command)
    {
        screen.gameObject.SetActive(!screen.isActiveAndEnabled);
        if(screen.isActiveAndEnabled) player.isMovable = false;
        else player.isMovable = true;

        GetSaveFiles();

        Transform filesPanel  = SaveGridContent;
        if(command == " save") filesPanel = SaveGridContent;
        else if (command == "load") filesPanel = LoadGridContent;

        foreach(Transform obj in filesPanel)
        {
            if(obj.GetComponent<Button>())
            {
                Destroy(obj.gameObject);
            }
        }

        if(screen.isActiveAndEnabled){
            foreach(var data in saveFiles)
            {
                GameObject buttonObj = Instantiate(button);
                buttonObj.transform.SetParent(filesPanel, false);

                buttonObj.GetComponent<Button>().onClick.AddListener(
                    () =>{
                            if(command == "load")
                            {
                                LoadData(data);
                            }
                            else if (command == "save")
                            {
                                SaveData(data);
                            }
                            
                            screen.gameObject.SetActive(!screen.isActiveAndEnabled);
                            if(screen.isActiveAndEnabled) player.isMovable = false;
                            else player.isMovable = true;
                    });

                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = data.Replace(saveDir, "");

                GameObject childButton = Instantiate(deleteButton);
                childButton.transform.SetParent(buttonObj.transform, false);
                childButton.GetComponent<Button>().onClick.AddListener(
                    () => { 
                        SaveSystem.deleteData(data); 
                        screen.gameObject.SetActive(!screen.isActiveAndEnabled);
                        displaySaveFiles(screen,command);
                    });
            }
        }
    }

    public void deleteFile()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        Debug.Log(button.GetComponentInChildren<TextMeshProUGUI>().text);
    }
}
