using Cysharp.Threading.Tasks;
using Fantasy.Logic.Achieve;
using Fantasy.UI.Gen;
using TMPro;

namespace Fantasy.UI
{
    public class UILogin : AFantasyBaseUI
    {
        private void Awake()
        {
            UILoginFlow().Forget();
        }

        private async UniTask UILoginFlow()
        {
            var loginUiElements = new LoginElements(GameRootUICache)
            {
                PasswordInputField =
                {
                    inputType = TMP_InputField.InputType.Password
                }
            };
            loginUiElements.LoginButton.AddListener(() =>
            {
                
            });
            loginUiElements.RegisterButton.AddListener(() =>
            {
            });
            loginUiElements.ForgotPasswordButton.AddListener(() =>
            {
                
            });
        }
    }
}