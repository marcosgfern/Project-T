using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyHealth;

/* Class EnemyPool is used as a component for game object EnemyPool.
 * Implements the pooling technique for enemy spawning.
 */ 
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

    /* Sets an exisiting enemy in pool with the stats specified in @template.
     * Returns: Component EnemyController of the changed enemy.
     */
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

    /* Instantiates a new enemy of the @prefab 's kind in the pool. */
    private EnemyController AddEnemyToPool(GameObject prefab) {
        GameObject enemy = Instantiate(prefab, this.transform);
        enemy.SetActive(false);
        return enemy.GetComponent<EnemyController>();
    }
}

/* Class EnemyTemplate is used as a template for enemies.
 * Used by other classes and game objects to obtain enemies from the enemy pool.
 */
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
