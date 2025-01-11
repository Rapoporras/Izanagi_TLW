using System.Collections;
using System.Collections.Generic;
using CustomAttributes;
using SceneMechanics.Stalactite;
using UnityEngine;
using Utils;

namespace Bosses
{
    public class SeiryuStalactitesManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, MinMax(0,5)] private Vector2 _stalatitesTimeRange;
        
        [Space(10), SerializeField] private List<Stalactite> _stalactitesList;

        private List<Stalactite> _firstSublist;
        private List<Stalactite> _secondSublist;

        private bool _firstActivation;
        private Coroutine _stalactitesCoroutine;

        private void Start()
        {
            _firstActivation = true;
            AssignSublists();
        }

        private void AssignSublists()
        {
            _firstSublist = new List<Stalactite>();
            _secondSublist = new List<Stalactite>();
            
            var shuffledList = new List<Stalactite>(_stalactitesList);
            shuffledList.Shuffle();

            int middle = shuffledList.Count / 2;

            _firstSublist = shuffledList.GetRange(0, middle);
            _secondSublist = shuffledList.GetRange(middle, shuffledList.Count - middle);
        }

        public void ActivateStalactites()
        {
            if (_stalactitesCoroutine != null)
                StopCoroutine(_stalactitesCoroutine);
            
            if (_firstActivation)
            {
                _stalactitesCoroutine = StartCoroutine(_ActivateStalactites(_firstSublist));
                _firstActivation = false;
            }
            else
            {
                _stalactitesCoroutine = StartCoroutine(_ActivateStalactites(_secondSublist));
            }
        }

        private IEnumerator _ActivateStalactites(List<Stalactite> stalactites)
        {
            foreach (var stalactite in stalactites)
            {
                if (!stalactite) continue;
                
                stalactite.Activate();
                float time = Random.Range(_stalatitesTimeRange.x, _stalatitesTimeRange.y);
                yield return new WaitForSeconds(time);
            }
            
            // stalactites.Clear();
        }
    }
}