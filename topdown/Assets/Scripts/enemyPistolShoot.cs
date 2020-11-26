using UnityEngine;

public class EnemyPistolShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform firePoint;
    private int beatCount = 0;
    private EnemyController enemyController;

    void OnBeat()
    {
        if (enemyController.isActive && beatCount == 1) //shoot every second beat
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
        beatCount++;
        if (beatCount > 1)
        {
            beatCount = 0;
        }
    }

    void Start()
    {
        FloorGlobal.Instance.onBeat.AddListener(OnBeat);
        enemyController = transform.parent.GetComponent<EnemyController>();
        enemyController.enemyScripts.Add(this);
        enemyController.weapons.Add(gameObject);
    }

}
