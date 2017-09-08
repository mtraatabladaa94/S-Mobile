﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static Firebase.FirebaseApp app;

        /// <summary>
        /// URL de los nodos en Firebase
        /// </summary>
        public const string FIREBASE_URL = "https://sadara-app.firebaseio.com/";

        /// <summary>
        /// Opciones de firebase preconfiguradas
        /// </summary>
        private static Firebase.FirebaseOptions _firebaseOptions = 
            new Firebase.FirebaseOptions
                .Builder()
                .SetApplicationId(FirebaseConfig.FIREBASE_APP_KEY)
                .SetApiKey(FirebaseConfig.FIREBASE_API_KEY)
                .Build();

        /// <summary>
        /// Opciones de firebase preconfiguradas
        /// </summary>
        public static Firebase.FirebaseOptions firebaseOptions {

            get {

                return FirebaseConfig._firebaseOptions;

            }

        }

    }
}