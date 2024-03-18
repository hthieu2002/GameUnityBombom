using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    // Tạo hoạt ảnh cho các Sprite

    #region Biến
    private SpriteRenderer _spriteRenderer;

    public Sprite idleSprite; // idle ở đây có nghĩa là chờ đợi
    public Sprite[] animationSprites;

    public float animationTime = 0.25f;
    private int animationFrame;

    public bool loop = true;
    public bool idle = true;

    #endregion

    #region Hàm chức năng
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    } 

    // Bật SpriteRenderer để kết xuất Sprite Animation
    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }

    // Tắt SpriteRenderer khi không cần dùng
    private void OnDisable()
    {
        _spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    private void NextFrame()
    {
        // Tăng khung hình trước khi chuyển sang khung tiếp theo
        animationFrame++;

        // Kiểm tra xem ảnh có cần lặp lại từ đầu chưa
        if(loop &&  animationFrame >= animationSprites.Length) 
        {
            animationFrame = 0;
        }

        if(idle)
        {
            _spriteRenderer.sprite = idleSprite;
        }
        else if(animationSprites.Length > animationFrame && animationFrame >= 0)
        {
            _spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }
    #endregion
}
