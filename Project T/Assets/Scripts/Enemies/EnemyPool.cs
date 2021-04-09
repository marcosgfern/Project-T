using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyHealth;

public class EnemyPool : MonoBehaviour {

    public GameObject meleePrefab, shooterPrefab;

    public int initialSize = 8;

    void Awake() {
        Floors.Room.enemyPool = this;
    }

    private void Start() {
        for(int i = 0; i < initialSize; i+=2) {
            Instantiate(meleePrefab, this.transform).SetActive(false);
            Instantiate(shooterPrefab, this.transform).SetActive(false);
        }
    }

    public EnemyController GetEnemy(EnemyTemplate template) {
        EnemyController enemy;
        switch (template.kind) {
            case EnemyKind.Melee:
                enemy = GetEnemyByKind(meleePrefab);
                break;

            case EnemyKind.Shooter:
                enemy = GetEnemyByKind(shooterPrefab);
                break;

            default:
                enemy = GetEnemyByKind(meleePrefab);
                break;
        }

        enemy.ResetEnemy(template.health, template.color, template.damage);
        return enemy;
    }

    private EnemyController GetEnemyByKind(GameObject prefab) {
        EnemyController enemy = null;
        for(int i = 0; i < this.transform.childCount; i++) {
            if (!this.transform.GetChild(i).gameObject.activeSelf
                    && this.transform.GetChild(i).name == (prefab.name + "(Clone)")) {
                enemy = this.transform.GetChild(i).GetComponent<EnemyController>();
                break;
            }
        }

        if(enemy == null) {
            return AddEnemyToPool(prefab);
        } else {
            return enemy;
        }
    }

    private EnemyController AddEnemyToPool(GameObject prefab) {
        GameObject enemy = Instantiate(prefab, this.transform);
        enemy.SetActive(false);
        return enemy.GetComponent<EnemyController>();
    }
}
public class EnemyTemplate {

    public EnemyKind kind { get; set; }
    public int health { get; set; }
    public DamageColor color { get; set; }
    public int damage { get; set; }

    public EnemyTemplate() { }

    public EnemyTemplate(EnemyKind kind, int health, DamageColor color, int damage) {
        this.kind = kind;
        this.health = health;
        this.color = color;
        this.damage = damage;
    }

    override public string ToString() {
        return this.kind
            + ", health: " + this.health
            + ", " + this.color
            + ", damage: " + this.damage;
    }
}

public enum EnemyKind {
    Melee,
    Shooter
}
