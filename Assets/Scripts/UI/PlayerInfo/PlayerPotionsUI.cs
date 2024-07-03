using GlobalVariables;
using UnityEngine;

public class PlayerPotionsUI : MonoBehaviour
{
    [SerializeField] private IntReference _potionsAvailable;
    [SerializeField] private IntReference _maxPotionsAmount;
    [SerializeField] private float _potionImageUIWidth;

    [Space(10)]
    [SerializeField] private RectTransform _foregroundImage;
    [SerializeField] private RectTransform _backgroundImage;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Vector2 size = new Vector2(_potionImageUIWidth * _maxPotionsAmount, _potionImageUIWidth);
        
        _rectTransform.sizeDelta = size;
        _foregroundImage.sizeDelta = size;
        _backgroundImage.sizeDelta = size;
    }

    private void OnEnable()
    {
        _potionsAvailable.AddListener(UpdatePotions);
    }
    
    private void OnDisable()
    {
        _potionsAvailable.RemoveListener(UpdatePotions);
    }
    
    private void UpdatePotions(int amount)
    {
        _foregroundImage.sizeDelta = new Vector2(_potionImageUIWidth * amount, _potionImageUIWidth);
    }
}
