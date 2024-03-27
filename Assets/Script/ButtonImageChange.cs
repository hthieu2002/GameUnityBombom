using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChange : MonoBehaviour
{
    public Sprite image;

    public void FlipImage(Button button)
    {
        button.GetComponent<Image>().sprite = image;
    }
}
