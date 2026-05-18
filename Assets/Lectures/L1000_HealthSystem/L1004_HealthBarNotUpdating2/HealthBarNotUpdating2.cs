using UnityEngine;
using UnityEngine.InputSystem;

namespace CMProblemSolving.L1004_HealthSystem {

    public class HealthBarNotUpdating2 : MonoBehaviour {


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