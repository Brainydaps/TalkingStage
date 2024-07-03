using System;
using Microsoft.Maui.Controls;

namespace TalkingStage
{
    public partial class MainPage : ContentPage
    {
        private TalkingStageBot chatbot;

        public MainPage()
        {
            InitializeComponent();
            chatbot = new TalkingStageBot(); // Initialize the chatbot
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async void OnEntryCompleted(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async Task SendMessage()
        {
            string question = UserInput.Text;
            if (string.IsNullOrEmpty(question))
            {
                System.Diagnostics.Debug.WriteLine("User input is null or empty.");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Sending question: '{question}' to chatbot.");

            // Get response from chatbot
            var (response, isFirstInteraction) = chatbot.GetResponse(question);

            // Display alert if it's the first interaction
            if (isFirstInteraction)
            {
                await DisplayAlert("Information", "Note: Different questions are identified and answered when separated by a comma, question mark, full stop or exclamation marks.", "OK");
                return;
            }

            // Display response
            ResponseLabel.Text = response;

            System.Diagnostics.Debug.WriteLine($"Received response: '{response}'.");
        }
    }
}
