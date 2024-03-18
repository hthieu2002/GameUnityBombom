using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroy : MonoBehaviour
{
    #region Biến
    public float timeDestroy = 1f;
    #endregion

    #region Hàm
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }
    #endregion
}
