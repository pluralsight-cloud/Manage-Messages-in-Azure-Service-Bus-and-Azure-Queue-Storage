using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Azure.Identity;

/*
 * NOTE:
 * The code below is a simple console application that allows you to send and receive messages from an Azure Service Bus queue.
 * It uses the Azure.Storage.Queues library to interact with the Queue Storage.
 * The application uses the Azure.Identity library for authentication.
 * The application provides a menu for the user to choose between sending a message, receiving a message, or exiting the application.
 * The storageAccountName and queue name are hardcoded for simplicity.
 * It is not designed to be a production-ready application and should be used for educational purposes only.
 */

// TODO: Replace the "<STORAGE-ACCOUNT-NAME>" and "<QUEUE-NAME>" placeholders.
const string storageAccountName = "<STORAGE-ACCOUNT-NAME>";
const string queueName = "<QUEUE-NAME>";

var queueUri = new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}");

await using QueueClient queueClient = new QueueClient(queueUri, new DefaultAzureCredential());

while (true)
{
    ShowMenu();
}

async Task ShowMenu()
{
    Console.WriteLine("---------");
    Console.WriteLine("Main Menu");
    Console.WriteLine("---------");
    Console.WriteLine("Please choose an option:");
    Console.WriteLine("1. Send a message to the queue");
    Console.WriteLine("2. Receive a message from the queue");
    Console.WriteLine("3. Exit");
    Console.Write("Enter your choice (1-3): ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await SendMessage();
            break;
        case "2":
            await ReceiveMessage();
            break;
        case "3":
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Invalid choice, please try again.\n");
            break;
    }
}

async Task SendMessage()
{
    Console.WriteLine("Enter the message to send:");
    string? message = Console.ReadLine();

    try
    {
        await queueClient.SendMessageAsync(message);
        Console.WriteLine($"Message sent: {message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to send message: {ex.Message}");
    }
}

async Task ReceiveMessage()
{
    try
    {
        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);

        if (messages.Length > 0)
        {
            var message = messages[0];
            Console.WriteLine($"Message received: {message.Body}");
            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }
        else
        {
            Console.WriteLine("No messages available.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to receive message: {ex.Message}");
    }
}