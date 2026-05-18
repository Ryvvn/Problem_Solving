using UnityEngine;

namespace CMProblemSolving.L1006_GoingUnder0AboveMax {

    public class Enemy : MonoBehaviour {

        
        private HealthSystem healthSystem;


        private void Start() {
            healthSystem = GetComponent<HealthSystem>();
        }

    }

}