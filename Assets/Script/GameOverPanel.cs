using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    GameManager game = new GameManager();
    public void ShowGameOverPanel()
    {
        game.CharacterDied();

        if (game.livingCharacters <= 0)
        {
            // Hiển thị cửa sổ thông báo game over
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
    } 
}
