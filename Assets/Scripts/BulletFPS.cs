using UnityEngine;


public class BulletFPS : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float Speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(this.transform.forward * Speed, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Inimigo")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
