using Firebase.Auth;
using UnityEngine;
using System;
using Firebase;

namespace MySampleEx
{
    /// <summary>
    /// Firebase 인증 (로그인, 계정생성)
    /// </summary>
    public class FirebaseAuthManager
    {
        #region Singleton
        private static FirebaseAuthManager instance = null;

        public static FirebaseAuthManager Instance
        {
            get
            {
                if (instance == null)      //널이면 객체 생성
                {
                    instance = new FirebaseAuthManager();
                }
                return instance;
            }
        }
        #endregion

        #region Variables
        private FirebaseAuth auth;
        private FirebaseUser user;

        public string userId => user?.UserId ?? string.Empty;

        //Auth 상태 변경시 등록된 함수 호출
        public Action<int> OnChangedAuthState;

        #endregion

        //FirebaseAuth 초기화
        public void InitializeFirebase()
        {
            auth = FirebaseAuth.DefaultInstance;
            auth.StateChanged += OnAuthStatechanged;
            OnAuthStatechanged(this, null);
        }

        //Firebase 계정 생성(email, password)
        public async void CreateUser(string email, string password)
        {
            int result = 0;

            await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("CreateUserWithEmailAndPasswordAsync was cancled");
                    result = 2;
                    return;
                }
                if(task.IsFaulted)
                {
                    int errorCode = GetFirebaseErrorCode(task.Exception);
                    result = (errorCode == (int)Firebase.Auth.AuthError.EmailAlreadyInUse) ? 1 : 2;
                    Debug.LogError($"CreateUserWithEmailAndPasswordAsync error : {task.Exception}");
                    return;
                }
                //계정 생성 성공
                Firebase.Auth.AuthResult authResult = task.Result;
                Debug.Log($"Firebase user create success : {authResult.User.DisplayName},{authResult.User.UserId}");
            });

            OnChangedAuthState?.Invoke(result); //등록된 함수에 업데이트 
        }

        //Firebase auth 로그인
        public async void SignIn(string email, string password)
        {
            int result = 0;
            await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("SignInWithEmailAndPasswordAsync was cancled");
                    result = 2;
                    return;
                }
                if (task.IsFaulted)
                {
                    int errorCode = GetFirebaseErrorCode(task.Exception);
                    switch(errorCode)
                    {
                        case (int)Firebase.Auth.AuthError.EmailAlreadyInUse:
                            Debug.LogError($"Email already Use");
                            result = 2;
                            break;
                        case (int)Firebase.Auth.AuthError.WrongPassword:
                            Debug.LogError($"WrongPassword");
                            result = 1;
                            break;
                        default:
                            result = 2;
                            break;
                    }
                    return;
                }
                //계정 생성 성공
                Firebase.Auth.AuthResult authResult = task.Result;
                Debug.Log($" user Signed in success : {authResult.User.DisplayName},{authResult.User.UserId}");
            });
            OnChangedAuthState?.Invoke(result);
        }

        //Firebase auth  로그아웃 
        public void SignOut()
        {
            auth.SignOut(); 
        }

        //Firebase auth 에러코드 가져오기
        private int GetFirebaseErrorCode(AggregateException exception)
        {
            FirebaseException firebaseException = null;
            foreach (Exception ex in exception.Flatten().InnerExceptions)
            {
                firebaseException = ex as FirebaseException;
                if(firebaseException != null)
                {
                    break;
                }
            }
            return firebaseException?.ErrorCode ?? 0;
        }

        private void OnAuthStatechanged(object sender, EventArgs eventArgs)
        {
            if(auth.CurrentUser != user)
            {
                bool signedIn = (user != auth.CurrentUser && auth.CurrentUser != null);
                if (!signedIn && user != null)
                {
                    Debug.Log($"Signed out : {user.UserId}");
                    //OnChangedAuthState?.Invoke(0);

                }
                user = auth.CurrentUser;
                if(signedIn)
                {
                    Debug.Log($"Signed in : {user.UserId}");
                    //OnChangedAuthState?.Invoke(0);
                }
            }
        }
    }
}
