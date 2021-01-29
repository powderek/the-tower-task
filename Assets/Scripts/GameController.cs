using UnityEngine;

public class FlyweightTower
{
	public GameObject gameObject;
}

public class GameController : MonoBehaviour
{
	[SerializeField]
	private GameObjectPoolItem _towerPoolItem;
	[SerializeField]
	private GameObjectPoolItem _bulletPoolItem;
	[SerializeField]
	private UnityEngine.UI.Text _towersCountText;
	[SerializeField]
	private GameSettings _gameSettings;
	public GameSettings GameSettings
	{
		get => _gameSettings;
	}

	private GameObjectPool<Bullet> _bulletObjectPool;
	private GameObjectPool<Tower> _towerObjectPool;
	private bool _towersLimitReached;
	private bool _bulletFired;
	private bool _towerInstantiated;
	private bool _bulletDestroyed;
	private Tower _destroyedTower;

	private void Awake()
	{
		Bullet bulletPrototype = new Bullet(this);
		_bulletObjectPool = new GameObjectPool<Bullet>(_bulletPoolItem, bulletPrototype);
		Tower towerPrototype = new Tower(this);
		_towerObjectPool = new GameObjectPool<Tower>(_towerPoolItem, towerPrototype);
	}

	private void Start()
	{
		_towerObjectPool.GetPooledItem().Initialize(Vector3.zero, 0f);
	}

	private void OnEnable()
	{
		_towerObjectPool.onActiveObjectsChanged += OnActiveTowerCountChanged;
	}

	private void OnDisable()
	{
		_towerObjectPool.onActiveObjectsChanged -= OnActiveTowerCountChanged;
	}

	private void Update()
	{
		foreach (Tower tower in _towerObjectPool.ActiveObjects)
		{
			tower.Update(Time.deltaTime, out _bulletFired);
			if (_bulletFired)
				FireBullet(tower);
		}

		for (int i = _bulletObjectPool.ActiveObjects.Count - 1; i >= 0; i--)
		{
			_bulletObjectPool.ActiveObjects[i].Update(Time.deltaTime, out _towerInstantiated, out _bulletDestroyed, out _destroyedTower);
			if (_towerInstantiated)
				InstantiateTower(_bulletObjectPool.ActiveObjects[i]);
			if (_bulletDestroyed)
				DestroyBullet(_bulletObjectPool.ActiveObjects[i]);
			if (_destroyedTower != null)
				DestroyTower(_destroyedTower);
		}	
	}

	private void OnActiveTowerCountChanged() 
	{
		_towersCountText.text = _towerObjectPool.ActiveObjects.Count.ToString();
	}

	private void FireBullet(Tower sourceTower)
	{
		_bulletObjectPool.GetPooledItem().Initialize(sourceTower);
	}

	private void InstantiateTower(Bullet sourceBullet)
	{
		if (_towersLimitReached == false)
		{
			_towerObjectPool.GetPooledItem().Initialize(sourceBullet.GetPosition(), _gameSettings.TowerStartDelay);
			_towersLimitReached = _towerObjectPool.ActiveObjects.Count >= _gameSettings.TowersLimit;
			if (_towersLimitReached)
				ResetActiveTowers();
		}
	}

	private void DestroyBullet(Bullet targetBullet)
	{
		_bulletObjectPool.ReturnToPool(targetBullet);
	}

	private void DestroyTower(Tower targetTower)
	{
		_towerObjectPool.ReturnToPool(targetTower);
	}

	private void ResetActiveTowers()
	{
		foreach (Tower tower in _towerObjectPool.ActiveObjects)
			tower.ResetBulletCounter();
	}

	public bool IsInsideTower(float distanceFromTower)
	{
		return distanceFromTower <= GameSettings.TowerSqrRadius;
	}

	public Tower GetClosestTower(Vector3 position, out float distanceFromTower)
	{
		distanceFromTower = float.MaxValue;
		Tower closestTower = null;

		foreach (Tower tower in _towerObjectPool.ActiveObjects)
		{
			float distance = Vector3.SqrMagnitude(tower.GetPosition() - position);
			if (distance < distanceFromTower)
			{
				distanceFromTower = distance;
				closestTower = tower;
			}
		}
		return closestTower;
	}
}