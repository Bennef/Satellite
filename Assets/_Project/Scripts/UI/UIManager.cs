using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text _nearestPlanetText;

        public Text NearestPlanetText => _nearestPlanetText;

        public void ShowText(Text textToShow) => textToShow.enabled = true;

        public void HideText(Text textToHide) => textToHide.enabled = false;

        public void SetText(Text textToSet, string value) => textToSet.text = value;
    }
}