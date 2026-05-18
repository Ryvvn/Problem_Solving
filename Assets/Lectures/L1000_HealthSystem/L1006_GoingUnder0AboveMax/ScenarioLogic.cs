using UnityEngine;
using UnityEngine.InputSystem;

namespace CMProblemSolving.L1006_GoingUnder0AboveMax {

    public class ScenarioLogic : MonoBehaviour {


        [SerializeField] private Transform bulletPrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private HealthSystem healthSystem;


        private void Update() {
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                // Shoot bullet
                Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            }
            if (Keyboard.current.fKey.wasPressedThisFrame) {
                healthSystem.Heal(30);
            }
        }


    }

}