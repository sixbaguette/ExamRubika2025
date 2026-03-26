public class WeaponPowerUp : PowerUpBase
{
    private int bulletCount;
    private int maxBulletCount;
    private int score;

    private UIManager uiManager;

    private void Start()
    {
        bulletCount = FindAnyObjectByType<WeaponSystem>().BulletCount;
        maxBulletCount = FindAnyObjectByType<WeaponSystem>().MaxBulletCount;
        score = FindAnyObjectByType<GameManager>().Score;

        uiManager = FindAnyObjectByType<UIManager>();
    }

    public void ApplyPowerUp()
    {
        // Augmenter le nombre de projectiles pour tous les power-ups
        if (bulletCount < maxBulletCount)
        {
            bulletCount++;

            // Affichage d'un message temporaire pour informer le joueur
            StartCoroutine(uiManager.ShowPowerupMessage("Weapon Upgraded! Bullets: " + bulletCount));
        }
        else
        {
            // Bonus de score si le joueur a d�j� le maximum de projectiles
            score += 200;
            StartCoroutine(uiManager.ShowPowerupMessage("Max Weapon Level! +200 Score"));
        }
    }
}
