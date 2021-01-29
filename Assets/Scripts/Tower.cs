using UnityEngine;

public class Tower : IPoolable
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
			_renderersCache = _gameObject.GetComponentsInChildren<MeshRenderer>();
		}
	}

	public System.Guid Guid
	{
		get; private set;
	}

	private bool _towerActive;
	private bool TowerActive
	{
		set
		{
			if (_towerActive != value)
				TowerActiveStateChanged(value);
			_towerActive = value;
		}
	}

	private Transform _transformCache;
	private MeshRenderer[] _renderersCache;
	private GameController _gameController;
	private float _timeSinceLastRotation;
	private bool _isRotating;
	private float _rotationParameterT;
	private Quaternion _targetRotation;
	private Quaternion _sourceRotation;
	private float _targetAngle;
	private int _bulletsFired = 0;
	private float _activationDelay;
	private float _activationTimer;

	public Tower(GameController gameController)
	{
		Guid = System.Guid.NewGuid();
		_gameController = gameController;
	}

	public void Initialize(Vector3 position, float activationDelay = 0f)
	{
		_activationDelay = activationDelay;
		_activationTimer = 0f;
		_bulletsFired = 0;
		_transformCache.position = new Vector3(position.x, 0f, position.z);
	}

	public IPoolable CloneInstance()
	{
		return new Tower(_gameController);
	}

	public void ResetBulletCounter()
	{
		_bulletsFired = 0;
	}

	public void Update(float deltaTime, out bool bulletFired)
	{
		bulletFired = false;
		_activationTimer += deltaTime;
		TowerActive = IsTowerActive();
		if (_towerActive)
		{
			if (_isRotating)
			{
				UpdateRotation(deltaTime);
				if (_rotationParameterT >= 1)
				{
					CompleteRotation();
					FireBullet();
					bulletFired = true;
				}
			}
			else
			{
				_timeSinceLastRotation += deltaTime;
				if (_timeSinceLastRotation >= _gameController.GameSettings.TowerRotationInterval)
					StartRotation();
			}
		}
	}

	private void UpdateRotation(float deltaTime)
	{
		_rotationParameterT += _gameController.GameSettings.TowerRotationSpeed / _targetAngle * deltaTime;
		_transformCache.rotation = Quaternion.Lerp(_sourceRotation, _targetRotation, _rotationParameterT);
	}

	private void StartRotation()
	{
		_timeSinceLastRotation = 0f;
		_targetAngle = Random.Range(_gameController.GameSettings.TowerRotationAngleMin, _gameController.GameSettings.TowerRotationAngleMax);
		_sourceRotation = _transformCache.rotation;
		_targetRotation = _sourceRotation * Quaternion.AngleAxis(_targetAngle, Vector3.up);
		_isRotating = true;
	}

	private void CompleteRotation()
	{
		_isRotating = false;
		_rotationParameterT = 0f;		
	}

	private void FireBullet()
	{
		_bulletsFired++;
	}

	private void SetRendererColor(Color color)
	{
		foreach (Renderer renderer in _renderersCache)
			renderer.material.color = color;
	}

	private void TowerActiveStateChanged(bool active)
	{
		if (active)
			SetRendererColor(_gameController.GameSettings.TowerActiveColor);
		else
			SetRendererColor(_gameController.GameSettings.TowerInactiveColor);
	}

	public Vector3 GetDirection()
	{
		return _transformCache.forward;
	}

	public Vector3 GetShotPivotPosition()
	{
		return _transformCache.TransformPoint(_gameController.GameSettings.TowerShotPivot);
	}

	public Vector3 GetPosition()
	{
		return _transformCache.position;
	}

	private bool IsTowerActive()
	{
		return _activationTimer >= _activationDelay && _bulletsFired < _gameController.GameSettings.TowerShootingLifetime;
	}
}