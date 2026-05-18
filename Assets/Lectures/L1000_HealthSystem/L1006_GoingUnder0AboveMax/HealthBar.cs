using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CMProblemSolving.L1006_GoingUnder0AboveMax {

    public class HealthBar : MonoBehaviour {


        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private Image barImage;
        [SerializeField] private TextMeshPro textMesh;


        private void Start() {
            healthSystem.OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;

            barImage.fillAmount = healthSystem.GetHealthAmountNormalized();
            textMesh.text = healthSystem.GetHealthAmount().ToString();
        }

        private void HealthSystem_OnHealthAmountChanged(object sender, System.EventArgs e) {
            barImage.fillAmount = healthSystem.GetHealthAmountNormalized();
            textMesh.text = healthSystem.GetHealthAmount().ToString();
        }

    }

}