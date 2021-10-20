using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Play,
    Win,
    Lose
}
public class GameManager : MonoBehaviour
{
    public LevelManager levels;
    public GameState gameState;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.Play:
                
                break;
            case GameState.Win:
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name,1);
                PlayerPrefs.Save();
                LoadNewLevel();
                break;
            case GameState.Lose:
                ResetLevel();
                break;
            default:
                break;
        }
    }

    void LoadNewLevel()
    {
        gameState = GameState.Play;

        for (int i = 0; i < levels.levelSet.Count; i++)
        {
            if (!PlayerPrefs.HasKey(levels.levelSet[i].levelName))
            {
                Debug.Log("Create Key - " + levels.levelSet[i].levelName);
                PlayerPrefs.SetInt(levels.levelSet[i].levelName , 0);
                PlayerPrefs.Save();
                
            }
            if(PlayerPrefs.GetInt(levels.levelSet[i].levelName) < 1)
            {               
                 SceneManager.LoadScene(levels.levelSet[i].levelName);
                 return;
                
            }
        }

        var randomLevelIndex = Random.Range(0, levels.levelSet.Count);
        Debug.Log("random level");
        SceneManager.LoadScene(levels.levelSet[randomLevelIndex].levelName);
    }

    void ResetLevel()
    {
        gameState = GameState.Play;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DeleteSavedDatas()
    {
        PlayerPrefs.DeleteAll();
    }
}
