using UnityEngine;
using TheSuperShell.Microbes;

namespace TheSuperShell.UI
{
    public abstract class AdditionalInfoWindow : MonoBehaviour
    {
        protected Microbe currentMicrobe;

        private void FixedUpdate()
        {
            if (currentMicrobe != null)
            {
                UpdateStats();
            }
        }

        public void Show(Microbe microbe)
        {
            gameObject.SetActive(true);
            currentMicrobe = microbe;
            StartStats();
            LeanTween.moveLocalY(gameObject, -265, 0.1f);
        }

        public void Hide()
        {
            currentMicrobe = null;
            LeanTween.moveLocalY(gameObject, -690, 0.1f).setOnComplete(() => gameObject.SetActive(false));
        }

        protected abstract void UpdateStats();
        protected virtual void StartStats() { }
    }
}
