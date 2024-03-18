using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public void ShowGameOverPanel()
    {
        // Hiển thị cửa sổ thông báo game over
        gameObject.SetActive(true);
    }
}
