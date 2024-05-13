using UnityEngine;

public class Shoot : MonoBehaviour
{
    public static void Fire(Bullet bullet, Transform firePoint, GameObject father)
    {
        //Instantiate(bullet, firePoint.position, firePoint.rotation).SetBullet(father);
        bullet.SetBullet(father);
        bullet.gameObject.SetActive(true);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
    }
}