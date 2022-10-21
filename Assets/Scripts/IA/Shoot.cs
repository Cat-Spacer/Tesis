using UnityEngine;

public class Shoot : MonoBehaviour
{
    public static void Fire(GameObject bullet, Transform firePoint)
    {
        ///Instantiate(bullet, firePoint.position, firePoint.rotation);
        //bullet.SetActive(true);
        Debug.Log($"{bullet} has position & direction");
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
    }
}
