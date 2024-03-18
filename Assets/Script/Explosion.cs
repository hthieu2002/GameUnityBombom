using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Biến
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;
    #endregion

    #region Chức năng
    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    // Chức năng xoay hướng cho hình ảnh vụ nổ
    public void SetDirection(Vector2 direction)
    {
        float angle =  Mathf.Atan2(direction.y, direction.x); // Tính góc xoay

        // quay hình theo trục Z
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward); 

    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
    #endregion
}
