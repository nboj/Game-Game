using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject {
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float attackDistance = 1f;
    [SerializeField] float enemySpeed = 2000f;
    [SerializeField] float repelDistance = 0.5f;
    [SerializeField] Sprite enemySprite; 

    public float ChaseDistance {
        get => chaseDistance;
    }

    public float AttackDistance {
        get => attackDistance;
    }

    public float EnemySpeed {
        get => enemySpeed;
    }

    public float RepelDistance {
        get => repelDistance;
    }

    public Sprite EnemySprite {
        get => enemySprite;
    }
}