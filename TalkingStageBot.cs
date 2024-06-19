using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;

namespace TalkingStage
{
    public class TalkingStageBot
    {
        private static Dictionary<string, string> responses = new Dictionary<string, string>
        {
            { "name", "your name" },
            { "location", "your location" },
            { "age", "your age" },
            { "occupation", "your job" },
            { "height", "your height" },
            { "physical activity", "your level of physical activities" },
            { "educational level", "your educational level" },
            { "drinking habit", "your drinking frequency" },
            { "smoking habit", "your smoking frequency" },
            { "gender identity", "your gender" },
            { "seeking", "what you seek from your partner" },
            { "want children", "if you want children or not" },
            { "star sign", "your horoscope sign" },
            { "politics", "how political are you" },
            { "religion", "your religion or lack of" },
            { "tribe", "your tribe if you have one" },
            { "hobbies", "your hobbies" },
            { "passion", "your passion" },
            { "dreams", "what you dream of doing or being" },
            { "expectations in a relationship", "what you expect in a relationship" },
            { "favorite food", "best food" },
            { "favorite color", "best color" },
            { "favorite animal", "best animal" },
            { "favorite movie", "best movie" },
            { "favorite book", "best book" },
            { "favorite music genre", "best music type" },
            { "favorite artist", "best musician" },
            { "favorite travel destination", "best place to travel" },
            { "favorite sport", "best sport" },
            { "favorite team", "best team" },
            { "favorite player", "best player" },
            { "favorite subject in school", "best subject" },
            { "favorite type of music", "best music type" },
            { "favorite type of food", "best food type" },
            { "favorite type of movie", "best movie type" },
            { "favorite type of book", "best book type" },
            { "favorite type of sport", "best sport type" },
            { "favorite type of travel", "best travel type" },
            { "do you prefer being the one to start chats the most or your man?", "initiates chat" }
        };

        private MLContext mlContext;
        private ITransformer model;
        private PredictionEngine<InputData, Prediction> predictionEngine;

        public TalkingStageBot()
        {
            mlContext = new MLContext();

            // Determine the path to training_data.csv relative to the executable
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var trainingDataPath = Path.Combine(baseDirectory, "training_data.csv");

            // Load training data
            var trainingData = mlContext.Data.LoadFromTextFile<InputData>(trainingDataPath, separatorChar: ',', hasHeader: true);

            // Preprocess the training data to ensure consistency
            var preprocessedTrainingData = mlContext.Data.CreateEnumerable<InputData>(trainingData, reuseRowObject: false)
                .Select(row => new InputData
                {
                    Text = row.Text.Trim().ToLower(),
                    Label = row.Label.Trim().ToLower()
                });

            // Create IDataView from preprocessed data
            var preprocessedTrainingDataView = mlContext.Data.LoadFromEnumerable(preprocessedTrainingData);

            // Define the data preparation and training pipeline
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(InputData.Text))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("LabelKey", nameof(InputData.Label)))
                .Append(mlContext.Transforms.Concatenate("Features", "Features"))
                .AppendCacheCheckpoint(mlContext)
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                    labelColumnName: "LabelKey",
                    featureColumnName: "Features",
                    l2Regularization: 0.1f,           // Adjust regularization strength
                    l1Regularization: 0.01f,
                    maximumNumberOfIterations: 1000)) // Adjust maximum number of iterations
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            // Train the model
            model = pipeline.Fit(preprocessedTrainingDataView);

            // Create a prediction engine
            predictionEngine = mlContext.Model.CreatePredictionEngine<InputData, Prediction>(model);
        }

        public string GetResponse(string question)
        {
            // Ensure lowercase for consistency
            question = question.ToLower();

            // Check if any keywords directly match in the question
            foreach (var keyword in responses.Keys)
            {
                if (question.Contains(keyword))
                {
                    return responses[keyword];
                }
            }

            // If no direct keyword match, then use ML model prediction
            var prediction = predictionEngine.Predict(new InputData { Text = question });
            System.Diagnostics.Debug.WriteLine("Prediction engine is initialized");
            System.Diagnostics.Debug.WriteLine($"Predicted Label: '{prediction.PredictedLabel}', Score: [{string.Join(", ", prediction.Score.DenseValues())}]");

            if (string.IsNullOrEmpty(prediction.PredictedLabel))
            {
                System.Diagnostics.Debug.WriteLine("PredictedLabel is null");
                return "I don't have an answer for that.";
            }

            // Debug: Check if predicted label exists in the responses dictionary
            System.Diagnostics.Debug.WriteLine($"Checking if predicted label '{prediction.PredictedLabel}' exists in the responses dictionary...");
            foreach (var key in responses.Keys)
            {
                System.Diagnostics.Debug.WriteLine($"Dictionary Key: '{key}'");
            }

            if (responses.TryGetValue(prediction.PredictedLabel, out var response))
            {
                System.Diagnostics.Debug.WriteLine("Prediction engine gave response");
                return response;
            }

            System.Diagnostics.Debug.WriteLine("Predicted label not found in responses dictionary");
            return "I don't have an answer for that.";
        }

        public class InputData
        {
            [LoadColumn(0)] // Column index for 'Text' in training_data.csv
            public string Text { get; set; }

            [LoadColumn(1)] // Column index for 'Label' in training_data.csv
            public string Label { get; set; }
        }

        public class Prediction
        {
            [ColumnName("PredictedLabel")]
            public string PredictedLabel { get; set; }

            public VBuffer<float> Score { get; set; }
        }
    }
}
