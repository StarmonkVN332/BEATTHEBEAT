using TMPro;
using UnityEngine;
using Firebase.Auth;

public class UpdateUserUI : MonoBehaviour
{
    public TMP_InputField coinsInput;
    public TMP_InputField pointInput;

    [Header("🔥 Gán Text mới từ scene menu")]
    public TMP_Text nameText;
    public TMP_Text coinsText;
    public TMP_Text pointText;

    private FirebaseManager firebaseManager;
    private string userId;

    private void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

        if (firebaseManager == null || userId == null)
        {
            Debug.LogError("❌ Không tìm thấy FirebaseManager hoặc User chưa đăng nhập.");
            return;
        }

        // GÁN lại UI Text cho FirebaseManager
        firebaseManager.SetUITextReferences(nameText, coinsText, pointText);
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
