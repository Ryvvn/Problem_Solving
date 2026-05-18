using System;
using UnityEngine;


namespace CMProblemSolving.L1002_HealthSystem {

    public class HealthSystem : MonoBehaviour {


        public event EventHandler OnHealthAmountChanged;


        [SerializeField] private int healthAmount = 100;
        [SerializeField] private int healthAmountMax = 100;


        public void Damage(int damageAmount) {
            healthAmount += damageAmount;
            healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

            OnHealthAmountChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Heal(int healAmount) {
            healthAmount += healAmount;
            healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

            OnHealthAmountChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetHealthAmount() {
            return healthAmount;
        }

        public float GetHealthAmountNormalized() {
            return (float)healthAmount / healthAmountMax;
        }

        public bool IsDead() {
            return healthAmount <= 0;
        }



    }

}