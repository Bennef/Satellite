using Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Environment
{
    public class Galaxy : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _availablePlanetList;
        [SerializeField] private List<GameObject> _availableAsteroidList;
        [SerializeField] private List<Transform> _planetList;
        [SerializeField] private List<Transform> _asteroidList;
        [SerializeField] private int _asteroidCount;
        private UIManager _uIManager;

        public List<Transform> PlanetList { get => _planetList; }
        public List<Transform> AsteroidList { get => _asteroidList; }
        public int AsteroidCount { get => _asteroidCount; set => _asteroidCount = value; }      

        void Awake()
        {
            Screen.SetResolution(1920, 1080, false);
            _uIManager = FindObjectOfType<UIManager>();
            PopulateGalaxy();
            UpdatePlanetList();
            UpdateAsteroidList();
        }

        void PopulateGalaxy()
        {

        }

        void UpdatePlanetList()
        {
            foreach (GameObject planetGo in GameObject.FindGameObjectsWithTag("Planet"))
                PlanetList.Add(planetGo.GetComponent<Transform>());
        }

        public void UpdateAsteroidList()
        {
            AsteroidList.Clear();
            foreach (GameObject AsteroidGo in GameObject.FindGameObjectsWithTag("Asteroid"))
                if (AsteroidGo.GetComponent<Asteroid>().IsDestroyed == false)
                    AsteroidList.Add(AsteroidGo.GetComponent<Transform>());

            AsteroidCount = AsteroidList.Count;
            _uIManager.SetText(_uIManager.AsteroidsLeftText, AsteroidCount.ToString());
        }
    }
}