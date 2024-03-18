using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Biến
    public GameObject players;
    #endregion

    #region Hàm
    public void CheckWinStage(GameOverPanel gameOverPanel)
    {
        int aliveCount = 0;
        // Kiểm tra xem người chơi còn active hay không.
        if (players.activeSelf)
        {
            aliveCount++;
        }

        if(aliveCount < 1)
        {
            gameOverPanel.ShowGameOverPanel();
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        // Tải lại cảnh xung quanh như lúc ban đầu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
