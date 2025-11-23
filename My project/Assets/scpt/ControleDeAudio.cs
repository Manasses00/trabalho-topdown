using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControleDeAudio : MonoBehaviour
{
    public static ControleDeAudio instance;

    public AudioMixer audioMixer;
    public TMP_Text texto;
    public Slider slider;

    float volumePorcentagem; // 0 a 100

    void Awake()
    {
        // Impede duplicatas
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        float volumeDB;
        audioMixer.GetFloat("Master", out volumeDB);

        volumePorcentagem = DBToPercent(volumeDB);
        slider.value = volumePorcentagem;

        AtualizarTexto();
    }

    void Update()
    {
        volumePorcentagem = slider.value;

        float volumeDB = PercentToDB(volumePorcentagem);
        audioMixer.SetFloat("Master", volumeDB);

        AtualizarTexto();
    }

    void AtualizarTexto()
    {
        texto.text = Mathf.RoundToInt(volumePorcentagem) + "%";
    }

    // ----------- CONVERSÕES -----------
    float PercentToDB(float pct)
    {
        if (pct <= 0.01f) return -80f;
        return Mathf.Lerp(-20f, 0f, pct / 100f);
    }

    float DBToPercent(float db)
    {
        if (db <= -80f) return 0;
        return Mathf.InverseLerp(-20f, 0f, db) * 100f;
    }
}
