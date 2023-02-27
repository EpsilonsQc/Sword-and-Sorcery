using UnityEngine;

namespace RPG_Project.AI.Manager
{
    public class DataManager : MonoBehaviour
    {
        [Header("IsDead")]
        public bool isDead;

        [Header("Name")]
        public new string name;

        [Header("Ressources")]
        public int healthPoints;
        public int manaPoints;

        [Header("Attack stats")]
        public float attackRange;
        public float attackSpeed;
        public int minAttackDamage;
        public int maxAttackDamage;

        [Header("Field of View")]
        public float FOVRange;

        [Header("Distance offset from target/player")]
        public float distanceOffset;
    }
}
