# TalkingStage

TalkingStage is a conversational bot that uses machine learning to predict responses to various questions about personal preferences and characteristics. The bot is designed to assist users in gathering and responding to personal information during the "talking stage" of a relationship.

## Features

- Predefined responses for direct keyword matches.
- Machine learning-based predictions for non-direct keyword matches.
- Model training and saving functionality.
- Uses a saved model for making predictions in production.

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

3. **Build the project:**

    ```sh
    dotnet build
    ```

4. **Run the project:**

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
- If the model file is found, it will load the existing model.

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

## Contributing

Feel free to fork this repository, make changes, and submit pull requests. Contributions are welcome!

### Reporting Issues

If you encounter any issues or have suggestions, please create an issue in the GitHub repository.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
