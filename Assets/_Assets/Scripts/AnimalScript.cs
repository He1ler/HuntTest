using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class AnimalScript : MonoBehaviour
{
    public string name { get; private set; }
    public int id { get; private set; }
    public int attack { get; private set; }
    public Animator animator { get; private set; }
    [SerializeField] GameObject particle;
    [HideInInspector] public UnityEvent huntEvent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public AnimalScript StartAnimal(AnimalSO animalSO)
    {
        name = animalSO.name;
        id = animalSO.id;
        attack = animalSO.attack;
        return this;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if(other.gameObject.layer == 6)
        {
            huntEvent.Invoke();
            Destroy(gameObject,1f);
            return;
        }
        if (other.gameObject.layer == 10)
        {
            Instantiate(particle, new Vector3 (other.transform.position.x, other.transform.position.y + 0.5f, other.transform.position.z), Quaternion.identity, other.transform);
            huntEvent.Invoke();
            if(GameManager.instance.RecieveAttack(attack))
            {
                Destroy(other.gameObject, .25f);
            }
            Destroy(gameObject, 1f);
            return;
        }
    }
}