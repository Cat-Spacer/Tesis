using UnityEngine;

public class Shoot : MonoBehaviour
{
    public static void Fire(Bullet bullet, Transform firePoint, GameObject father)
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation).SetBullet(father);
        bullet.gameObject.SetActive(true);
        #region Pool & Factory
        /*Debug.Log($"{bullet} has position & direction");
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;*/
        #endregion
    }
}