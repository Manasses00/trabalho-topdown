using UnityEngine;
using UnityEngine.UI;

public class Inimigo : MonoBehaviour
{
    public float velocidade = 2f;
    public float raioDeteccao = 5f;
    public int vidaMaxima = 50;
    public int vidaAtual;
    public int dano = 10;
    public int pontosAoMorrer = 10;
    public Slider barraVida;

    private Transform player;
    private bool jogadorDetectado = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        vidaAtual = vidaMaxima;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); // pega o sprite
    }

    void Update()
    {
        if (player == null) return;

        // Distância ao jogador
        float distancia = Vector2.Distance(transform.position, player.position);
        jogadorDetectado = distancia <= raioDeteccao;

        // Seguir player
        if (jogadorDetectado)
        {
            Vector2 direcao = (player.position - transform.position).normalized;

            // --- Flip do sprite ---
            if (direcao.x > 0)
                spriteRenderer.flipX = true;    // olha pra direita
            else if (direcao.x < 0)
                spriteRenderer.flipX = false;   // olha pra esquerda

            transform.position += (Vector3)direcao * velocidade * Time.deltaTime;
        }

        // Atualiza barra de vida
        if (barraVida != null)
            barraVida.value = (float)vidaAtual / vidaMaxima;
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Player p = outro.GetComponent<Player>();
            if (p != null)
                p.LevarDano(dano);
        }
    }

    public void LevarDano(int danoRecebido)
    {
        vidaAtual -= danoRecebido;

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;

            if (player != null)
            {
                Player p = player.GetComponent<Player>();
                if (p != null)
                    p.AdicionarPontuacao(pontosAoMorrer);
            }

            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao);
    }
}
