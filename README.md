# TalkingStage

TalkingStage is a conversational bot that uses machine learning to predict responses to various questions about personal preferences and characteristics. The bot is designed to assist users in gathering and responding to personal information during the "talking stage" of a relationship.

## Features

- Predefined responses for direct keyword matches.
- Machine learning-based predictions for non-direct keyword matches.
- Model training and saving functionality.
- Uses a saved model for making predictions in production.
- Supports multiple platforms, including macOS and Windows, would support Android and iOS in future updates through API model access or if microsoft makes ML.NET compatible for mobile devices.

  ## Screenshots
![talkingstage2 0](https://github.com/user-attachments/assets/69e85246-3d25-4cd9-8b66-cebe5dc7a41e)

![Screenshot 2024-06-29 at 03 26 57](https://github.com/Brainydaps/TalkingStage/assets/41041115/b3a18c48-0370-4471-8e84-2e24a2abfdd3)

[YouTube explaination of the brain structure of Talking Stage](https://youtu.be/_ELoo-8MJmI)

# What's New in TalkingStageBot v2.0

We are excited to announce the release of TalkingStageBot v2.0! This new version comes with significant enhancements and improvements to make the chatbot smarter and more efficient. Here’s what’s new:

## Key Features and Improvements

### 1. Enhanced Machine Learning Model

- **Upgraded ML Model**: The machine learning model has been upgraded to use the `OneVersusAll` trainer with the `LbfgsLogisticRegression` binary classifier. This provides better accuracy and performance for multiclass classification.
- **Regularization Parameters**: Fine-tuned L1 and L2 regularization parameters for optimal model performance.
  - L1 Regularization: 0.031347524F
  - L2 Regularization: 0.03125F

### 2. Improved Data Preprocessing

- **Text Normalization**: Text data is now preprocessed to ensure all text is in lowercase and trimmed, which helps in reducing the complexity and improving the model's ability to generalize.
- **Data Pipeline**: Enhanced data pipeline includes text featurization, feature concatenation, and key-to-value mapping for labels.

### 3. Robust Error Handling

- **Platform Compatibility**: Added specific handling for `PlatformNotSupportedException` to ensure smooth operation across different platforms.
- **Type Initialization**: Improved error handling for type initialization issues to make debugging easier and enhance stability.

### 4. Multi-Platform Support

- **Cross-Platform Compatibility**: TalkingStageBot is now compatible with multiple platforms, including Android, iOS, macOS, and Windows.

### 5. Simplified Training Process

- **Streamlined Training and Saving**: The process of training and saving the model has been streamlined with comprehensive debug logs to track the progress and catch errors effectively.


## Project Structure

- `TalkingStageBot.cs`: Main class for the bot functionality.
- `TrainingModel.cs`: Class responsible for training and saving the machine learning model.

## Setup and Installation

1. **Clone the repository:**

    ```sh
    git clone https://github.com/Brainydaps/TalkingStage.git
    cd TalkingStage
    ```

2. **Ensure you have .NET SDK installed:**
    - You can download it from the official [Microsoft .NET](https://dotnet.microsoft.com/download) website.

3. **Restore the required packages:**

    ```sh
    dotnet restore
    ```

4. **Build the project:**

    ```sh
    dotnet build
    ```

5. **Run the project:**

    ```sh
    dotnet run
    ```

## Usage

### Adding Training Data

1. Place your training data in a CSV file named `training_data.csv` in the project directory.
2. Ensure the CSV file has two columns: `Text` and `Label`.

### Running the Bot

When you run the project for the first time, the bot will check for a pre-existing model file (`model.zip`) in the project directory:
- If the model file is not found, it will train a new model using the provided `training_data.csv` and save it as `model.zip`.
- If the model file is found, it will load the existing model and create a `PredictionEngine` from it.

### Getting Responses

You can get responses by calling the `GetResponse` method with a question string:
```csharp
var bot = new TalkingStageBot();
var response = bot.GetResponse("What is your favorite color?");
Console.WriteLine(response);
```

## Development

### Code Structure

- **TalkingStageBot.cs:** Contains the bot logic, predefined responses, and model prediction methods.
- **TrainingModel.cs:** Contains the logic for training the machine learning model and saving it to a file.

### Logging and Error Handling

- Added logging to confirm paths for the model and training data.

## Machine Learning Techniques

### Data Preparation
- Text data is featurized using the `FeaturizeText` method, converting text into numerical vectors.
- The labels are mapped to keys using `MapValueToKey`.

### Model Training in the initial release
- The `SdcaMaximumEntropy` trainer from the ML.NET library is used for multiclass classification.
- Regularization parameters and the number of iterations were adjusted to improve model performance:
  - L2 Regularization: 0.1
  - L1 Regularization: 0.01
  - Maximum Number of Iterations: 1000

### Prediction
- The trained model is used to create a `PredictionEngine` that takes user input, processes it, and predicts the most appropriate label.

## Training Data

The training data consists of user queries and corresponding labels. The labels are aligned with predefined responses in the bot. The training data was populated by mapping various ways users might ask a question to a specific label. Below is a sample of the `training_data.csv` format also found in this repo:

```csv
Text,Label
What is your name,name
Where do you live,location
How old are you,age
What is your job,occupation
...
```

## Application Structure

- **TalkingStageBot.cs**: The core file containing the `TalkingStageBot` class, responsible for initializing the ML context, training the model, and providing responses.
- **training_data.csv**: The CSV file containing the training data used to train the machine learning model.
- **Responses Dictionary**: A predefined dictionary mapping keywords to responses, used for quick responses to known queries. Before compiling the code, edit the placeholders in the responses dictionary values to your actual information. For example, change "your name" to "My name is Adedapo Adeniran", "your age" to "I am 29 years old, born in October 1994", etc.
- **ML Model**: The ML.NET model trained using the `SdcaMaximumEntropy` trainer for multiclass classification.

## License

This project is licensed under the Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0) License. For more details, please refer to the [LICENSE](LICENSE) file.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue on GitHub.

## Contact

For any questions or inquiries, please contact Brainydaps via GitHub.
