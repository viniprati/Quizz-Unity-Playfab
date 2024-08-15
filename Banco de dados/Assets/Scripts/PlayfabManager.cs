using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class PlayfabManager : MonoBehaviour
{
    // Refer�ncias para os TMP_Dropdowns
    public TMP_Dropdown escolaridadeDropdown;
    public TMP_Dropdown idadeDropdown;
    public TMP_Dropdown sexoDropdown;

    // Refer�ncia para o texto que exibe o resultado do quizz
    public Text resultadoText;

    // Vari�veis para armazenar as informa��es
    private string escolaridade;
    private string idade;
    private string sexo;
    private int resultadoQuizz;

    public int result;

    void Start()
    {

        //Inicializar os dropdowns
        escolaridadeDropdown.onValueChanged.AddListener(AtualizarEscolaridade);
        idadeDropdown.onValueChanged.AddListener(AtualizarIdade);
        sexoDropdown.onValueChanged.AddListener(AtualizarSexo);
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Andre " + Random.Range(0, 1000000),
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }

        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucess, OnError);
    }
    void OnSucess(LoginResult result)
    {
        Debug.Log("Fez Login");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        if (name == null)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = "Andre " + Random.Range(0, 1000000)
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        }
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("name done");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error: " + error.ErrorMessage);
    }
    // Fun��es para atualizar as vari�veis quando os dropdowns mudam
    void AtualizarEscolaridade(int index)
    {
        escolaridade = escolaridadeDropdown.options[index].text;
    }

    void AtualizarIdade(int index)
    {
        idade = idadeDropdown.options[index].text;
    }

    void AtualizarSexo(int index)
    {
        sexo = sexoDropdown.options[index].text;
    }

    // Fun��o para salvar as informa��es no PlayFab
    IEnumerator EsperarLogin(int res)
    {
        yield return new WaitForSeconds(3);
        SalvarInformacoes(res);
    }
    public void SalvarInformacoes(int resultado)
    {
        // Obter o resultado do quizz
        resultadoQuizz = resultado;

        // Criar um novo objeto para armazenar as informa��es
        var dados = new Dictionary<string, string>
        {
            { "escolaridade", escolaridade },
            { "idade", idade },
            { "sexo", sexo },
            { "resultadoQuizz", resultadoQuizz.ToString() }
        };

        // Salvar as informa��es no PlayFab
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = dados

        }, (result) =>
        {
            Debug.Log("Informa��es salvas com sucesso!");
        }, (error) =>
        {
            Debug.LogError("Erro ao salvar informa��es: " + error.ErrorMessage);
        });
    }
}