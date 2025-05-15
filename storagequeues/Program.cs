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

QueueClient queueClient = new QueueClient(queueUri, new DefaultAzureCredential());

while (true)
{
    ShowMenu();
}

void ShowMenu()
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
            SendMessage();
            break;
        case "2":
            ReceiveMessage();
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

void SendMessage()
{
    Console.WriteLine("Enter the message to send and press Return/Enter:");
    string? message = Console.ReadLine();

    try
    {
        queueClient.SendMessage(message);
        Console.WriteLine();
        Console.WriteLine($"Message sent: {message}");
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Failed to send message: {ex.Message}");
        Console.WriteLine();
    }
}

void ReceiveMessage()
{
    try
    {
        QueueMessage[] messages = queueClient.ReceiveMessages(maxMessages: 1);

        if (messages.Length > 0)
        {
            var message = messages[0];
            Console.WriteLine();
            Console.WriteLine($"Message received: {message.Body}");
            Console.WriteLine();
            queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("No messages available.");
            Console.WriteLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Failed to receive message: {ex.Message}");
        Console.WriteLine();
    }
}
