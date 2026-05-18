using UnityEngine;

namespace CMProblemSolving.L1003_HealthSystem {

    public class Enemy : MonoBehaviour {

        
        private HealthSystem healthSystem;


        private void Start() {
            healthSystem = GetComponent<HealthSystem>();
            healthSystem.OnDead += HealthSystem_OnDead;
        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e) {
            Destroy(gameObject);
        }

    }

}