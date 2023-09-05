using UnityEngine;
public class DragAndSnapObject : MonoBehaviour
{
    public string name { get; private set; }
    public int id { get; private set; }
    [SerializeField] private Vector2 gridSize = new Vector2(2f, 2f);
    [SerializeField] private Vector2 gapSize = new Vector2(0.5f, 0.5f);
    private DragAndSnapObject dragAndSnapObject = null;
    private AnimalPlacesScript animalPlacesScript = null;
    private AnimalPlacesScript nextAnimalPlacesScript = null;
    private bool isDragging = false;
    private Vector3 offset;
    public void EnableDragAndSnapObject (AnimalSO animalSO, AnimalPlacesScript animalPlacesScript)
    {
        this.name = animalSO.name;
        id = animalSO.id;
        this.animalPlacesScript = animalPlacesScript;
    }
    private void OnMouseDown()
    {
        Debug.Log("Down");
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }
    private void OnMouseUp()
    {
        MergeWith();
        SetToGridElement();
        isDragging = false;
    }
    private void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPos() + offset;
            newPosition.y = transform.position.y; // Lock the vertical position
            transform.position = newPosition;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer != 7 || !isDragging)
        {
            return;
        }
        dragAndSnapObject = collision.gameObject.GetComponent<DragAndSnapObject>();
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!isDragging)
        {
            return;
        }
        dragAndSnapObject = null;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 8 || !isDragging)
        {
            return;
        }
        nextAnimalPlacesScript = other.GetComponent<AnimalPlacesScript>();
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void MergeWith()
    {
        if(dragAndSnapObject == null)
        {
            return;
        }
        if(dragAndSnapObject.id == id && id <= GameManager.instance.maxAnimalLvl)
        {
            GameManager.instance.MergeAnimals(animalPlacesScript, dragAndSnapObject.animalPlacesScript, id);
            Destroy(dragAndSnapObject.gameObject, .1f);
            Destroy(gameObject, .1f);
            dragAndSnapObject.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    private void SetToGridElement()
    {
        if (dragAndSnapObject != null || nextAnimalPlacesScript == null)
        {
            dragAndSnapObject = null;
            transform.position = animalPlacesScript.animalPoint.position;
            return;
        }
        GameManager.instance.ChangeAnimalPlace(animalPlacesScript, nextAnimalPlacesScript, id);
        animalPlacesScript = nextAnimalPlacesScript;
        transform.position = animalPlacesScript.animalPoint.position;
        nextAnimalPlacesScript = null;
    }
}