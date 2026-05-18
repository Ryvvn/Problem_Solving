using UnityEngine;
using UnityEngine.UI;

namespace CMProblemSolving.L1002_HealthSystem {

    public class HealthBar : MonoBehaviour {


        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private Image barImage;


        private void Start() {
            healthSystem.OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;
            barImage.fillAmount = healthSystem.GetHealthAmountNormalized();
        }

        private void HealthSystem_OnHealthAmountChanged(object sender, System.EventArgs e) {
            barImage.fillAmount = healthSystem.GetHealthAmountNormalized();
        }

    }

}