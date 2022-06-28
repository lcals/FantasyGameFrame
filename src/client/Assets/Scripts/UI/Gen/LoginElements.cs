using AnKuchen.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fantasy.UI.Gen
{
   public class LoginElements : IMappedObject
{
    public IMapper Mapper { get; private set; }
    public GameObject Root { get; private set; }
    public Button ForgotPasswordButton { get; private set; }
    public TextMeshProUGUI ForgotPasswordButtonButtonText { get; private set; }
    public Button LoginButton { get; private set; }
    public TextMeshProUGUI LoginButtonButtonText { get; private set; }
    public Button RegisterButton { get; private set; }
    public TextMeshProUGUI RegisterButtonButtonText { get; private set; }
    public TMP_InputField PasswordInputField { get; private set; }
    public TextMeshProUGUI PasswordInputFieldInputFieldText { get; private set; }
    public TextMeshProUGUI PasswordInputFieldInputFieldTextPlaceholder { get; private set; }
    public TMP_InputField AccountInputField { get; private set; }
    public TextMeshProUGUI AccountInputFieldInputFieldText { get; private set; }
    public TextMeshProUGUI AccountInputFieldInputFieldTextPlaceholder { get; private set; }

    public LoginElements() { }
    public LoginElements(IMapper mapper) { Initialize(mapper); }

    public void Initialize(IMapper mapper)
    {
        Mapper = mapper;
        Root = mapper.Get();
        ForgotPasswordButton = mapper.Get<Button>("ForgotPasswordButton");
        ForgotPasswordButtonButtonText = mapper.Get<TextMeshProUGUI>("ForgotPasswordButton/ButtonText");
        LoginButton = mapper.Get<Button>("LoginButton");
        LoginButtonButtonText = mapper.Get<TextMeshProUGUI>("LoginButton/ButtonText");
        RegisterButton = mapper.Get<Button>("RegisterButton");
        RegisterButtonButtonText = mapper.Get<TextMeshProUGUI>("RegisterButton/ButtonText");
        PasswordInputField = mapper.Get<TMP_InputField>("PasswordInputField");
        PasswordInputFieldInputFieldText = mapper.Get<TextMeshProUGUI>("PasswordInputField/InputFieldText");
        PasswordInputFieldInputFieldTextPlaceholder = mapper.Get<TextMeshProUGUI>("PasswordInputField/InputFieldText/Placeholder");
        AccountInputField = mapper.Get<TMP_InputField>("AccountInputField");
        AccountInputFieldInputFieldText = mapper.Get<TextMeshProUGUI>("AccountInputField/InputFieldText");
        AccountInputFieldInputFieldTextPlaceholder = mapper.Get<TextMeshProUGUI>("AccountInputField/InputFieldText/Placeholder");
    }
}

}