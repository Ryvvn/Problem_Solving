using UnityEngine;
using UnityEngine.InputSystem;

namespace CMProblemSolving.L1005_HealthSystem {

    public class NoDamage : MonoBehaviour {


        [SerializeField] private Transform bulletPrefab;
        [SerializeField] private Transform shootPoint;


        private void Update() {
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                // Shoot bullet
                Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            }
        }


    }

}