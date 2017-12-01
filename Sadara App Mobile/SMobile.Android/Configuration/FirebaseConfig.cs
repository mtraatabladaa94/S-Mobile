using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase;

namespace SMobile.Android.Configuration
{
    internal sealed class FirebaseConfig
    {
        /// <summary>
        /// Utiliza este número para identificar tu aplicación
        /// cuando te pongas en contacto con el servicio
        /// de asistencia de Firebase.
        /// </summary>
        public const string FIREBASE_APP_KEY = "1:603040444031:android:fbe2e50d50062b1e";

        /// <summary>
        /// Key del API para comunicación con la plataforma
        /// </summary>
        public const string FIREBASE_API_KEY = "AIzaSyBFdEMwXBf4zzFxRYD0S24tPrjFs-ASKUg";

        /// <summary>
        /// Instancia de FirebaseApp
        /// </summary>
        private static Firebase.FirebaseApp app;

        /// <summary>
        /// Propiedad Instancia de FirebaseApp
        /// </summary>
        public static FirebaseApp App { get => app; set => app = value; }
        

        /// <summary>
        /// URL de los nodos en Firebase
        /// </summary>
        public const string FIREBASE_URL = "https://sadara-app.firebaseio.com/";

        /// <summary>
        /// Valor de 1MB
        /// </summary>
        public const long ONE_MEGABYTE = 1024 * 1024;

        /// <summary>
        /// URL de servicio LUIS --Proyecto Bot
        /// </summary>
        public const string LUIS_URL = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/ec76c67f-ebd2-4836-b3bf-46a652083389?subscription-key=f50e7bb3b47b493a9744f26b20657b90&verbose=true&timezoneOffset=0&q=";

        /// <summary>
        /// URL Reference Firebase Storage
        /// </summary>
        public const string FIREBASE_STORAGE_URL = "gs://sadara-app.appspot.com/";

        public const string PROFILE_IMAGES_STORAGE_URL = "users/profiles/images/";

        /// <summary>
        /// Opciones de firebase preconfiguradas
        /// </summary>
        private static Firebase.FirebaseOptions firebaseOptions = 
            new Firebase.FirebaseOptions
                .Builder()
                .SetApplicationId(FirebaseConfig.FIREBASE_APP_KEY)
                .SetApiKey(FirebaseConfig.FIREBASE_API_KEY)
                .Build();

        /// <summary>
        /// Opciones de firebase preconfiguradas
        /// </summary>
        public static Firebase.FirebaseOptions FirebaseOptions {

            get {

                return FirebaseConfig.firebaseOptions;

            }

        }

        /// <summary>
        /// Usuario autenticado
        /// </summary>
        private static Firebase.Auth.FirebaseAuth auth;
        

        /// <summary>
        /// Permite obtener o establecer el Usuario Actual
        /// </summary>
        public static Firebase.Auth.FirebaseAuth Auth { get => auth; set => auth = value; }
        
    }

}