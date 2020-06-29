using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Environment
{
    public class Galaxy : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _availablePlanetList;
        [SerializeField] private List<GameObject> _availableAsteroidList;
        [SerializeField] private List<Transform> _planetList;

        public List<Transform> PlanetList { get => _planetList; }

        private void Awake()
        {
            PopulateGalaxy();

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Planet"))
                PlanetList.Add(go.GetComponent<Transform>());
        }

        void PopulateGalaxy()
        {

        }
    }
}