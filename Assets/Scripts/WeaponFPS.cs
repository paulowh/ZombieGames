using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngineInternal;

/// <summary>
/// Gerencia a lógica de disparo usando Raycast a partir do centro da câmera.
/// </summary>
public class WeaponFPS : MonoBehaviour
{
    #region Configurações
    [Header("Configurações de Tiro")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private LayerMask hitLayers; // Define o que pode ser atingido (Zumbis, Paredes)
    [SerializeField] private GameObject bulletPrefab; // Objeto da Bala
    [SerializeField] private Transform firePoint; // Ponta da Arma


    [Header("Efeitos Visuais")]
    [SerializeField] private ParticleSystem muzzleFlash; // Fogo na ponta da arma
    [SerializeField] private GameObject impactEffectPrefab; // Faísca/Sangue no local do impacto
    
    [Header("Efeitos Sonoros")]
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;


    #endregion

    #region Referências
    private Camera mainCamera;
    #endregion

    void Awake()
    {
        // Pega a câmera principal para disparar o raio do centro dela
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Método chamado pelo UnityEvent do PlayerController.
    /// </summary>
    private int box = 0;
    public void Shoot()
    {
        // 1. Efeito visual na arma (opcional)
        if (muzzleFlash != null) muzzleFlash.Play();

        if (audioSource != null && audioClip != null) audioSource.PlayOneShot(audioClip);   

        // 2. Efeito visual/tiro da bala
        if (bulletPrefab != null)
        {

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            //Rigidbody rb = bullet.GetComponent<Rigidbody>();
            //rb.AddForce(firePoint.forward * 20f, ForceMode.Impulse);

        }

        // 3. Lógica do Raycast
        // O raio sai do centro da visão (Viewport 0.5, 0.5)
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 2f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, hitLayers))
        {
            if (hit.transform.tag == "Inimigo")
            {
                Destroy(hit.collider.gameObject);
                box++;
            }

            Debug.Log("Acertou: " + hit.transform.name);

            // 4. Aplicar dano se o alvo tiver o script de saúde
            // Exemplo: hit.transform.GetComponent<ZombieHealth>()?.TakeDamage(damage);

            // 5. Criar um efeito no ponto de impacto (faísca, buraco de bala)
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }


    }

}