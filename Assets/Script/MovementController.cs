using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemPickup;

public class MovementController : MonoBehaviour
{
    #region Biến
    public Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;

    // Nhận biết phím bấm
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;
    public GameOverPanel gameOverPanel;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private int speedItemCount = 1;
    private int bombCount = 1; // Số lượng bom
    private int blastItemCount = 1; // Số lượng item tăng bán kính nổ

    #endregion

    #region Chức năng
    // Hàm này lấy toàn bộ thuộc tính của người chơi ngay khi chương trình chạy
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    // Awake và Update là 2 trong nhiều hàm có trong MonoBehaviour
    // Nên khi gọi ta cần phải gọi chính xác nó
    // Update sẽ được gọi mọi lúc trong game nó thường để kiểm tra thông tin đầu vào (ví dụ nhấn phím)
    // Hàm Update chạy theo tốc độ khung hình(FPS).
    // Nếu cho nhiều đối tượng không cần thiết, ít thay đổi trạng thái sẽ làm tốn tài nguyên
    // Vì thế chỉ nên đưa các vật thể có tính thường xuyên thay đổi vị trí, trạng thái vào trong hàm này
    private void Update()
    {
        // Nhận phím nhấn giữ sử dụng GetKey (vd: Nhấn giữ phím W để nhân vật đi lên liên tục)
        // Chỉ nhận phím nhấp nhấp thì sử dụng GetKeyDown, GetKeyUp,... (vd: Nhấn 4 phím S để nhân vật đi xuống 4. Nghĩa là muốn đi lên bạn phải nhấp nhả phím S đến chết)
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft); 
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        // Không di chuyển
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }
    public void IncrementBombCount()
    {
        bombCount++;
        UpdateItemCountText();
    }

    // Phương thức để tăng số lượng item tăng bán kính nổ
    public void IncrementBlastItemCount()
    {
        blastItemCount++;
        UpdateItemCountText();
    }

    // Phương thức để tăng số lượng item tăng tốc độ
    public void IncrementSpeedItemCount()
    {
        speedItemCount++;
        UpdateItemCountText();
    }

    // Phương thức cập nhật số lượng item trên Text
    private void UpdateItemCountText()
    {
        // Lấy đối tượng ScoreDisplay trong cùng GameObject
        ScoreDisplay scoreDisplay = GetComponent<ScoreDisplay>();

        // Kiểm tra null tránh lỗi
        if (scoreDisplay != null)
        {
            // Cập nhật số lượng item trên Text
            scoreDisplay.UpdateItemCountText(bombCount, blastItemCount, speedItemCount);
        }
    }

    // Thiết lập hướng quay cho nhân vật
    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }
    
    // Hàm FixedUpdate là hàm có trong MonoBehaviour
    // Nó sẽ được gọi mỗi khi bản chất physics thay đổi và chỉ chạy trong thời gian nhất định
    // Thường được sử dụng đối với các vật thể có tính physics(vật cản, người chơi,...)
    // Nó giúp ta tiết kiệm, kiểm soát thời gian mà vật thể thay đổi tính Physics một cách nhất quán
    // Nghĩa là thời gian thực thi sẽ không phụ thuộc vào FPS giúp ta kiểm soát lượng tài nguyên hệ thống tốt hơn.
    private void FixedUpdate()
    {
        // Lấy vị trí người chơi
        Vector2 position = rigidbody.position;
        // Set sự di chuyển cho player
        //Time.fixedDeltaTime = 0.02f
        Vector2 translation = direction * speed * Time.fixedDeltaTime; 

        // Di chuyển vị trí player
        rigidbody.MovePosition(position + translation);
    }

    // Kiểm tra người chơi có chạm phải vụ nổ hay enemy không
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Explosion") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            DeathSequence();
        }
        // Đụng trúng item
       
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombCotroller>().enabled = false;

        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererUp.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinStage(gameOverPanel);
    }
   
    #endregion

}
