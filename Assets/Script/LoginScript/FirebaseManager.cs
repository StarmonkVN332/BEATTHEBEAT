using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    [Header("🗂️ UI Texts")]
    public TMP_Text nameText, coinsText, pointText;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                LoadUserDataAndDisplay();
                Debug.Log("✅ Firebase đã sẵn sàng!");
            }
            else
            {
                Debug.LogError("❌ Firebase chưa sẵn sàng: " + task.Result);
            }
        });
    }

    public void SetUITextReferences(TMP_Text name, TMP_Text coins, TMP_Text point)
    {
        nameText = name;
        coinsText = coins;
        pointText = point;

        // Gọi lại LoadUserData để cập nhật các text mới này
        LoadUserDataAndDisplay();
    }

    private void Awake()
    {
        if (FindObjectsOfType<FirebaseManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SaveUserData(string userId, string name, int coins, int point)
    {
        User user = new User(name, coins, point);
        string json = JsonUtility.ToJson(user);

        dbReference.Child("Users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
                Debug.Log("✅ Đã lưu dữ liệu user.");
            else
                Debug.LogError("❌ Lỗi khi lưu user: " + task.Exception);
        });
    }

    public void UpdateUserData(string userId, int newCoins, int newPoint)
    {
        Dictionary<string, object> updates = new Dictionary<string, object>();
        updates["Coins"] = newCoins;
        updates["Point"] = newPoint;

        dbReference.Child("Users").Child(userId).UpdateChildrenAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
                Debug.Log("✅ Dữ liệu đã được cập nhật!");
            else
                Debug.LogError("❌ Cập nhật dữ liệu thất bại: " + task.Exception);
        });
    }

    public void LoadUserData(string userId, System.Action<string, int, int> callback)
    {
        dbReference.Child("Users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    User user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                    callback?.Invoke(user.Name, user.Coins, user.Point);
                }
                else
                {
                    Debug.LogWarning("⚠ Không tìm thấy dữ liệu user.");
                    callback?.Invoke("Unknown", 0, 0);
                }
            }
            else
            {
                Debug.LogError("❌ Lỗi khi load user: " + task.Exception);
                callback?.Invoke("Error", 0, 0);
            }
        });
    }

    public void LoadUserDataAndDisplay()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;
        dbReference.Child("Users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    User u = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());

                    if (nameText != null) nameText.text = u.Name;
                    if (coinsText != null) coinsText.text = u.Coins.ToString();
                    if (pointText != null) pointText.text = u.Point.ToString();
                }
                else
                {
                    Debug.LogWarning("⚠ User không có dữ liệu.");
                }
            }
            else
            {
                Debug.LogError("❌ Lỗi khi load user: " + task.Exception);
            }
        });
    }

    public void OnLoginSuccess()
    {
        Debug.Log("🎉 OnLoginSuccess");
        LoadUserDataAndDisplay();
    }

    /// <summary>
    /// 🏆 Lưu điểm cao nhất nếu score > điểm đã lưu
    /// </summary>
    public void SaveScoreFromGame(int score)
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null)
        {
            Debug.LogWarning("⚠ Không có user đăng nhập.");
            return;
        }

        string userId = user.UserId;
        DatabaseReference pointRef = dbReference.Child("Users").Child(userId).Child("Point");

        pointRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                int currentPoint = 0;
                if (task.Result.Exists && int.TryParse(task.Result.Value.ToString(), out currentPoint))
                {
                    if (score > currentPoint)
                    {
                        pointRef.SetValueAsync(score).ContinueWithOnMainThread(setTask =>
                        {
                            if (setTask.IsCompletedSuccessfully)
                                Debug.Log("🏆 Đã lưu điểm cao mới: " + score);
                            else
                                Debug.LogError("❌ Lỗi khi lưu điểm mới: " + setTask.Exception);
                        });
                    }
                    else
                    {
                        Debug.Log("ℹ️ Điểm mới (" + score + ") không cao hơn (" + currentPoint + "), không lưu.");
                    }
                }
                else
                {
                    // Nếu chưa có điểm => lưu luôn
                    pointRef.SetValueAsync(score).ContinueWithOnMainThread(setTask =>
                    {
                        if (setTask.IsCompletedSuccessfully)
                            Debug.Log("🏆 Đã lưu điểm mới (lần đầu): " + score);
                        else
                            Debug.LogError("❌ Lỗi khi lưu điểm: " + setTask.Exception);
                    });
                }
            }
            else
            {
                Debug.LogError("❌ Lỗi khi đọc điểm hiện tại: " + task.Exception);
            }
        });
    }

    public void SaveSongScore(string userId, string songId, int score)
    {
        dbReference.Child("Users").Child(userId).Child("SongScores").Child(songId).SetValueAsync(score).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
                Debug.Log("✅ Đã lưu điểm bài hát: " + songId);
            else
                Debug.LogError("❌ Lỗi khi lưu điểm bài hát: " + task.Exception);
        });
    }
}

[System.Serializable]
public class User
{
    public string Name;
    public int Coins;
    public int Point;

    public User(string name, int coins, int point)
    {
        Name = name;
        Coins = coins;
        Point = point;
    }
}
