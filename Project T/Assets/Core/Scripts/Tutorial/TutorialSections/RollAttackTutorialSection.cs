using EnemyHealth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAttackTutorialSection : TutorialSection
{
    [SerializeField] protected List<EnemyHealthController> enemies;

    private int defeatedEnemies = 0;

    private void Start()
    {
        if(enemies.Count > 0)
        {
            foreach(EnemyHealthController enemy in enemies)
            {
                enemy.Death += OnEnemyDeath;
            }
        }
    }

    private void OnEnemyDeath()
    {
        defeatedEnemies++;
        Debug.LogError(defeatedEnemies);
        if (defeatedEnemies >= enemies.Count)
        {
            FinishSection();
        }
    }
}
