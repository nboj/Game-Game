using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject {
        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float attackDistance = 1f;
        [SerializeField] float enemySpeed = 2000f;
        [SerializeField] Sprite enemySprite;
        public float ChaseDistance { get => chaseDistance; }
        public float AttackDistance { get => attackDistance; }
        public float EnemySpeed { get => enemySpeed; }
        public Sprite EnemySprite { get => enemySprite; }
}