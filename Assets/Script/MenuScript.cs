
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject pausePanel; // Tham chiếu đến panel pause
    public GameObject GameOver;
    public Text timerText;
    private float timeRemaining = 180f; // Thời gian còn lại là 2 phút
  
    private void Start()
    {
        timerText = GameObject.Find("Time").GetComponent<Text>();
        if (timerText == null)
        {
            Debug.LogError("Không tìm thấy đối tượng Text!");
        }
    }

    public void OnPauseButtonClicked()
    {
        // Tạm dừng trò chơi
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

  
    void Update()
    {
        if (timeRemaining > 0)
        {
            // Giảm thời gian còn lại
            timeRemaining -= Time.deltaTime;

            // Cập nhật hiển thị đồng hồ đếm ngược
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("{0:00}:{01:00}", minutes, seconds);
            if (timeRemaining <= 30f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.green;
            }
        }
        else
        {
            // Thực hiện các hành động khi đếm ngược kết thúc (ví dụ: kết thúc trò chơi)
            Time.timeScale = 0;
            GameOver.SetActive(true);
            Debug.Log("game over Finished!");
        }
    }
   public void continute()
    {
        Time.timeScale = 1;

        // Hiển thị bảng Game Over
        pausePanel.SetActive(false);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
        // Hiển thị bảng Game Over
        pausePanel.SetActive(false);
    }

}
