using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyHealth;

public class EnemyPool : MonoBehaviour {

    public GameObject meleePrefab, shooterPrefab;

    public int quantity = 8;

    void Awake() {
        Floors.Room.enemyPool = this;
    }

    private void Start() {
        for(int i = 0; i < quantity; i++) {
            Instantiate(meleePrefab, this.transform).SetActive(false);
            Instantiate(shooterPrefab, this.transform).SetActive(false);
        }
    }

    public EnemyController GetEnemy(EnemyTemplate template) {
        EnemyController enemy;
        switch (template.kind) {
            case EnemyKind.Melee:
                enemy = this.transform.Find(meleePrefab.name + "(Clone)").GetComponent<EnemyController>();
                break;

            case EnemyKind.Shooter:
                enemy = this.transform.Find(shooterPrefab.name + "(Clone)").GetComponent<EnemyController>();
                break;

            default:
                enemy = this.transform.Find(meleePrefab.name + "(Clone)").GetComponent<EnemyController>();
                break;
        }

        enemy.ResetEnemy(template.health, template.color, template.damage);
        return enemy;
    }
}
public class EnemyTemplate {

    public EnemyKind kind { get; }
    public int health { get; }
    public DamageColor color { get; }
    public int damage { get; }

    public EnemyTemplate(EnemyKind kind, int health, DamageColor color, int damage) {
        this.kind = kind;
        this.health = health;
        this.color = color;
        this.damage = damage;
    }
}

public enum EnemyKind {
    Melee,
    Shooter
}
