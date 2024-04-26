using EnemyHealth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorialSection : TutorialSection
{
    [SerializeField] protected List<EnemyController> enemies;

    private void Start()
    {
        if(enemies.Count > 0)
        {
            foreach(EnemyController enemy in enemies)
            {
                enemy.Death += OnEnemyDeath;
            }
        }
    }

    private void OnEnemyDeath(EnemyController enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            FinishSection();
        }
    }
}
