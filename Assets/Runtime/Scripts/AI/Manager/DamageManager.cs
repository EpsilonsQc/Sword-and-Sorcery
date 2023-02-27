using UnityEngine;
using UnityEngine.AI;

namespace RPG_Project.AI.Manager
{
    public class DamageManager : MonoBehaviour
    {
        private Transform _transform;
        private DataManager _dataManager;

        private void Awake()
        {
            _transform = this.transform;
            _dataManager = transform.GetComponent<DataManager>();
        }

        public bool TakeHit()
        {
            // TO DO : Add a DataManager for the PLAYER and use the attack damage of the player instead.
            int _damage = Random.Range(_dataManager.minAttackDamage, _dataManager.maxAttackDamage);
            _dataManager.healthPoints -= _damage;

            _dataManager.isDead = _dataManager.healthPoints <= 0; // Check if the entity is dead or not

            if(_dataManager.isDead)
            {
                _dataManager.healthPoints = 0; // Reset health to 0 (in case it's negative)
            }

            return _dataManager.isDead; // Return if the entity is dead or not
        }
    }
}
