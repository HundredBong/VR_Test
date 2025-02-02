using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;

public class ChatGPTManager : MonoBehaviour
{
    [TextArea(5, 20)]
    public string personality;
    [TextArea(5, 20)]
    public string scene;
    public int maxResponseWordLimit = 15;

    public List<NPCAction> actions;

    public OnResponseEvent OnResponse;

    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    public Animator anim;

    public string GetInstructions()
    {
        string instructions = "You are a video game character and will answer to the message the player ask you. \n" +

            "You must reply to the player message only using the information from your Personnality and the Scene that are provided afterwards. \n" +

            "Do not invent or create response that are not mentionned in these information. \n" +

            "Do not break character or mention you are an AI or a video game character. \n" +



            "You must answer in less than " + maxResponseWordLimit + "words. \n" +



            "Here is the information about your personality : \n" +

            personality + "\n" +



            "Here is the information about the Scene around you : \n" +

            scene + "\n" +

            BuildActionInstructions() +

            "Here is the message of the player : \n";


        return instructions;

    }

    public string BuildActionInstructions()
    {
        string instructions = "";

        foreach (var item in actions)
        {
            instructions += "if i imply that I want you to do the following : " + item.actionDescription
                + ". You Must add to your answer the following keyword : " + item.actionKeyword + ". \n";
        }

        return instructions;

    }

    public async void AskChatGPT(string newText)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = GetInstructions() + newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4o";
        //gpt-4o-mini-2024-07-18
        //gpt-4o
        //gpt-3.5-turbo

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;

            foreach (var item in actions)
            {
                if (chatResponse.Content.Contains(item.actionKeyword))
                {
                    string textNoKeyword = chatResponse.Content.Replace(item.actionKeyword, "");
                    chatResponse.Content = textNoKeyword;
                    item.actionEvent.Invoke();
                }

            }

            messages.Add(chatResponse);

            Debug.Log(chatResponse.Content);

            OnResponse.Invoke(chatResponse.Content);
        }
    }

    [ContextMenu("test")]
    public void Test()
    {
        anim.SetTrigger("Flip");
    }
}

[System.Serializable]
public class OnResponseEvent : UnityEvent<string> { }

[System.Serializable]
public struct NPCAction
{
    public string actionKeyword;

    [TextArea(2, 5)]
    public string actionDescription;

    public UnityEvent actionEvent;
}
