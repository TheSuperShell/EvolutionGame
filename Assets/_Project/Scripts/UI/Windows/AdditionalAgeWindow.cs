using TMPro;
using UnityEngine;

namespace TheSuperShell.UI
{
    public class AdditionalAgeWindow : AdditionalInfoWindow
    {
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI penaltyText;

        protected override void UpdateStats()
        {
            penaltyText.text = "+" + (currentMicrobe.AgePenalty - 1).ToString("P0");
        }

        protected override void StartStats()
        {
            float lifetime = currentMicrobe.Lifetime;
            int minutes = Mathf.FloorToInt(lifetime / 60);
            int seconds = Mathf.FloorToInt(lifetime % 60);
            ageText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
    }
}
