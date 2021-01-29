using UnityEngine;

public class Bullet : IPoolable
{
	private GameObject _gameObject;
	public GameObject GameObject
	{
		get
		{
			return _gameObject;
		}
		set
		{
			_gameObject = value;
			_transformCache = _gameObject.transform;
		}
	}

	private Transform _transformCache;
	private GameController _gameController;
	private Vector3 _startPosition;
	private Vector3 _targetPosition;
	private float _targetDistance;
	private System.Guid _sourceTowerGuid;
	private float _movementParameterT;

	public Bullet(GameController gameController)
	{
		_gameController = gameController;		
	}

	public IPoolable CloneInstance()
	{
		return new Bullet(_gameController);
	}

	public void Initialize(Tower sourceTower)
	{
		_sourceTowerGuid = sourceTower.Guid;
		_startPosition = sourceTower.GetShotPivotPosition();
		_targetDistance = Random.Range(_gameController.GameSettings.BulletMovementDistanceMin, _gameController.GameSettings.BulletMovementDistanceMax);
		_targetPosition = _startPosition + sourceTower.GetDirection() * _targetDistance;
		_movementParameterT = 0f;
	}

	public void Update(float deltaTime, out bool towerInstantiated, out bool bulletDestroyed, out Tower destroyedTower)
	{
		bulletDestroyed = false;
		towerInstantiated = false;
		destroyedTower = null;
		if (_movementParameterT >= 1)
		{
			towerInstantiated = true;
			bulletDestroyed = true;
		}
		else
		{
			_movementParameterT += _gameController.GameSettings.BulletMovementSpeed / _targetDistance * deltaTime;
			_transformCache.position = Vector3.Lerp(_startPosition, _targetPosition, _movementParameterT);
			Tower closestTower;
			if (TryCheckTowerCollision(out closestTower))
			{
				bulletDestroyed = true;
				destroyedTower = closestTower;
			}
		}
	}

	private bool TryCheckTowerCollision(out Tower closestTower)
	{
		float distanceFromTower;
		closestTower = _gameController.GetClosestTower(_transformCache.position, out distanceFromTower);
		return closestTower != null && closestTower.Guid != _sourceTowerGuid && _gameController.IsInsideTower(distanceFromTower);
	}

	public Vector3 GetPosition()
	{
		return _transformCache.position;
	}
}