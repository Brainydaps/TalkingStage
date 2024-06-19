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

        private void OnSendClicked(object sender, EventArgs e)
        {
            string question = UserInput.Text;
            if (string.IsNullOrEmpty(question))
            {
                System.Diagnostics.Debug.WriteLine("User input is null or empty.");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Sending question: '{question}' to chatbot.");

            // Get response from chatbot
            string response = chatbot.GetResponse(question);

            // Display response
            ResponseLabel.Text = response;

            System.Diagnostics.Debug.WriteLine($"Received response: '{response}'.");
        }
    }
}
