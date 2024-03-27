using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGame : MonoBehaviour
{
    
    public GameObject play;
    public GameObject menuPlay;
    public int playGame = 1;
   
    public void Play()
    {
        play.SetActive(false);
        menuPlay.SetActive(true);
        //Time.timeScale = 0;
        // gameObject.SetActive(true);
        //SceneManager.LoadScene(1);
    }
    public void exit()
    {
        Application.Quit();
    }
    public void Play1()
    {
        playGame = 1;
        Data.playGameValue = playGame;
        play.SetActive(false);
        menuPlay.SetActive(false);
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.livingCharacters--;
        }
        //Time.timeScale = 0;
        // gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void Play2()
    {
        playGame = 2;
        Data.playGameValue = playGame;
        play.SetActive(false);
        menuPlay.SetActive(false);
        //Time.timeScale = 0;
        // gameObject.SetActive(true);
        SceneManager.LoadScene(3);
    }
    public void Online()
    {
        ///
        //SceneManager.LoadScene(1);
    }
    public void Rank()
    {
        //
        SceneManager.LoadScene(4);
    }
   
}
