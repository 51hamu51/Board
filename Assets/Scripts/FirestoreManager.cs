using Firebase;
using Firebase.Firestore;
using UnityEngine;
using System.Collections.Generic;

public class FirestoreManager : MonoBehaviour
{
    FirebaseFirestore db;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firestore Initialized!");
            }
            else
            {
                Debug.LogError("Firebase dependencies are not available.");
            }
        });
    }

    public void SaveData()
    {
        Dictionary<string, object> user = new Dictionary<string, object>
    {
        { "name", "Alice" },
        { "age", 25 },
        { "score", 100 }
    };

        db.Collection("users").Document("user_001").SetAsync(user).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data saved successfully!");
            }
            else
            {
                Debug.LogError("Failed to save data: " + task.Exception);
            }
        });
    }

    public void AddData()
    {
        Dictionary<string, object> user = new Dictionary<string, object>
    {
        { "name", "Bob" },
        { "age", 30 },
        { "score", 200 }
    };

        db.Collection("users").AddAsync(user).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data added with ID: " + task.Result.Id);
            }
            else
            {
                Debug.LogError("Failed to add data: " + task.Exception);
            }
        });
    }

    public void LoadData()
    {
        db.Collection("users").Document("user_001").GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log($"User: {snapshot.GetValue<string>("name")}, Age: {snapshot.GetValue<long>("age")}, Score: {snapshot.GetValue<long>("score")}");
                }
                else
                {
                    Debug.Log("No such document!");
                }
            }
            else
            {
                Debug.LogError("Failed to load data: " + task.Exception);
            }
        });
    }
    public void LoadAllData()
    {
        db.Collection("users").GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                foreach (DocumentSnapshot doc in task.Result.Documents)
                {
                    Debug.Log($"User ID: {doc.Id}, Name: {doc.GetValue<string>("name")}");
                }
            }
            else
            {
                Debug.LogError("Failed to load data: " + task.Exception);
            }
        });
    }
    public void ListenToChanges()
    {
        db.Collection("users").Document("user_001").Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                Debug.Log($"Updated User: {snapshot.GetValue<string>("name")}, Score: {snapshot.GetValue<long>("score")}");
            }
        });
    }





}
