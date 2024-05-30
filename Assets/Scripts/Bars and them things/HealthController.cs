using UnityEngine;

public class HealthController : StatConroller
{
    Animator animator;

    PlayerStatsFin playerStats;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("enemyBullet"))
        {
            SubAmount(1);
            animator.SetTrigger("Hurting");

        }
    }

    public void Start()
    {
        StartCoroutine(RecoverNew());
        playerStats = GetComponent<PlayerStatsManager>().playerStats;
        UpdateAllStats();

    }

    public void DamagePlayer(float amount)
    {
        SubAmount(amount);

        animator.SetTrigger("Hurting");
    }

    public void UpdateAllStats()
    {
        foreach (var stat in playerStats.stats)
        {
            switch (stat.statName)
            {
                case PlayerStatEnum.manaMax:
                    maxAmount = stat.value;
                    break;
                case PlayerStatEnum.manaCurrent:
                    currentAmount = stat.value;
                    break;
                case PlayerStatEnum.manaRecoverTime:
                    recoverTime = stat.value;
                    break;
            }
        }
    }
}
