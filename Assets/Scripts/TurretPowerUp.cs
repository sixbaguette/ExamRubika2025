public class TurretPowerUp : PowerUpBase
{
    private int bulletCount;
    private int maxBulletCount;
    private UIManager uiManager;

    private float fireRate;

    public void ApplyPowerUpTurret() // a finir
    {
        if (bulletCount < maxBulletCount)
        {
            bulletCount = FindAnyObjectByType<WeaponSystem>().BulletCount++;

            StartCoroutine(uiManager.ShowPowerupMessage("Weapon Upgraded! Bullets: " + bulletCount));
        }
    }
}
