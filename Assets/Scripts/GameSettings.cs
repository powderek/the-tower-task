using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
	[Header("Tower")]

	[SerializeField]
	private int _towersLimit = 100;
	public int TowersLimit
	{
		get => _towersLimit;
	}

	[SerializeField]
	private float _towerRotationSpeed = 1f;
	public float TowerRotationSpeed
	{
		get => _towerRotationSpeed;
	}

	[SerializeField]
	private float _towerRotationAngleMin = 15f;
	public float TowerRotationAngleMin
	{
		get => _towerRotationAngleMin;
	}

	[SerializeField]
	private float _towerRotationAngleMax = 45f;
	public float TowerRotationAngleMax
	{
		get => _towerRotationAngleMax;
	}

	[SerializeField]
	private float _towerRotationInterval = 0.5f;
	public float TowerRotationInterval
	{
		get => _towerRotationInterval;
	}

	[SerializeField]
	private float _towerStartDelay = 6f;
	public float TowerStartDelay
	{
		get => _towerStartDelay;
	}

	[SerializeField]
	private int _towerShootingLifetime = 12;
	public int TowerShootingLifetime
	{
		get => _towerShootingLifetime;
	}

	[SerializeField]
	private Color32 _towerActiveColor = new Color32(255, 0, 0, 255);
	public Color32 TowerActiveColor
	{
		get => _towerActiveColor;
	}

	[SerializeField]
	private Color32 _towerInactiveColor = new Color32(255, 255, 255, 255);
	public Color32 TowerInactiveColor
	{
		get => _towerInactiveColor;
	}

	[Header("Optimized tower helpers")]

	[SerializeField]
	private Vector3 _towerShotPivot = Vector3.zero;
	public Vector3 TowerShotPivot
	{
		get => _towerShotPivot;
	}

	[SerializeField]
	private float _towerSqrRadius = 0.0625f;
	public float TowerSqrRadius
	{
		get => _towerSqrRadius;
	}

	[Header("Bullet")]

	[SerializeField]
	private float _bulletMovementSpeed = 4f;
	public float BulletMovementSpeed
	{
		get => _bulletMovementSpeed;
	}

	[SerializeField]
	private float _bulletMovementDistanceMin = 1;
	public float BulletMovementDistanceMin
	{
		get => _bulletMovementDistanceMin;
	}

	[SerializeField]
	private float _bulletMovementDistanceMax = 4;
	public float BulletMovementDistanceMax
	{
		get => _bulletMovementDistanceMax;
	}
}