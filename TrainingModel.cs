using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.FastTree;

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
            try
            {
                Debug.WriteLine("Loading training data...");
                var trainingData = mlContext.Data.LoadFromTextFile<TalkingStageBot.InputData>(trainingDataPath, separatorChar: ',', hasHeader: true);

                Debug.WriteLine("Preprocessing training data...");
                var preprocessedTrainingData = mlContext.Data.CreateEnumerable<TalkingStageBot.InputData>(trainingData, reuseRowObject: false)
                    .Select(row => new TalkingStageBot.InputData
                    {
                        Text = row.Text.Trim().ToLower(),
                        Label = row.Label.Trim().ToLower()
                    });

                Debug.WriteLine("Creating IDataView from preprocessed data...");
                var preprocessedTrainingDataView = mlContext.Data.LoadFromEnumerable(preprocessedTrainingData);

                Debug.WriteLine("Defining data preparation and training pipeline...");
                var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName: @"Text", outputColumnName: @"Text")
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"Text" }))
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: @"Label", inputColumnName: @"Label", addKeyValueAnnotationsAsText: false))
                                    .Append(mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator: mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(new LbfgsLogisticRegressionBinaryTrainer.Options() { 
                                        L1Regularization = 0.031347524F, 
                                        L2Regularization = 0.03125F, 
                                        LabelColumnName = @"Label", 
                                        FeatureColumnName = @"Features" }), 
                                        labelColumnName: @"Label"))
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: @"PredictedLabel", inputColumnName: @"PredictedLabel"));

                Debug.WriteLine("Training the model...");

                var model = pipeline.Fit(preprocessedTrainingDataView);

                Debug.WriteLine("Saving the model...");
                mlContext.Model.Save(model, preprocessedTrainingDataView.Schema, modelPath);

                Debug.WriteLine("Model training and saving completed successfully.");
            }
            catch (PlatformNotSupportedException ex)
            {
                Debug.WriteLine("Platform not supported: " + ex.Message);
            }
            catch (TypeInitializationException ex)
            {
                Debug.WriteLine("Type initialization error: " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
