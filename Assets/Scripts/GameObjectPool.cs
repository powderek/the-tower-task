using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
	IPoolable CloneInstance();
	GameObject GameObject
	{
		get; set;
	}
}

public class GameObjectPool<T> where T : IPoolable
{
	public List<T> ActiveObjects
	{
		get; private set;
	}

	public event System.Action onActiveObjectsChanged = () => { };

	private Queue<T> _pooledObjects;
	private GameObjectPoolItem _itemToPool;
	private T _itemPrototype;

	public GameObjectPool(GameObjectPoolItem itemToPool, T itemPrototype)
	{
		_itemToPool = itemToPool;
		_itemPrototype = itemPrototype;
		_pooledObjects = new Queue<T>(itemToPool.InitialPoolSize);
		ActiveObjects = new List<T>(itemToPool.InitialPoolSize);
		_itemToPool.Parent = new GameObject(itemToPool.ObjectPrefab.name).transform;
		InstantiateItems(itemToPool.InitialPoolSize);
	}

	public T GetPooledItem()
	{
		if (_pooledObjects.Count <= 0)
		{
			InstantiateItems(_itemToPool.ExpandPoolSize);
			Debug.LogWarning($"Not enough items in the pool. Expand pool by {_itemToPool.ExpandPoolSize}.");
		}
		T item = _pooledObjects.Dequeue();
		ActiveObjects.Add(item);
		item.GameObject.SetActive(true);
		onActiveObjectsChanged.Invoke();
		return item;
	}

	public void ReturnToPool(T target)
	{
		target.GameObject.SetActive(false);
		_pooledObjects.Enqueue(target);
		ActiveObjects.Remove(target);
		onActiveObjectsChanged.Invoke();
	}

	private void InstantiateItems(int quantity)
	{
		for (int i = 0; i < quantity; i++)
		{
			GameObject gameObjectInstance = Object.Instantiate(_itemToPool.ObjectPrefab, _itemToPool.Parent);
			IPoolable itemInstance = _itemPrototype.CloneInstance();
			itemInstance.GameObject = gameObjectInstance;
			ReturnToPool((T)itemInstance);
		}
	}
}
