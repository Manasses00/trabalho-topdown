using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float velocidade = 5f;
    public int vidaMaxima = 100;
    public int vidaAtual;
    public int pontuacao;

    public Slider barraVidaUI;
    public TMPro.TextMeshProUGUI textoPontuacao;

    private Rigidbody2D rb;
    private Vector2 direcao;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        vidaAtual = vidaMaxima;
        AtualizarHUD();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        direcao = new Vector2(moveX, moveY).normalized;

        // --- Flip corrigido (se seu sprite está ao contrário) ---
        if (moveX > 0)
            spriteRenderer.flipX = true;     // olha para a direita
        else if (moveX < 0)
            spriteRenderer.flipX = false;    // olha para a esquerda

        AtualizarHUD();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    public void LevarDano(int dano)
    {
        vidaAtual -= dano;
        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            AtualizarHUD();
            Morrer();
            return;
        }

        AtualizarHUD();
    }

    void Morrer()
    {
        Destroy(gameObject);
    }

    public void AdicionarPontuacao(int pontos)
    {
        pontuacao += pontos;
        AtualizarHUD();
    }

    void AtualizarHUD()
    {
        if (barraVidaUI != null)
            barraVidaUI.value = (float)vidaAtual / vidaMaxima;

        if (textoPontuacao != null)
            textoPontuacao.text = $"Pontos: {pontuacao}";
    }
}
