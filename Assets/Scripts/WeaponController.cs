using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform barrel;

    [Header("Ammo")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private bool infiniteAmmo;

    [Header("performance")]
    [SerializeField] private float bulledSpeed;
    [SerializeField] private float shootRate;
    [SerializeField] private int damage;

    private ObjectPool objectPool;
    private float lastShootTime;

    private bool isPlayer;

    private void Awake()
    {
        //Check if I am a Player
        isPlayer = GetComponent<PlayerMovement>() != null;

        //get objectPool component
        objectPool = GetComponent < ObjectPool>();
    }


    /// <summary>
    /// Check if is possible to shoot
    /// </summary>
    /// <returns>bool</returns>
    public bool CanShoot()
    {
        //Check shootRate
        if(Time.time - lastShootTime >= shootRate)
        {
            //Check Ammo
            if(currentAmmo >0 || infiniteAmmo)
            {
                return true;
            }
        }
        return false;
    }

    //Handle Weapon Shoot
    public void Shoot()
    {
        //Update last shoot time
        lastShootTime = Time.time;

        //reduce the ammo
        if (!infiniteAmmo) currentAmmo--;

        //Get a new bullet
        GameObject bullet = objectPool.GetGameObject();

        //Locate the ball at barrel position
        bullet.transform.position= barrel.position;
        bullet.transform.rotation= barrel.rotation;
        //TODO BulletControlller assgn damage
        bullet.GetComponent<BulletController>().Damage = damage;


        //Give velocity to bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulledSpeed;
    }
}
