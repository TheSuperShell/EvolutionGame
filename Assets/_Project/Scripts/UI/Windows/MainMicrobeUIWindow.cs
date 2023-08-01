using UnityEngine;
using TheSuperShell.Microbes;
using CodeMonkey.Utils;
using System;
using TMPro;
using System.Collections.Generic;

namespace TheSuperShell.UI
{
    public sealed class MainMicrobeUIWindow : MonoBehaviour
    {
        public static Action<AdditionalInfoType> AdditionalInfoOpenAction;
        public static Action<AdditionalInfoType> AdditionalInfoCloseAction;

        [Header("Buttons")]
        [SerializeField] private Button_UI exitButton;
        [SerializeField] private Button_UI infoWindowButton;
        [SerializeField] private Button_UI genomeWindowButton;
        [SerializeField] private Button_UI followButton;
        [Header("Resource Bars")]
        [SerializeField] private ResourceUI healthBar;
        [SerializeField] private ResourceUI energyBar;
        [Header("Windows")]
        [SerializeField] private GenomeUIWindow genomeWindow;
        [SerializeField] private InfoUIWindow infoWindow;
        [SerializeField] private ConsumptionWindow consumptionWindow;
        [SerializeField] private AdditionalMaturityWindow additionalMaturityWindow;
        [SerializeField] private AdditionalAgeWindow additionalAgeWindow;
        [Header("Other")]
        [SerializeField] private CameraControl cameraControl;
        [SerializeField] private MaturityUI maturityUI;
        [SerializeField] private TextMeshProUGUI clock;
        [SerializeField] private TextMeshProUGUI generation;

        private Microbe currentMicrobe;
        private Dictionary<AdditionalInfoType, AdditionalInfoWindow> additionalWindows;

        private void OnEnable()
        {
            AdditionalInfoOpenAction = OnAdditionalInfoOpen;
            AdditionalInfoCloseAction = OnAdditionalInfoClose;
        }

        private void OnDisable()
        {
            AdditionalInfoOpenAction -= OnAdditionalInfoOpen;
            AdditionalInfoCloseAction -= OnAdditionalInfoClose;
        }

        private void OnAdditionalInfoOpen(AdditionalInfoType tooltip)
        {
            additionalWindows[tooltip].Show(currentMicrobe);
        }

        private void OnAdditionalInfoClose(AdditionalInfoType tooltip)
        {
            additionalWindows[tooltip].Hide();
        }

        private void Awake()
        {
            exitButton.ClickFunc = () => { Hide(); };
            genomeWindowButton.ClickFunc = () => {
                if (genomeWindow.gameObject.activeInHierarchy)
                    genomeWindow.Hide();
                else
                {
                    HideAllTabs();
                    genomeWindow.Show(currentMicrobe);
                }
            };
            infoWindowButton.ClickFunc = () =>
            {
                if (infoWindow.gameObject.activeInHierarchy)
                    infoWindow.Hide();
                else
                {
                    HideAllTabs();
                    infoWindow.Show(currentMicrobe);
                }
            };
            followButton.ClickFunc = () => { cameraControl.Connect(currentMicrobe.transform); };

            additionalWindows = new();
            additionalWindows.Add(AdditionalInfoType.CONSUMPTIONS, consumptionWindow);
            additionalWindows.Add(AdditionalInfoType.AGE, additionalAgeWindow);
            additionalWindows.Add(AdditionalInfoType.MATURITY, additionalMaturityWindow);

            gameObject.SetActive(false);
        }

        public void FollowMicrobe(Microbe microbe)
        {
            if (currentMicrobe == microbe)
            {
                cameraControl.Connect(currentMicrobe.transform);
                return;
            }
            if (currentMicrobe != null)
                Hide();
            generation.text = "Gen. " + microbe.Genome.generation.ToString();
            gameObject.SetActive(true);
            currentMicrobe = microbe;
            microbe.DeathEvent += OnDeath;
            microbe.Energy.ValueChangeEvent += OnEnergyChanged;
            microbe.Health.ValueChangeEvent += OnHealthChanged;
            healthBar.UpdateBar(microbe.Health.Value, microbe.Health.MaxValue);
            energyBar.UpdateBar(microbe.Energy.Value, microbe.Energy.MaxValue);
            cameraControl.Connect(microbe.transform);
            ((AdditionalMaturityWindow)additionalWindows[AdditionalInfoType.MATURITY]).DrawGrowthCurve(microbe);
        }

        private void Update()
        {
            if (currentMicrobe != null)
            {
                maturityUI.SetValue(currentMicrobe.Maturity);
                HandleTime();
            }
        }

        private void OnDeath(object sender, EventArgs e)
        {
            Hide();
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            Resource health = (Resource)sender;
            healthBar.UpdateBar(health.Value, health.MaxValue);
        }

        private void OnEnergyChanged(object sender, EventArgs e)
        {
            Resource energy = (Resource)sender;
            energyBar.UpdateBar(energy.Value, energy.MaxValue);
        }

        private void HandleTime()
        {
            float timeInSeconds = currentMicrobe.Age;
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            clock.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            HideAllTabs();
            foreach (AdditionalInfoWindow window in additionalWindows.Values)
                window.gameObject.SetActive(false);
            currentMicrobe.Energy.ValueChangeEvent -= OnEnergyChanged;
            currentMicrobe.Health.ValueChangeEvent -= OnHealthChanged;
            currentMicrobe.DeathEvent -= OnDeath;
            currentMicrobe = null;
            cameraControl.Disconnect();
        }

        private void HideAllTabs()
        {
            genomeWindow.Hide();
            infoWindow.Hide();
        }
    }
}

public enum AdditionalInfoType
{
    CONSUMPTIONS,
    MATURITY,
    AGE
}
