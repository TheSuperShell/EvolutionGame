using UnityEngine.EventSystems;
using UnityEngine;

namespace TheSuperShell.UI
{
    public class AdditionalInfoWindowHover: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AdditionalInfoType type;

        public void OnPointerEnter(PointerEventData eventData)
        {
            MainMicrobeUIWindow.AdditionalInfoOpenAction.Invoke(type);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MainMicrobeUIWindow.AdditionalInfoCloseAction.Invoke(type);
        }
    }
}
