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

    private bool isFirebaseReady = false;
    internal readonly bool IsReady;

    private void Start()
    {


        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                isFirebaseReady = true;
                LoadUserDataAndDisplay(); // gọi ở đây đảm bảo firebase ready
            }
        });

        Debug.Log("🟡 Đang khởi tạo Firebase...");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("✅ Firebase đã sẵn sàng!");
            }
            else
            {
                Debug.LogError("❌ Firebase chưa sẵn sàng: " + status);
            }
        });
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

    public void UpdatePoint(string userId, int newPoint)
    {
        dbReference.Child("Users").Child(userId).Child("Point").SetValueAsync(newPoint).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
                Debug.Log("✅ Cập nhật Point thành công.");
            else
                Debug.LogError("❌ Lỗi khi cập nhật Point: " + task.Exception);
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
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        dbReference.Child("Users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    User user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                    Debug.Log($"👤 {user.Name} | 💰 {user.Coins} | 🏆 {user.Point}");

                    if (nameText != null) nameText.text = user.Name;
                    if (coinsText != null) coinsText.text = user.Coins.ToString();
                    if (pointText != null) pointText.text = user.Point.ToString();
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

    public void UpdateUserData(string userId, int newCoins, int newPoint)
    {
        Dictionary<string, object> updates = new Dictionary<string, object>();
        updates["Coins"] = newCoins;
        updates["Point"] = newPoint;

        dbReference.Child("Users").Child(userId).UpdateChildrenAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("✅ Dữ liệu đã được cập nhật!");
            }
            else
            {
                Debug.LogError("❌ Cập nhật dữ liệu thất bại: " + task.Exception);
            }
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


