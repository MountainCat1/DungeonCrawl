using UnityEngine;
using Zenject;

namespace DefaultNamespace.Systems
{
    public interface ISpawnManager
    {
        T Spawn<T>(T prefab) where T : Component;
        T Spawn<T>(T prefab, Transform parent) where T : Component;
        T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component;
        T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component;

        GameObject Spawn(GameObject prefab);
        GameObject Spawn(GameObject prefab, Transform parent);
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent);
    }

    public class SpawnManager : ISpawnManager
    {
        [Inject] private DiContainer _diContainer;

        public T Spawn<T>(T prefab) where T : Component
        {
            return _diContainer.InstantiatePrefab(prefab).GetComponent<T>();
        }

        public T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return _diContainer.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, parent)
                .GetComponent<T>();
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return _diContainer.InstantiatePrefab(prefab, position, rotation, null)
                .GetComponent<T>();
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
        {
            return _diContainer.InstantiatePrefab(prefab, position, rotation, parent)
                .GetComponent<T>();
        }

        public GameObject Spawn(GameObject prefab)
        {
            return _diContainer.InstantiatePrefab(prefab);
        }

        public GameObject Spawn(GameObject prefab, Transform parent)
        {
            return _diContainer.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return _diContainer.InstantiatePrefab(prefab, position, rotation, null);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            return _diContainer.InstantiatePrefab(prefab, position, rotation, parent);
        }
    }
}