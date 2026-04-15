using UnityEngine;

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

    [Header("Efeitos Visuais")]
    [SerializeField] private ParticleSystem muzzleFlash; // Fogo na ponta da arma
    [SerializeField] private GameObject impactEffectPrefab; // Faísca/Sangue no local do impacto
    #endregion

    #region Referências
    private Camera mainCamera;
    #endregion

    void Awake()
    {
        // Pega a câmera principal para disparar o raio do centro dela
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Método chamado pelo UnityEvent do PlayerController.
    /// </summary>
    public void Shoot()
    {
        // 1. Efeito visual na arma (opcional)
        if (muzzleFlash != null) muzzleFlash.Play();

        // 2. Lógica do Raycast
        // O raio sai do centro da visão (Viewport 0.5, 0.5)
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 2f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, hitLayers))
        {
            Debug.Log("Acertou: " + hit.transform.name);

            // 3. Aplicar dano se o alvo tiver o script de saúde
            // Exemplo: hit.transform.GetComponent<ZombieHealth>()?.TakeDamage(damage);

            // 4. Criar um efeito no ponto de impacto (faísca, buraco de bala)
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}