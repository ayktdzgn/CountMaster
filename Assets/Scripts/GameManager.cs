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
    string level;
    [SerializeField] Horde horde;
    [SerializeField] FinishPoint finishPoint;
    public GameState gameState;

    private void Awake()
    {

        level = SceneManager.GetActiveScene().name;
        Debug.Log(level);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.Play:
                finishPoint.ValueChangeOnUI(horde.transform.position);
                break;
            case GameState.Win:
                PlayerPrefs.SetInt(level.ToString() , 1);
                PlayerPrefs.Save();

                for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    if (!PlayerPrefs.HasKey(i.ToString()))
                    {
                        PlayerPrefs.SetInt(i.ToString(), 0);
                        PlayerPrefs.Save();
                    }

                    if (PlayerPrefs.GetInt(i.ToString()) < 1)
                    {
                        Debug.Log("level : " + i.ToString());
                        SceneManager.LoadScene(i.ToString());
                    }
                }
             
                SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCount));
                break;
            case GameState.Lose:
                SceneManager.LoadScene(level);
                break;
            default:
                break;
        }
    }
}
