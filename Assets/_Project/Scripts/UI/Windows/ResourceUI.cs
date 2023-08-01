using UnityEngine;
using TMPro;
using TheSuperShell.Microbes;
using System;

namespace TheSuperShell
{
    public class ResourceUI : MonoBehaviour
    {
        private Transform bar;
        private TextMeshProUGUI text;

        private void Awake()
        {
            bar = transform.Find("bar").transform;
            text = transform.Find("value").GetComponent<TextMeshProUGUI>();
        }

        public void UpdateBar(float value, float maxValue)
        {
            text.text = Mathf.CeilToInt(value).ToString() + "/" + Mathf.CeilToInt(maxValue).ToString();
            bar.localScale = new Vector3(value / maxValue, 1);
        }
    }
}
