using UnityEngine;

[System.Serializable]
public class GameObjectPoolItem
{
	[SerializeField]
	private GameObject _objectPrefab = null;
	public GameObject ObjectPrefab
	{
		get => _objectPrefab;
	}
	[SerializeField]
	private int _initialPoolSize = 10;
	public int InitialPoolSize
	{
		get => _initialPoolSize;
	}
	[SerializeField]
	private int _expandPoolSize = 5;
	public int ExpandPoolSize
	{
		get => _expandPoolSize;
	}
	public Transform Parent
	{
		get; set;
	}
}