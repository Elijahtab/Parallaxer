using OpenAI_API;
using OpenAI_API.Models;
using OpenAI_API.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;



public class OpenAIControllerScript : MonoBehaviour
{
    public GameObject chatPanel;
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    // Start is called before the first frame update
    void Start()
    {
        chatPanel.SetActive(false);
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        okButton.onClick.AddListener(() => GetResponse());
        string name = "Name";
    }

    
    public void StartConversation(string prompt, string starter, string ainame)
    {
        chatPanel.SetActive(true);

        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, prompt)
        };
        
        inputField.text = "";
        string startString = starter;
        textField.text = startString;
        name = ainame;
        

    }
    
    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        
        //Disable OK Button
        okButton.enabled = false;


        //Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            // Limit Message to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 100);

        }

        //Add the user message to the list of messages
        messages.Add(userMessage);

        textField.text = string.Format("You: {0}", userMessage.Content);

        inputField.text = "";

        //Get the response from the API
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = .5,
            MaxTokens = 300,
            Messages = messages
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;

        messages.Add(responseMessage);

        textField.text = string.Format("You: {0}\n\n{2} :{1}", userMessage.Content, responseMessage.Content, name);

        okButton.enabled = true;
        
    }


}
