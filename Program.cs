using Azure;
using System;
using System.Globalization;
using Azure.AI.TextAnalytics;
using System.Collections.Generic;

namespace TextAnalyticsAzure
{
    public class Program
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("7b1c5d9433654febb54b66ba78a5b452");
        private static readonly Uri endpoint = new Uri("https://textanalyticsnightmare.cognitiveservices.azure.com/");
        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            // Estas funciones se implementarán después.
         /*   SentimentAnalysisExample(client);
            Console.Write("Presiona una tecla para pasar a la Mineria de Opiniones.");
            Console.ReadKey();
            SentimentAnalysisWithOpinionMiningExample(client);
            Console.Write("Presiona una tecla para pasar a la Detección de Lenguaje.");
            Console.ReadKey();
            LanguageDetectionExample(client);
            Console.Write("Presiona una tecla para pasar al Reconocimiento de Entidades.");
            Console.ReadKey();
            EntityRecognitionExample(client);
            Console.Write("Presiona una tecla para pasar al Relacionamiento de Entidades.");
            Console.ReadKey();
            EntityLinkingExample(client);
            Console.Write("Presiona una tecla para pasar al Reconocimiento de Información Personal.");
            Console.ReadKey();
            RecognizePIIExample(client);
            Console.Write("Presiona una tecla para pasar al Reconociemiento de Palabras Clave.");
            Console.ReadKey();
            KeyPhraseExtractionExample(client);

            Console.Write("Presiona una tecla para salir.");
            Console.ReadKey();*/

        }




    }

}
