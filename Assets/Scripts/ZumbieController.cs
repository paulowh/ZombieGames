using UnityEngine;
using UnityEngine.AI;

public class ZumbieControleler : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    Transform visual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        visual = transform.GetChild(Random.Range(1, transform.childCount));
        visual.gameObject.SetActive(true);

        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("Andar", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Andar()
    {
        agent.destination = player.position;
    }
    


}
