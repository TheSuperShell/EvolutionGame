using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TheSuperShell.UI
{
    public class MaturityUI : MonoBehaviour
    {
        [SerializeField] private Gradient barColorGradient;

        private Transform bar;
        private Image barImage;
        private TextMeshProUGUI text;

        private void Awake()
        {
            bar = transform.Find("bar");
            barImage = bar.GetComponent<Image>();
            text = transform.Find("value").GetComponent<TextMeshProUGUI>();
        }

        public void SetValue(float value)
        {
            text.text = value.ToString("F2");
            value = Mathf.Clamp01(value);
            barImage.color = barColorGradient.Evaluate(value);
            bar.localScale = new Vector3(1, value);
        }
    }
}
