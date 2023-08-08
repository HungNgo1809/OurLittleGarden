using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoverChatText : MonoBehaviour
{
    public RectTransform mess;

    public float border = 5f;
    private void Update()
    {
        // Lấy kích thước của mess
        Vector2 messSize = mess.sizeDelta;

        // Tăng kích thước width và height của messSize thêm 5 đơn vị
        messSize += new Vector2(border, border);

        // Thiết lập kích thước width và height cho myRectTransform
        this.GetComponent<RectTransform>().sizeDelta = messSize;
    }
}
