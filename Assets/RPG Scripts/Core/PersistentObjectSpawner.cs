using UnityEngine;
using RPG.SceneManagement;
using System;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        // CONFIG DATA
        [Tooltip("This prefab will only be spawned once and persisted between " +
        "scenes.")]
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;

        void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects();

            hasSpawned = true;
        }

        void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}