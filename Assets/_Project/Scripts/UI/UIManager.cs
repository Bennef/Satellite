using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nearestPlanetLabel, _nearestPlanetText;
        [SerializeField] private TextMeshProUGUI _faceTrajectoryLabel, _faceTrajectoryText;
        [SerializeField] private TextMeshProUGUI _velocityLabel, _velocityText;
        [SerializeField] private TextMeshProUGUI _healthLabel, _healthText;

        public TextMeshProUGUI NearestPlanetLabel => _nearestPlanetLabel;
        public TextMeshProUGUI NearestPlanetText { get => _nearestPlanetText; set => _nearestPlanetText = value; }
        public TextMeshProUGUI FaceTrajectoryLabel { get => _faceTrajectoryLabel; }        
        public TextMeshProUGUI FaceTrajectoryText { get => _faceTrajectoryText; set => _faceTrajectoryText = value; }
        public TextMeshProUGUI VelocityLabel { get => _velocityLabel; }
        public TextMeshProUGUI VelocityText { get => _velocityText; set => _velocityText = value; }
        public TextMeshProUGUI HealthLabel { get => _healthLabel; }
        public TextMeshProUGUI HealthText { get => _healthText; set => _healthText = value; }

        public void ShowText(TextMeshProUGUI textToShow) => textToShow.enabled = true;

        public void HideText(TextMeshProUGUI textToHide) => textToHide.enabled = false;

        public void SetText(TextMeshProUGUI textToSet, string value) => textToSet.SetText(value);

        public void SetColour(TextMeshProUGUI textToSet, Color colorToApply) => textToSet.color = colorToApply;
    }
}