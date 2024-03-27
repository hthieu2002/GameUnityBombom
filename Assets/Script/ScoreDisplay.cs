using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText; // Tham chiếu đến đối tượng Text để hiển thị điểm
    public Text bombItemCountText; // Text hiển thị số lượng bom
    public Text blastItemCountText; // Text hiển thị số lượng item tăng bán kính nổ
    public Text speedItemCountText; // Text hiển thị số lượng item tăng tốc độ
    private GameManager gameManager; // Tham chiếu đến GameManager

    private void Start()
    {
        // Lấy tham chiếu đến GameManager
        gameManager = GameManager.instance;

        // Kiểm tra xem GameManager có tồn tại không
        if (gameManager == null)
        {
            Debug.LogError("GameManager không được tìm thấy.");
        }
    }

    private void Update()
    {
        // Hiển thị điểm số trên Text
        if (scoreText != null && gameManager != null)
        {
            scoreText.text = "Score: " + gameManager.DisplayScore().ToString();
        }
        if (scoreText != null && gameManager != null)
        {
            scoreText.text = "Score: " + gameManager.DisplayScore().ToString();
        }

        
    }
    public void UpdateItemCountText(int bombCount, int blastItemCount, int speedItemCount)
    {
        // Cập nhật số lượng bom
        if (bombItemCountText != null)
        {
            bombItemCountText.text = bombCount.ToString();
        }

        // Cập nhật số lượng item tăng bán kính nổ
        if (blastItemCountText != null)
        {
            blastItemCountText.text = blastItemCount.ToString();
        }

        // Cập nhật số lượng item tăng tốc độ
        if (speedItemCountText != null)
        {
            speedItemCountText.text = speedItemCount.ToString();
        }
    }
}
