using PlayerController.Abilities;
using UnityEngine;

namespace SceneMechanics.SaveStatue
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AbilitySymbol : MonoBehaviour
    {
        [field: Header("Settings")]
        [field: SerializeField] public AbilityType AbilityType { get; private set; }
        
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            TurnOff();
        }

        public void TurnOn()
        {
            Color spriteColor = _spriteRenderer.color;
            spriteColor.a = 1;
            _spriteRenderer.color = spriteColor;
        }

        public void TurnOff()
        {
            Color spriteColor = _spriteRenderer.color;
            spriteColor.a = 0;
            _spriteRenderer.color = spriteColor;
        }
    }
}