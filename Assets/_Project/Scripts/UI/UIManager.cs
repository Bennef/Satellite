using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nearestPlanetLabel, _nearestPlanetText;

        public TextMeshProUGUI NearestPlanetLabel => _nearestPlanetLabel;

        public TextMeshProUGUI NearestPlanetText { get => _nearestPlanetText; set => _nearestPlanetText = value; }

        public void ShowText(TextMeshProUGUI textToShow) => textToShow.enabled = true;

        public void HideText(TextMeshProUGUI textToHide) => textToHide.enabled = false;

        public void SetText(TextMeshProUGUI textToSet, string value) => textToSet.SetText(value);
    }
}