using UnityEngine;
using TheSuperShell.Microbes;
using TMPro;
using System;

namespace TheSuperShell.UI
{
    public sealed class ConsumptionWindow : AdditionalInfoWindow
    {
        [SerializeField] private TextMeshProUGUI forcePenalty;
        [SerializeField] private TextMeshProUGUI rotationForcePenalty;
        [SerializeField] private Transform metabolismBar;
        [SerializeField] private TextMeshProUGUI metabolismValue;
        [SerializeField] private Transform motionBar;
        [SerializeField] private TextMeshProUGUI motionValue;
        [SerializeField] private Transform growthBar;
        [SerializeField] private TextMeshProUGUI growthValue;

        protected override void UpdateStats()
        {
            float agePenalty = 1 - currentMicrobe.Health.GetShare();
            forcePenalty.text = "-" + agePenalty.ToString("P0");
            rotationForcePenalty.text = "-" + agePenalty.ToString("P0");

            metabolismValue.text = "-" + currentMicrobe.MetabolismCost.ToString("F2") + " mJ/s";

            motionBar.localScale = new Vector3(currentMicrobe.MovementWant, 1);
            motionValue.text = "-" + (currentMicrobe.MovementCost + currentMicrobe.RotationCost).ToString("F2") + " mJ/s";

            growthBar.localScale = new Vector3((currentMicrobe.GrowthCost != 0) ? currentMicrobe.GrowthWant : 0, 1);
            growthValue.text = "-" + currentMicrobe.GrowthCost.ToString("F2") + " mJ/s";
        } 
    }
}
