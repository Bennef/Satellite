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
            _healthLeft = 100;
            _maxHealth = 100;
        }

        // Deals damage to the thing.
        public virtual void TakeDamage(int damageToTake)
        {
            if (_isInvincible)
                return;
            Invincible();
            damageToTake = Mathf.Clamp(damageToTake, 0, _maxHealth);
            _healthLeft -= damageToTake;
        }

        // Increase the health of the thing.
        public void IncreaseHealth(int healthToAdd)
        {
            _healthLeft += healthToAdd;
            _healthLeft = Mathf.Clamp(HealthLeft, 0, _maxHealth);
        }

        public void SetHealth(int valueToSet)
        {
            _healthLeft = valueToSet;
            _healthLeft = Mathf.Clamp(_healthLeft, 0, _maxHealth);
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