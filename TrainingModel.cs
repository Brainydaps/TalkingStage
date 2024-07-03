using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
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
            var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName: @"Text", outputColumnName: @"Text")
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"Text" }))
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: @"Label", inputColumnName: @"Label", addKeyValueAnnotationsAsText: false))
                                    .Append(mlContext.MulticlassClassification.Trainers.LightGbm(new LightGbmMulticlassTrainer.Options() { 
                                        NumberOfLeaves = 20,
                                        NumberOfIterations = 100, 
                                        MinimumExampleCountPerLeaf = 5, 
                                        LearningRate = 0.05, 
                                        LabelColumnName = @"Label", FeatureColumnName = @"Features", ExampleWeightColumnName = null, 
                                        Booster = new GradientBooster.Options() { 
                                            SubsampleFraction = 0.8, 
                                            FeatureFraction = 0.8, 
                                            L1Regularization = 0.1, 
                                            L2Regularization = 0.2 }, 
                                        MaximumBinCountPerFeature = 254,
                                        EarlyStoppingRound = 20
                                    }))
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: @"PredictedLabel", inputColumnName: @"PredictedLabel"));

            // Train the model
            var model = pipeline.Fit(preprocessedTrainingDataView);

            // Save the model
            mlContext.Model.Save(model, preprocessedTrainingDataView.Schema, modelPath);
        }
    }
}
