using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    private static CameraFollow instance; // Instância estática para o Singleton

    void Awake()
    {
        // Singleton pattern para garantir que apenas uma instância da câmera exista
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantém a câmera ao carregar novas cenas
        }
        else
        {
            Destroy(gameObject); // Destrói instâncias duplicadas
            return;
        }
    }

    void Start()
    {
        // Encontra o jogador no início da cena (se já existir)
        FindPlayer();
    }

    void Update()
    {
        // Garante que o jogador seja encontrado se não estiver definido
        if (target == null)
        {
            FindPlayer();
            if(target == null) return; // se apos procurar o player ainda nao encontrar, não executa o resto do update.
        }

        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

    // Função para encontrar o jogador na cena
    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Encontra o jogador pela tag
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("TECNOLOGIA");
        }
    }
}