using System.Collections;
using UnityEngine;

namespace Scripts.Player
{
    public class Health : MonoBehaviour
    {
        // Manages health.
        [SerializeField] private int _healthLeft;
        private int _maxHealth;
        private bool _isInvincible;

        public int HealthLeft { get => _healthLeft; set => _healthLeft = value; }

        void Start()
        {
            HealthLeft = 100;
            _maxHealth = 100;
        }

        // Deals damage to the thing.
        public virtual void TakeDamage(int damageToTake)
        {
            if (_isInvincible)
                return;
            Invincible();
            HealthLeft -= damageToTake;
            damageToTake = Mathf.Clamp(damageToTake, 0, _maxHealth);
        }

        // Increase the health of the thing.
        public void IncreaseHealth(int healthToAdd)
        {
            HealthLeft += healthToAdd;
            HealthLeft = Mathf.Clamp(HealthLeft, 0, _maxHealth);
        }

        public void SetHealth(int valueToSet)
        {
            HealthLeft = valueToSet;
            HealthLeft = Mathf.Clamp(HealthLeft, 0, _maxHealth);
        }

        protected void Invincible() => StartCoroutine(InvincibleCo());

        // Make thing invincible for a short time after getting hit.
        IEnumerator InvincibleCo()
        {
            _isInvincible = true;
            yield return new WaitForSeconds(0.2f);
            _isInvincible = false;
        }

        public void Reset() => HealthLeft = _maxHealth;
    }
}