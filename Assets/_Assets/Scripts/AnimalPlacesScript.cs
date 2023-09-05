using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class AnimalPlacesScript : MonoBehaviour
{
    public Transform animalPoint;
    [SerializeField] Color standartColor;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] int id;
    [SerializeField] int price = 200;
    [SerializeField] GameObject canvas;
    [SerializeField] Collider collider;
    [SerializeField] bool isBought = false;
    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI.text = price.ToString();
        OpenPlace();
    }
    public void BuyPlace()
    {
        if(isBought)
        {
            return;
        }
        if(UIManager.instance.BuyStuff(price))
        {
            isBought = true;
            OpenPlace();
        }
    }
    public void OpenPlace()
    {
        if (isBought)
        {
            GetComponent<EventTrigger>().enabled = false;
            canvas.SetActive(false);
            collider.isTrigger = true;
            spriteRenderer.color = standartColor;
            GameManager.instance.OpenPlace(this);
        }
    }
    public int GetID()
    {
        return id;
    }
}