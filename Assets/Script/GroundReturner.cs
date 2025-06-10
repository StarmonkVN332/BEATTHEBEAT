using UnityEngine;

public class GroundReturner : MonoBehaviour
{
    [SerializeField] private float lifetime = 20f;

    private void OnEnable()
    {
        // Khi object được bật lại → bắt đầu đếm ngược
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable()
    {
        // Nếu object bị tắt trước khi hết thời gian, huỷ Invoke để tránh lỗi
        CancelInvoke(nameof(ReturnToPool));
    }

    private void ReturnToPool()
    {
        GroundPool.Instance.ReturnGround(gameObject);
    }
}
