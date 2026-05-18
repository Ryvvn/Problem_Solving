using UnityEngine;
using UnityEngine.InputSystem;

namespace CMProblemSolving.L1003_HealthSystem {

    public class HealthBarNotUpdating : MonoBehaviour {


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