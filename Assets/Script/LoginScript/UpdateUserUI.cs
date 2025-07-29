using TMPro;
using UnityEngine;
using Firebase.Auth;

public class UpdateUserUI : MonoBehaviour
{
    public TMP_InputField coinsInput;
    public TMP_InputField pointInput;
    private FirebaseManager firebaseManager;
    private string userId;

    private void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

        if (firebaseManager == null || userId == null)
        {
            Debug.LogError("❌ Không tìm thấy FirebaseManager hoặc User chưa đăng nhập.");
        }
    }

    public void OnClickUpdate()
    {
        if (int.TryParse(coinsInput.text, out int newCoins) &&
            int.TryParse(pointInput.text, out int newPoint))
        {
            firebaseManager.UpdateUserData(userId, newCoins, newPoint);
        }
        else
        {
            Debug.LogWarning("⚠ Vui lòng nhập số hợp lệ!");
        }
    }
}
