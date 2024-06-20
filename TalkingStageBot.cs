using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

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
        private DataViewSchema modelSchema;

        public TalkingStageBot()
        {
            mlContext = new MLContext();
            var modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "model.zip");
            var trainingDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "training_data.csv");

            System.Diagnostics.Debug.WriteLine($"Model path: {modelPath}");
            System.Diagnostics.Debug.WriteLine($"Training data path: {trainingDataPath}");

            if (!File.Exists(modelPath))
            {
                // Train and save the model if it doesn't exist
                var trainingModel = new TrainingModel();
                trainingModel.TrainAndSaveModel(trainingDataPath, modelPath);
            }

            // Load the model
            model = mlContext.Model.Load(modelPath, out modelSchema);
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
            var prediction = Predict(question);

            if (prediction == null || string.IsNullOrEmpty(prediction.PredictedLabel))
            {
                return "I don't have an answer for that.";
            }

            if (responses.TryGetValue(prediction.PredictedLabel, out var response))
            {
                return response;
            }

            return "I don't have an answer for that.";
        }

        private Prediction? Predict(string text)
        {
            var inputData = new List<InputData> { new InputData { Text = text } };
            var inputDataView = mlContext.Data.LoadFromEnumerable(inputData);

            var transformedData = model.Transform(inputDataView);

            var predictions = mlContext.Data.CreateEnumerable<Prediction>(transformedData, reuseRowObject: false).ToList();

            return predictions.FirstOrDefault();
        }

        public class InputData
        {
            [LoadColumn(0)] // Column index for 'Text' in training_data.csv
            public string Text { get; set; } = string.Empty;

            [LoadColumn(1)] // Column index for 'Label' in training_data.csv
            public string Label { get; set; } = string.Empty;
        }

        public class Prediction
        {
            [ColumnName("PredictedLabel")]
            public string PredictedLabel { get; set; } = string.Empty;

            public VBuffer<float> Score { get; set; }
        }
    }
}
