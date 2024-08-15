using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizzManager : MonoBehaviour
{
    public TMP_Text textoTitulo;
    public Image imagemQuizz;
    public TMP_Text textoPergunta;
    public TMP_Text[] textoAlternativas;

    public int perguntaAtual;
    public string[] titulos;
    public string[] perguntas;
    public string[] alternativas1;
    public string[] alternativas2;
    public string[] alternativas3;
    public string[] alternativas4;
    public Sprite[] imagens;
    public int[] alternativaCerta;

    public int acertos;

    public PlayfabManager playfab;
    public GameObject gameOver;
    public TMP_Text resultadoFinal;

    // Start is called before the first frame update
    void Start()
    {
        ProximaPergunta(0);
    }

    public void ProximaPergunta(int alternativa)
    {
        if (alternativa == 0)
        {
            textoTitulo.text = titulos[0];
            imagemQuizz.sprite = imagens[0];
            textoPergunta.text = perguntas[0];
            textoAlternativas[0].text = alternativas1[0];
            textoAlternativas[1].text = alternativas2[0];
            textoAlternativas[2].text = alternativas3[0];
            textoAlternativas[3].text = alternativas4[0];
        }
        else
        {
            if (alternativa == alternativaCerta[perguntaAtual])
            {
                acertos++;
            }

            perguntaAtual++;

            if (perguntaAtual >= perguntas.Length)
            {
                GameOver();
                resultadoFinal.text = "Você acertou" + acertos + "/10";
            }
            else
            {
                textoTitulo.text = titulos[perguntaAtual];
                imagemQuizz.sprite = imagens[perguntaAtual];
                textoPergunta.text = perguntas[perguntaAtual];
                textoAlternativas[0].text = alternativas1[perguntaAtual];
                textoAlternativas[1].text = alternativas2[perguntaAtual];
                textoAlternativas[2].text = alternativas3[perguntaAtual];
                textoAlternativas[3].text = alternativas4[perguntaAtual];
            }

        }
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        playfab.Login();

    }

    public void SalvarDados()
    {
        StartCoroutine(ChamarPlayfab(acertos));
    }

    IEnumerator ChamarPlayfab(int act)
    {
        yield return new WaitForSeconds(1);
        playfab.SalvarInformacoes(act);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

}
