using Weapons;

public class Turret : Gun
{
    private bool _canShoot = true;
    public bool ShootBool { get { return _canShoot; } set { _canShoot = value; } }

    private void Start()
    {
        _canShoot = true;
        base.Start();
    }
    private void Update()
    {
        if (_canShoot) FireCooldown();
    }
}