using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private int move = -5;
    private float timer = 0f;  // Thời gian từ lúc spawn

    void Start()
    {
        // Đảm bảo timer bắt đầu từ 0 khi spawn
        timer = 0f;
    }

    void Update()
    {
        //// Cập nhật timer mỗi frame
        //timer += Time.deltaTime;

        //// In thời gian mỗi frame (tuỳ chọn, có thể bỏ nếu không cần)
        //Debug.Log("Thời gian từ lúc spawn: " + timer.ToString("F2") + " giây");

        // Di chuyển đối tượng
        transform.position += new Vector3(0, 0, move) * Time.deltaTime;
    }
}
