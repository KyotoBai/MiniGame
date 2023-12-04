using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    public GameObject enemyParent;
    public bool shootingOn = true;
    [SerializeField] private Camera sceneCam;
    [SerializeField] private LayerMask groundLayermask;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerPrefabControll playerPrefabControll;
    [SerializeField] private KeyCode shootingKey = KeyCode.None;
    [SerializeField] private OpenClose openClose;
    [SerializeField] private GameEconomyManager gameEconomyManager;

    /*
    [Header("Bullet Properties")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 100;
    [SerializeField] private float bulletAoESize = 0f;
    [SerializeField] private float bulletCooldown = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletScale = 1f;
    [SerializeField] private Vector3 bulletOffset = Vector3.zero;
    [SerializeField] private LayerMask bulletTargetLayers;*/
    [SerializeField] private KeyCode bulletKey = KeyCode.None;

    [Header("Projectile Properties")]
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int projectileDamage = 100;
    [SerializeField] private float projectileAoESize = 0f;
    [SerializeField] private float projectileCooldown = 2f;
    [SerializeField] private int projectilePrice = 50;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileScale = 1f;
    [SerializeField] private Vector3 projectileOffset = Vector3.zero;
    [SerializeField] private LayerMask projectileTargetLayers;
    [SerializeField] private KeyCode projectileKey = KeyCode.None;

    private GunShooting GunShooting;
    private GunShooting throwing;
    private float nextBulletTime = 0f;
    private float nextProjectileTime = 0f;
    private Vector3 targetDirection;
    private Transform placeGunHere;


    void Start()
    {
        GameObject playerWithGun = GameObject.Find("Player With Gun");

        if (playerWithGun != null)
        {
            placeGunHere = playerWithGun.transform.Find("Place Gun Here");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(shootingKey) && openClose.isAllStateOff())
        {
            shootingOn = !shootingOn;
        }
        if (!shootingOn)
        {
            playerController.weaponOn = false;
            return;
        }
        playerController.weaponOn = true;
        Vector3 mousePos = GetSelectedMapPosition();
        // Debug.Log(mousePos);
        targetDirection = mousePos - transform.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        transform.rotation = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(0, -90, 0);
        /*if (Input.GetKeyDown(bulletKey) || Input.GetKeyDown(projectileKey))
        { playerController.stopMovement = true; } 
        else if (Input.GetKeyUp(bulletKey) || Input.GetKeyUp(projectileKey))
        { playerController.stopMovement = false; }
        */

        if (Input.GetKey(bulletKey) && Time.time >= nextBulletTime && openClose.isAllStateOff()) // Check for bullet cooldown
        {
            nextBulletTime = Time.time + GunShooting.GetCoolDown();
            //FireBullet();
            GunShooting.Fire(targetDirection);
            //weaponPrefabParent.GetComponentInChildren<>
            placeGunHere.GetChild(playerPrefabControll.getWeaponOnIndex()).GetComponent<AudioSource>().Play();
        }

        if (Input.GetKeyDown(projectileKey) && Time.time >= nextProjectileTime && openClose.isAllStateOff()) // Check for projectile cooldown
        {
            nextProjectileTime = Time.time + projectileCooldown;
            FireProjectile();
            
        }
    }

    public void SetGunShooting(GunShooting gs)
    {
        GunShooting = gs;
    }

    public void SetThrowing(GunShooting gs)
    {
        throwing = gs;
    }


    public KeyCode GetProjectionKey()
    {
        return projectileKey;
    }

    /*
    void FireBullet()
    {
        GameObject bullet = InstantiateBullet(bulletPrefab);
        bullet.transform.position += bulletOffset;
        bullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
        BulletController bulletController = bullet.AddComponent<BulletController>();
        bulletController.type = BulletController.BulletType.BulletPlayer;
        bullet.transform.LookAt(targetDirection);
        Vector3 targetPoint = transform.position + targetDirection * 10f;
        bulletController.target = targetPoint;
        bulletController.speed = bulletSpeed;
        bulletController.damage = bulletDamage;
        bulletController.aoESize = bulletAoESize;
        bulletController.targetLayers = bulletTargetLayers;

        // transform.rotation = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(0, -90, 0);
    }*/

    
    void FireProjectile()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            if (!gameEconomyManager.SpendCoins(projectilePrice))
            {
                return;
            }
            GameObject projectile = InstantiateBullet(projectilePrefab);
            projectile.transform.position += projectileOffset;
            projectile.transform.localScale = new Vector3(projectileScale, projectileScale, projectileScale);
            BulletController bulletController = projectile.AddComponent<BulletController>();
            bulletController.type = BulletController.BulletType.Projectile;
            bulletController.target = nearestEnemy.transform.position;
            bulletController.speed = projectileSpeed;
            bulletController.damage = projectileDamage;
            bulletController.aoESize = projectileAoESize;
            bulletController.targetLayers = projectileTargetLayers;
        }
    }

    GameObject InstantiateBullet(GameObject prefab)
    {
        if (prefab == null)
        {
            return CreateDefaultBullet();
        }
        else
        {
            return Instantiate(prefab, transform.position, Quaternion.LookRotation(transform.forward));
        }
    }

    GameObject CreateDefaultBullet()
    {
        GameObject defaultBullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        defaultBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        defaultBullet.transform.position = transform.position;
        return defaultBullet;
    }
    
    GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Transform child in enemyParent.transform)
        {
            float distance = Vector3.Distance(transform.position, child.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = child.gameObject;
            }
        }

        return nearestEnemy;
    }

    Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = sceneCam.nearClipPlane;

        //check if we pointing on plain
        Ray ray = sceneCam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 300, groundLayermask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

}
