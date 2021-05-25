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
            SentimentAnalysisExample(client);
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
            Console.ReadKey();

        }

        static void SentimentAnalysisExample(TextAnalyticsClient client)
        {
            string inputText = "Tuve el peor día de mi vida";
            DocumentSentiment documentSentiment = client.AnalyzeSentiment(inputText);
            Console.WriteLine($"Document sentiment: {documentSentiment.Sentiment}\n");

            foreach (var sentence in documentSentiment.Sentences)
            {
                Console.WriteLine($"\tTexto: \"{sentence.Text}\"");
                Console.WriteLine($"\tSentimiento de la oración: {sentence.Sentiment}");
                Console.WriteLine($"\tPuntaje Positivo: {sentence.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tPuntaje Negativo: {sentence.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tPuntaje Neutral: {sentence.ConfidenceScores.Neutral:0.00}\n");
            }
        }

        static void SentimentAnalysisWithOpinionMiningExample(TextAnalyticsClient client)
        {
            var documents = new List<string>
        {
        "The food and service were unacceptable, but the concierge were nice."
        };

            AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(documents, options: new AnalyzeSentimentOptions()
            {
                IncludeOpinionMining = true
            });

            foreach (AnalyzeSentimentResult review in reviews)
            {
                Console.WriteLine($"Sentimiento del Documento: {review.DocumentSentiment.Sentiment}\n");
                Console.WriteLine($"\tPuntaje Positivo: {review.DocumentSentiment.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tPuntaje Negativo: {review.DocumentSentiment.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tPuntaje Neutro: {review.DocumentSentiment.ConfidenceScores.Neutral:0.00}\n");
                foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
                {
                    Console.WriteLine($"\tTexto: \"{sentence.Text}\"");
                    Console.WriteLine($"\tSentimiento de la Oración: {sentence.Sentiment}");
                    Console.WriteLine($"\tPuntaje Positivo de la Oración: {sentence.ConfidenceScores.Positive:0.00}");
                    Console.WriteLine($"\tPuntaje Negativo de la Oración: {sentence.ConfidenceScores.Negative:0.00}");
                    Console.WriteLine($"\tPuntaje Neutro de la Oración: {sentence.ConfidenceScores.Neutral:0.00}\n");

                    foreach (SentenceOpinion sentenceOpinion in sentence.Opinions)
                    {
                        Console.WriteLine($"\tObjetivo: {sentenceOpinion.Target.Text}, Valor: {sentenceOpinion.Target.Sentiment}");
                        Console.WriteLine($"\tPuntaje Positivo de la Oración del Objetivo: {sentenceOpinion.Target.ConfidenceScores.Positive:0.00}");
                        Console.WriteLine($"\tPuntaje Negativo de la Oración del Objetivo: {sentenceOpinion.Target.ConfidenceScores.Negative:0.00}");
                        foreach (AssessmentSentiment assessment in sentenceOpinion.Assessments)
                        {
                            Console.WriteLine($"\t\tEvaluación: {assessment.Text}, Valor: {assessment.Sentiment}");
                            Console.WriteLine($"\t\tPuntaje Positivo de la Evaluación: {assessment.ConfidenceScores.Positive:0.00}");
                            Console.WriteLine($"\t\tPuntaje Negativo de la Evaluación: {assessment.ConfidenceScores.Negative:0.00}");
                        }
                    }
                }
                Console.WriteLine($"\n");
            }
        }

        static void LanguageDetectionExample(TextAnalyticsClient client)
        {
            string texto = "Ce document est rédigé en Français.";
            DetectedLanguage detectedLanguage = client.DetectLanguage(texto);
            Console.WriteLine("La oración: " + texto);
            Console.WriteLine("Está redactada en: ");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        }

        static void EntityRecognitionExample(TextAnalyticsClient client)
        {
            string texto = "I had a wonderful trip to Seattle last week.";
            var response = client.RecognizeEntities(texto);
            Console.WriteLine("Texto: " + texto);
            Console.WriteLine("Entidades nombradas: ");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tTexto: {entity.Text},\tCategoria: {entity.Category},\tSub-Categoria: {entity.SubCategory}");
                Console.WriteLine($"\t\tPuntaje: {entity.ConfidenceScore:F2},\tLongitud: {entity.Length},\tPosición: {entity.Offset}\n");
            }
        }

        static void EntityLinkingExample(TextAnalyticsClient client)
        {
            string texto = "Microsoft was founded by Bill Gates and Paul Allen on April 4, 1975, " +
                "to develop and sell BASIC interpreters for the Altair 8800. " +
                "During his career at Microsoft, Gates held the positions of chairman, " +
                "chief executive officer, president and chief software architect, " +
                "while also being the largest individual shareholder until May 2014.";
            var response = client.RecognizeLinkedEntities(texto);
            Console.WriteLine("Entidades Relacionadas:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tNombre: {entity.Name},\tID: {entity.DataSourceEntityId},\tURL: {entity.Url}\tFuente: {entity.DataSource}");
                Console.WriteLine("\tCoincidencias:");
                foreach (var match in entity.Matches)
                {
                    Console.WriteLine($"\t\tTexto: {match.Text}");
                    Console.WriteLine($"\t\tPuntaje de confianza: {match.ConfidenceScore:F2}");
                    Console.WriteLine($"\t\tLongitud: {match.Length}");
                    Console.WriteLine($"\t\tPosición en texto: {match.Offset}\n");
                }
            }
        }

        static void RecognizePIIExample(TextAnalyticsClient client)
        {
            string document = "A developer with SSN 859-98-0987 whose phone number is 800-102-1100 is building tools with our APIs.";

            PiiEntityCollection entities = client.RecognizePiiEntities(document).Value;

            Console.WriteLine($"Texto Redactado: {entities.RedactedText}");
            if (entities.Count > 0)
            {
                Console.WriteLine($"Se reconoció un total de {entities.Count} entida{(entities.Count > 1 ? "des" : "d")} de Información Personal:");
                foreach (PiiEntity entity in entities)
                {
                    Console.WriteLine($"Texto: {entity.Text}, Categoria: {entity.Category}, Sub-Categoria: {entity.SubCategory}, Puntaje de Confianza: {entity.ConfidenceScore}");
                }
            }
            else
            {
                Console.WriteLine("No se encontró ninguna entidad.");
            }
        }

        static void KeyPhraseExtractionExample(TextAnalyticsClient client)
        {
            var response = client.ExtractKeyPhrases("My cat might need to see a veterinarian.");

            // Printing key phrases
            Console.WriteLine("");
            Console.WriteLine("Palabras Clave:");

            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }
    }

}
