using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombCotroller : MonoBehaviour
{
    #region Biến
    private Collider2D colliderBox;
    [Header("Bomb")]
    public GameObject bombPrefabs;
    public KeyCode inpputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombRemaining = 0;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask[] explosionLayerMark;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTile;
    public Destructible destructiblePrefab;

    [Header("Item Destroy")]
    public AnimatedSpriteRenderer itemDestroyPrefab;
    #endregion

    #region Hàm chức năng
    private void OnEnable()
    {
        bombRemaining = bombAmount;
    }

    private void Update()
    {
        if(Input.GetKeyDown(inpputKey))
        {
            if(bombRemaining > 0 && Input.GetKeyDown(inpputKey))
            {
                // Chức năng của Coroutine là tạm dừng thực thi 1 hàm nào đó và trả quyền điều khiển cho Unity trong 1 khoảng thời gian nhất định.
                // Nhưng nó sẽ được thực thi lại ở khung hình tiếp theo

                StartCoroutine(PlaceBomb()); // Bắt đầu thực thi 1 Coroutine
            }
        }
    }

    // IEnumerator là 1 Coroutine.
    
    // Coroutine được sử dụng để xử lý các tác vụ chạy nền như tải tài nguyên, xử lý âm thanh, hoặc các tác vụ chạy liên tục như di chuyển của nhân vật
    private IEnumerator PlaceBomb()
    {
        // transform.position dùng để lấy vị trí người chơi
        Vector2 position = transform.position;
        
        // Làm tròn x,y để đặt bom đúng ô lưới của ta
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefabs, position, Quaternion.identity);
        bombRemaining--;

        // Lấy vị trí của quả bomb phòng khi nó bị đẩy dẫn đến thay đổi vị trí
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Tạm dừng hàm này trong thời gian được set ở bombFuseTime
        yield return new WaitForSeconds(bombFuseTime);

        // Khởi tạo vụ nổ
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        // Loại bỏ object
        Destroy(bomb);
        bombRemaining++;
    }

    // Hàm này sẽ bật tắt trigger khi bomb được đặt, tránh cho việc đi xuyên bomb
    public void OnTriggerExit2D(Collider2D other)
    {
        // tên layer được set ở mục Root in Prefab Asset trong Unity
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            // Ngăn không cho người chơi đẩy bomb khi bom đã đặt
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            // Có thể chỉnh FreezePosition trực tiếp trong Unity

            other.isTrigger = false;
        }
    }

    // Hàm nhận vị trí, hướng và độ dài của vụ nổ
    private void Explode(Vector2 position, Vector2 direction, int lenght)
    {
        if(lenght == 0)
        {
            return;
        }
        position += direction;

        // Kiểm tra xem vị trí của vụ nổ mới có va chạm với các vật thể hay không
        // Nếu có dừng không cho nổ
        colliderBox = Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMark[0]);
        if (colliderBox) // OverlapBox sẽ kiểm tra chồng chéo tại 1 số điểm
        // Nó nhận giá trị poin để lấy tâm điểm, sau đó là size để lấy kích thước (kiểu Vector2)
        // Chỉ số angle cho việc xoay theo góc bao nhiêu độ (float)
        // Và layerMark để nhận biết vật thể có nhãn nào là không được chồng lên
        {
            ClearDestructible(position);
            return;
        }

        colliderBox = Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMark[1]);
        if(colliderBox)
        {
            Instantiate(itemDestroyPrefab, position, Quaternion.identity);
            Destroy(colliderBox.gameObject, 0.7f);
            return;
        }

        // Khởi tạo vụ nổ mới
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(lenght>1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, lenght-1);
    }

    // Hàm xóa các viên gạch có thể bị phá trong game
    public void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTile.WorldToCell(position);
        TileBase tile = destructibleTile.GetTile(cell);

        if(tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTile.SetTile(cell, null);
        }

    }

    // Hàm này gọi ra để tránh trường hợp người chơi có 1 quả và đặt quả đó ra nhưng chưa nổ
    // Khi đó vừa lúc nhặt item thêm bomb, người chơi muốn đặt thêm quả bomb nữa ngay lúc đó
    // Thì thằng bombRemaining nó vẫn chưa được reset để = với bomAmount lúc đó
    // Nên nó vẫn = 0 dẫn đến việc không đặt được thêm bomb mặc dù đã nhặt đúng item thêm bomb
    public void AddBomb()
    {
        bombAmount++;
        bombRemaining++;
    }
    #endregion
}
