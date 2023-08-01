using TheSuperShell.Microbes;
using TMPro;
using UnityEngine;

namespace TheSuperShell.UI
{
    public class InfoUIWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sizeText;
        [SerializeField] private TextMeshProUGUI massText;
        [SerializeField] private TextMeshProUGUI speedText;

        private Microbe currentMicrobe;

        public void Show(Microbe microbe)
        {
            gameObject.SetActive(true);
            currentMicrobe = microbe;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            currentMicrobe = null;
        }

        private void Update()
        {
            if (currentMicrobe != null)
            {
                sizeText.text = currentMicrobe.Size.ToString("F2") + " u^2";
                massText.text = currentMicrobe.Mass.ToString("F2") + " g";
                speedText.text = currentMicrobe.Speed.ToString("F2") + " u/s";
            }
        }
    }
}
