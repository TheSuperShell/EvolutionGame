using UnityEngine;
using TheSuperShell.Microbes;
using TheSuperShell.Microbes.Params;
using TMPro;
using System.Collections.Generic;

namespace TheSuperShell.UI
{
    public class GenomeUIWindow : MonoBehaviour
    {
        [SerializeField] private Transform verticalSort;
        [SerializeField] private GameObject textObject;

        private List<TextMeshProUGUI> textList = new();

        private void Awake()
        {
            for (int i = 0; i < Parameters.ParameterList.Count; i++)
                textList.Add(Instantiate(textObject, verticalSort).GetComponent<TextMeshProUGUI>());
        }

        public void Show(Microbe microbe)
        {
            gameObject.SetActive(true);
            List<MutatableParameter> list = Parameters.ParameterList;
            for (int i = 0; i < list.Count; i++)
            {
                string text = list[i].Name;
                text += ":   ";
                text += list[i].AsString(microbe.Genome);
                textList[i].text = text;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
