using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int beatActionDelay = 1;
    [SerializeField]
    private Transform firePoint;
    private int beatCount = 1;
    private EnemyController enemyController;

    public void OnBeat()
    {
        if (enemyController.isActive && beatCount == beatActionDelay) //shoot every second beat
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
        beatCount++;
        if (beatCount > beatActionDelay)
        {
            beatCount = 1;
        }
    }

    private void Start()
    {
        FloorGlobal.Instance.onBeat.AddListener(OnBeat);
        enemyController = transform.parent.GetComponent<EnemyController>();
        enemyController.enemyScripts.Add(this);
        enemyController.weapons.Add(gameObject);
    }

}
