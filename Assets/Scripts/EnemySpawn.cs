using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject zombie;
    public float minX = -10f, maxX = 10f;
    public float minZ = -10f, maxZ = 10f;
    public float timeRepeat = 2f;


    private void Start()
    {

        InvokeRepeating("CriarZombie", 1f, timeRepeat);
    }

    void CriarZombie()
    {

        float positionX = Random.Range(minX, maxX);
        float positionZ = Random.Range(minZ, maxZ);

        Vector3 position = new Vector3(positionX,0, positionZ);

        Instantiate(zombie, position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
