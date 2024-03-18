using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    #region Biến
    public float destructibleTime = 1f;

    // Range giúp cho ta có thể kéo thanh trượt để chỉnh các biến liên quan đến số trong Unity
    [Range(0f, 1f)] 
    public float itemSpawnChance = 0.2f;
    public GameObject[] spawnableItems;
    #endregion

    #region Hàm
    private void Start()
    {
        Destroy(gameObject, destructibleTime);
    }

    // Làm gì sau khi đã Destroy gạch
    private void OnDestroy()
    {
        if(spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItems.Length);
            Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
    #endregion
}
