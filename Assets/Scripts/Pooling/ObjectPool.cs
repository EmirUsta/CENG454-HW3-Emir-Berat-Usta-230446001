using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _available = new Queue<T>();

    public ObjectPool(T prefab, Transform parent, int initialSize = 5)
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < initialSize; i++)
            CreateNew();
    }

    private void CreateNew()
    {
        T instance = Object.Instantiate(_prefab, _parent);
        instance.gameObject.SetActive(false);
        _available.Enqueue(instance);
    }

    public T Get()
    {
        if (_available.Count == 0)
            CreateNew();
        T item = _available.Dequeue();
        item.gameObject.SetActive(true);
        item.OnSpawned();
        return item;
    }

    public void Return(T item)
    {
        item.OnDespawned();
        item.gameObject.SetActive(false);
        _available.Enqueue(item);
    }
}
