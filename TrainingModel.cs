using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;

namespace TalkingStage
{
    public class TrainingModel
    {
        private MLContext mlContext;

        public TrainingModel()
        {
            mlContext = new MLContext();
        }

        public void TrainAndSaveModel(string trainingDataPath, string modelPath)
        {
            // Load training data
            var trainingData = mlContext.Data.LoadFromTextFile<TalkingStageBot.InputData>(trainingDataPath, separatorChar: ',', hasHeader: true);

            // Preprocess the training data to ensure consistency
            var preprocessedTrainingData = mlContext.Data.CreateEnumerable<TalkingStageBot.InputData>(trainingData, reuseRowObject: false)
                .Select(row => new TalkingStageBot.InputData
                {
                    Text = row.Text.Trim().ToLower(),
                    Label = row.Label.Trim().ToLower()
                });

            // Create IDataView from preprocessed data
            var preprocessedTrainingDataView = mlContext.Data.LoadFromEnumerable(preprocessedTrainingData);

            // Define the data preparation and training pipeline
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(TalkingStageBot.InputData.Text))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("LabelKey", nameof(TalkingStageBot.InputData.Label)))
                .Append(mlContext.Transforms.Concatenate("Features", "Features"))
                .AppendCacheCheckpoint(mlContext)
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                    labelColumnName: "LabelKey",
                    featureColumnName: "Features",
                    l2Regularization: 0.1f,
                    l1Regularization: 0.01f,
                    maximumNumberOfIterations: 1000))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            // Train the model
            var model = pipeline.Fit(preprocessedTrainingDataView);

            // Save the model
            mlContext.Model.Save(model, preprocessedTrainingDataView.Schema, modelPath);
        }
    }
}
