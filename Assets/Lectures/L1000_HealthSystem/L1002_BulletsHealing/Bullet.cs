using UnityEngine;

namespace CMProblemSolving.L1002_HealthSystem {

    public class Bullet : MonoBehaviour {


        private void Awake() {
            Destroy(gameObject, 3f);
        }

        private void Update() {
            float moveSpeed = 10f;
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collider2D) {
            if (collider2D.gameObject.TryGetComponent(out Enemy enemy)) {
                enemy.GetComponent<HealthSystem>().Damage(-10);
                DestroySelf();
            }
        }

        private void DestroySelf() {
            Destroy(gameObject);
        }


    }

}