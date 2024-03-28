using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    #region Biến
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
    }

    public ItemType Type;
    #endregion

    #region Hàm
    public void UpdateItemCount(GameObject player)
    {
        switch (Type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<MovementController>().IncrementBombCount();
                break;

            case ItemType.BlastRadius:
                player.GetComponent<MovementController>().IncrementBlastItemCount();
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().IncrementSpeedItemCount();
                break;
        }
    }
    private void OnItemPickup(GameObject player)
    {
        switch(Type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombCotroller>().AddBomb();
                break;

            case ItemType.BlastRadius:
                player.GetComponent<BombCotroller>().explosionRadius++;
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed++;
                break;
        }
        UpdateItemCount(player);

        Destroy(gameObject); // Xóa vật phẩm khi đã nhặt
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject); // Lấy object của người chơi
        }
    }
    #endregion
}
