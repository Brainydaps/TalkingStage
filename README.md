
# TalkingStageBot

TalkingStageBot is a multi-platform chatbot application designed to answer questions based on predefined responses and machine learning predictions. It leverages natural language processing (NLP) techniques to understand and respond to user queries.

## Table of Contents

- [Features](#features)
- [Screenshots](#screenshots)
- [Installation](#installation)
- [Usage](#usage)
- [Machine Learning Techniques](#machine-learning-techniques)
- [Training Data](#training-data)
- [Application Structure](#application-structure)
- [License](#license)
- [Contributing](#contributing)
- [Contact](#contact)


## Features

- Responds to user queries with predefined responses for known keywords.
- Uses a machine learning model to predict responses for queries without direct keyword matches.
- Leverages the ML.NET library for machine learning operations.
- Supports multiple platforms, including Android, iOS, macOS, and Windows. Works without issues on windows, but crashes on android, i would debug that soon, but for now, my brain needs some rest. 

## Screenshots
![Screenshot 2024-06-19 130341](https://github.com/Brainydaps/TalkingStage/assets/41041115/99b89534-1bee-47dc-b94d-ac0e7e833746)

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/Brainydaps/TalkingStageBot.git
    cd TalkingStageBot
    ```

2. Restore the required packages:
    ```bash
    dotnet restore
    ```

3. Build the project:
    ```bash
    dotnet build
    ```

## Usage

1. Run the application:
    ```bash
    dotnet run
    ```

2. The bot will be ready to accept queries and respond based on the logic defined.

## Machine Learning Techniques

### Data Preparation
- Text data is featurized using the `FeaturizeText` method, converting text into numerical vectors.
- The labels are mapped to keys using `MapValueToKey`.

### Model Training
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
What is your name,your name
Where do you live,your location
How old are you,your age
What is your job,your job
...
```

## Application Structure

- **TalkingStageBot.cs**: The core file containing the `TalkingStageBot` class, responsible for initializing the ML context, training the model, and providing responses.
- **training_data.csv**: The CSV file containing the training data used to train the machine learning model.
- **Responses Dictionary**: A predefined dictionary mapping keywords to responses, used for quick responses to known queries, before compiling the code, edit the placeholders in the responses dictionary values to your actual information, "your name" will be changed to "My name is Adedapo Adeniran", "your age" will be changed to "I am 29 years old, born in october 1994", and so on like that, so that the app will display your actual information to your intending lover instead of the placeholders.
- **ML Model**: The ML.NET model trained using the `SdcaMaximumEntropy` trainer for multiclass classification.

## License

This project is licensed under the Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0) License. For more details, please refer to the [LICENSE](LICENSE) file.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue on GitHub.

## Contact

For any questions or inquiries, please contact Brainydaps via GitHub.

