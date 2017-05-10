using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LoginAndRegistration : MonoBehaviour {

    #region Variables
    public Text usernameWarningText;
    public Text firstNameWarningText;
    public Text lastNameWarningText;
    public Text emailWarningText;
    public Text confirmEmailWarningText;
    public Text passwordWarningText;
    public Text confirmPasswordWarningText;

    public Text loginEmailWarningText;
    public Text loginPasswordWarningText;

    string username;
    string firstName;
    string lastName;
    string email;
    string confirmEmail;
    string password;
    string confirmPassword;
    string dropdownChoice;

    bool usernameValid;
    bool firstNameValid;
    bool lastNameValid;
    bool emailValid;
    bool confirmEmailValid;
    bool passwordValid;
    bool confirmPasswordValid;

    FirebaseAuth auth;
    FirebaseUser user;
    #endregion

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        username = "";
        firstName = "";
        lastName = "";
        email = "";
        confirmEmail = "";
        password = "";
        confirmPassword = "";

        usernameValid = false;
        firstNameValid = false;
        lastNameValid = false;
        emailValid = false;
        confirmEmailValid = false;
        passwordValid = false;
        confirmPasswordValid = false;
    }

    #region ValidateAllFields
    public void ValidateUsername(string username)
    {
        CheckIfInputIsInvalid(username, ref this.username, usernameWarningText, ref usernameValid);
    }

    public void ValidateFirstName(string firstName)
    {
        CheckIfInputIsInvalid(firstName, ref this.firstName, firstNameWarningText, ref firstNameValid);
    }

    public void ValidateLastName(string lastName)
    {
        CheckIfInputIsInvalid(lastName, ref this.lastName, lastNameWarningText, ref lastNameValid);
    }

    public void ValidateEmail(string email)
    {
        CheckIfInputIsInvalid(email, ref this.email, emailWarningText, ref emailValid);
    }

    public void ValidateConfirmEmail(string confirmEmail)
    {
        CheckIfInputIsInvalid(confirmEmail, ref this.confirmEmail, confirmEmailWarningText, ref confirmEmailValid);
        CheckIfInputIsInvalid(confirmEmail, email, confirmEmailWarningText, "Emails do not match.", ref confirmEmailValid);
    }

    public void ValidatePassword(string password)
    {
        CheckIfInputIsInvalid(password, ref this.password, passwordWarningText, ref passwordValid);
    }

    public void ValidateConfirmPassword(string confirmPassword)
    {
        CheckIfInputIsInvalid(confirmPassword, ref this.confirmPassword, confirmPasswordWarningText, ref confirmPasswordValid);
        CheckIfInputIsInvalid(confirmPassword, password, confirmPasswordWarningText, "Passwords do not match.", ref confirmPasswordValid);
    }

    public void ValidateLoginEmail(string email)
    {
        CheckIfInputIsInvalid(email, ref this.email, loginEmailWarningText, ref emailValid);
    }

    public void ValidateLoginPassword(string password)
    {
        CheckIfInputIsInvalid(password, ref this.password, loginPasswordWarningText, ref passwordValid);
    }
    #endregion

    #region Firebase functionality
    public void SubmitRegistrationForm()
    {
        if (usernameValid && firstNameValid && lastNameValid && emailValid && confirmEmailValid && passwordValid && confirmPasswordValid)
        {
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                FirebaseUser newUser = task.Result;
                newUser.UpdateUserProfileAsync(new UserProfile {
                    DisplayName = username
                });
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        }
        else
        {
            Debug.Log("One of the inputs is invalid");
        }
    }


    public void LoginFunction()
    {
        if (emailValid && passwordValid)
        {
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
            // Load the next scene
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("One of the inputs is invalid");
        }
    }

    void CheckUser()
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            string name = user.DisplayName;
            string email = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;
        }
    }
#endregion

    #region Validation Functions
    void CheckIfInputIsInvalid (string stringToCheck, ref string finalString, Text warningText, ref bool isValid)
    {
        if (string.IsNullOrEmpty(stringToCheck))
        {
            isValid = false;
            warningText.text = "You can't leave this field blank.";
            warningText.gameObject.SetActive(true);
        }
        else
        {
            isValid = true;
            finalString = stringToCheck;
            warningText.gameObject.SetActive(false);
        }
    }

    void CheckIfInputIsInvalid(string stringToCheck, string stringToMatch, Text warningText, string warningOuput, ref bool isValid)
    {
        if (stringToCheck != stringToMatch)
        {
            isValid = false;
            warningText.text = warningOuput;
            warningText.gameObject.SetActive(true);
        }
        else
        {
            isValid = true;
            warningText.gameObject.SetActive(false);
        }
    }
#endregion
}
