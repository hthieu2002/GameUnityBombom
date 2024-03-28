using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementEnemy : MonoBehaviour
{
    #region Biến   
    private int scorePerEnemyBalloon = 10;
    private float speed = 1.0f;

    private Vector2 moveNow;
    private Vector2 translation;

    private int count = 0;
    private Vector2 reduce = new Vector2(0.06f, 0.06f);
    
    public Rigidbody2D rigidbodyEnemy { get; private set; }
    public Vector2 direction = Vector2.up;

    [Header("Things to avoid")]
    public LayerMask[] denyLayer;   

    [Range(0f, 1f)]
    public float moveChance = 0.2f;

    [Header("Status Enemy")]
    public AnimatedSpriteRenderer spriteRenderEnemyDeath;
    public AnimatedSpriteRenderer spriteRenderEnemyLive;

    [Header("Item Destroy")]
    public AnimatedSpriteRenderer itemDestroyPrefab;
    #endregion

    private void Awake()
    {
        rigidbodyEnemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Đổi hướng ngẫu nhiên
        if (count==50 && Random.value <= moveChance)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    direction = Vector2.right;
                    break;
                case 1:
                    direction = Vector2.up;
                    break;
                case 2:
                    direction = Vector2.down;
                    break;
                case 3:
                    direction = Vector2.left;
                    break;
            }
        }
        moveChange();
    }

    // Bật tường sử dụng layerMark
    private bool checkBouncingWall(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        // Đếm số lần chạm để kiểm tra xem 2 hướng đối nhau đó có bị chặn không
        int soCham = 0;
        while (Physics2D.OverlapCircle(moveNow, 0.48f, denyLayer[0]) != null ||Physics2D.OverlapCircle(moveNow, 0.48f, denyLayer[1]) != null)
        {
            count = 1;
            soCham+=1;
            if (soCham == 2)
                return false;
            else
            {
                if (direction == Vector2.up)
                {
                    direction = Vector2.down;
                }
                else if (direction == Vector2.down)
                {
                    direction = Vector2.up;
                }
                else if (direction == Vector2.left)
                {
                    direction = Vector2.right;
                }
                else
                {
                    direction = Vector2.left;
                }
                translation = direction * speed * 0.02f;
                moveNow = position + translation;
            }
        }
        return true;
    }

    // Đổi hướng khi bị chặn nhiều đường
    private void changeDirection(Vector2 position)
    {
        if(direction.y != 0)
        {
            direction.y = 0;
            direction.x = 1;
        }
        else if(direction.x != 0)
        {
            direction.x = 0;
            direction.y = 1;
        }
        translation = direction * speed * 0.02f;
        moveNow = position + translation;
        checkBouncingWall(position);
    }

    // Kiểm tra đụng tường
    private void checkWallCollider(Vector2 position)
    {
        if(checkBouncingWall(position) == false)
        {
            changeDirection(position);
        }        
    }

    // Thực hiện di chuyển
    private void moveChange()
    {
        if (count == 50)
            count = 1;
        else
            count++;
        Vector2 position = rigidbodyEnemy.position;
        translation = direction * speed * 0.02f;
        moveNow = position + translation;
        checkWallCollider(position);
        rigidbodyEnemy.MovePosition(moveNow);
    }

    // Đụng trúng explosion
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Đụng trúng Explosion
        if(other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequenceEnemy();
        }

        // Đụng trúng item
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Instantiate(itemDestroyPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject, 0.7f);
        }
    }

    private void DeathSequenceEnemy()
    {
        enabled = false;
        spriteRenderEnemyLive.enabled = false;
        spriteRenderEnemyDeath.enabled = true;
        Destroy(gameObject,1.25f);

        GameManager.instance.AddScore(scorePerEnemyBalloon);
    }
}
