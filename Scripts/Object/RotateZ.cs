using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZ : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        // Lấy góc quay hiện tại của vật
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Cập nhật góc quay theo chiều Z
        float newRotationZ = currentRotation.z + rotationSpeed * Time.deltaTime;

        // Tạo một Vector3 mới với góc quay cập nhật
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y, newRotationZ);

        // Đặt góc quay mới cho vật
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
