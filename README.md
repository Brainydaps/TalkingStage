# TalkingStage

TalkingStage is a conversational bot that uses machine learning to predict responses to various questions about personal preferences and characteristics. The bot is designed to assist users in gathering and responding to personal information during the "talking stage" of a relationship.

## Features

- Predefined responses for direct keyword matches.
- Machine learning-based predictions for non-direct keyword matches.
- Model training and saving functionality.
- Uses a saved model for making predictions in production.
- Supports multiple platforms, including macOS and Windows, would support Android and iOS in future updates through API model access or if microsoft makes ML.NET compatible for mobile devices.

  ## Screenshots
![Screenshot 2024-06-25 172627](https://github.com/Brainydaps/TalkingStage/assets/41041115/7b8825b7-c394-4861-92b6-55dd1c005c8d)
![Screenshot 2024-06-25 172912](https://github.com/Brainydaps/TalkingStage/assets/41041115/2a04c075-23b4-4ae5-a9c1-87972993eff6)
![Screenshot 2024-06-29 at 03 26 57](https://github.com/Brainydaps/TalkingStage/assets/41041115/b3a18c48-0370-4471-8e84-2e24a2abfdd3)

[YouTube explaination of the brain structure of Talking Stage](https://youtu.be/_ELoo-8MJmI)

# What's New in v1.3.0

## Key Updates

### Enhanced User Interaction
- **New Input Method**: Users can now send their messages to the bot by pressing the enter key or clicking the send button. This makes the interaction smoother and more intuitive.

### Improved Machine Learning Model
- **Algorithm Parameter Tuning**: Updated the machine learning algorithm parameters for better performance. The LightGBM model now has the following configuration:
  - **Number of Leaves**: 20
  - **Number of Iterations**: 100
  - **Minimum Example Count Per Leaf**: 5
  - **Learning Rate**: 0.05
  - **Gradient Booster Options**:
    - **Subsample Fraction**: 0.8
    - **Feature Fraction**: 0.8
    - **L1 Regularization**: 0.1
    - **L2 Regularization**: 0.2
  - **Maximum Bin Count Per Feature**: 254
  - **Early Stopping Rounds**: 20

### Expanded Training Data
- **Increased Data Points**: The training dataset has been expanded to over 3500 data points. This larger dataset helps in making the model more robust and accurate.

### Model Pipeline Enhancements
- **Preprocessing Steps**: The training data is now preprocessed to ensure consistency. This involves trimming and converting text to lowercase to avoid discrepancies.
- **Feature Engineering**: Enhanced feature engineering steps in the training pipeline to improve model performance.



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
