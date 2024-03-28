using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Biến
    public GameObject players;
    private int score = 0; // Điểm hiện tại
    public static GameManager instance; // Singleton instance
    private int totalEnemies = 7; // Tổng số quái vật
    private int defeatedEnemies = 0; // Số quái vật đã bị tiêu diệt
    //private bool gameEnded = false; // Trạng thái kết thúc trò chơi

    public  int bombItemCount = 1;
    public int speedItemCount = 1;
    public int blastItemCount = 1;
    public int livingCharacters = 2; // Số lượng nhân vật còn sống
    #endregion

    #region Hàm
    private void Update()
    {
        Data.scoreValue = GetScore();
    }
    public void CheckWinStage(GameOverPanel gameOverPanel)
    {
        livingCharacters--;
        int aliveCount = 0;
        // Kiểm tra xem người chơi còn active hay không.
        if (players.activeSelf)
        {
            aliveCount++;
        }

        if(aliveCount < 1)
        {
            gameOverPanel.ShowGameOverPanel();
            //Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        // Tải lại cảnh xung quanh như lúc ban đầu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Phương thức để thêm điểm
    public void AddScore(int amount)
    {
        score += amount;
    }

    // Phương thức để hiển thị điểm
    public virtual int DisplayScore()
    {
        Debug.Log("Score: " + score);
        return score;
    }
    public int GetScore()
    {
        return score;
    }
    public void EnemyDefeated()
    {
        defeatedEnemies++;
        if (defeatedEnemies >= totalEnemies)
        {
            Finish();
        }
    }

    // Hàm kết thúc trò chơi
    private void Finish()
    {
        // Thêm logic để kết thúc trò chơi ở đây, ví dụ:
       // gameEnded = true;
        Debug.Log("Game Over!");
        // Gọi các hàm hoặc xử lý logic khi kết thúc trò chơi
    }
    // Phương thức để tăng điểm
    public void IncreaseScore(int amount)
    {
        score += amount;
    }

   

    public void CharacterDied()
    {
        livingCharacters--; // Giảm số lượng nhân vật còn sống khi một nhân vật chết
        if (livingCharacters <= 0)
        {
            //
        }
    }
    #endregion
}
