using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    private static CameraFollow instance; // Instância estática para o Singleton
    private float timeSinceTargetLost = 0f;
    [SerializeField] private float retryFindPlayerDelay = 0.5f; // Tempo para tentar encontrar o jogador novamente

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        // Se o jogador morreu (perdemos o alvo), comece a contar o tempo
        if (target == null)
        {
            timeSinceTargetLost += Time.deltaTime;
            // Tenta encontrar o jogador novamente após um pequeno atraso
            if (timeSinceTargetLost >= retryFindPlayerDelay)
            {
                FindPlayer();
                timeSinceTargetLost = 0f; // Resetar o tempo
            }
        }
        else
        {
            timeSinceTargetLost = 0f; // Resetar o tempo se o alvo for encontrado
            Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
        }
    }

    // Função para encontrar o jogador na cena
    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            Debug.Log("Câmera encontrou/reencontrou o jogador.");
        }
        else
        {
            Debug.LogWarning("Jogador não encontrado com a tag 'Player'. Tentando novamente...");
        }
    }

    // Função pública que pode ser chamada quando o jogador morrer (opcional para feedback imediato)
    public void OnPlayerDeath()
    {
        target = null;
        timeSinceTargetLost = 0f; // Resetar o tempo ao informar a morte
        Debug.Log("Jogador morreu, câmera parou de seguir.");
    }
}