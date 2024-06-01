using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public GameObject ship1;
    public GameObject ship2;
    public GameObject ship3;
    public GameObject ship4;


    private const int count_ship_1 = 4;
    private const int count_ship_2 = 3;
    private const int count_ship_3 = 2;
    private const int count_ship_4 = 1;


    void Start()
    {
        CreateShips(ship1, count_ship_1, 1);
        CreateShips(ship2, count_ship_2, 2);
        CreateShips(ship3, count_ship_3, 3);
        CreateShips(ship4, count_ship_4, 4);
    }

    public void CreateShips(GameObject prefab, int numberOfCopies, int health, float spacingMultiplier = 1.15f)
    {
        Renderer renderer = prefab.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Prefab does not have a Renderer component.");
            return;
        }
        float prefabWidth = renderer.bounds.size.x;

        float horizontalSpacing = prefabWidth * spacingMultiplier;

        for (int i = 0; i < numberOfCopies; i++)
        {
            Vector3 newPosition = prefab.transform.position + new Vector3(i * horizontalSpacing, 0f, 0f);
            GameObject newPrefabInstance = Instantiate(prefab, newPosition, Quaternion.Euler(0f, 0f, 90f));

            Ships ship = new Ships(health);

            Draggable draggable = newPrefabInstance.GetComponent<Draggable>();
            if (draggable != null)
            {
                draggable.ship = ship;
            }
        }
    }
}
