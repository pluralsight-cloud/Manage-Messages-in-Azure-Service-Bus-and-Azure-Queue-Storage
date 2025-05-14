using Azure.Messaging.ServiceBus;

/*
 * NOTE:
 * The code below is a simple console application that allows you to send and receive messages from an Azure Service Bus queue.
 * It uses the Azure.Messaging.ServiceBus library to interact with the Service Bus.
 * The application provides a menu for the user to choose between sending a message, receiving a message, or exiting the application.
 * The connection string and queue name are hardcoded for simplicity.
 * It is not designed to be a production-ready application and should be used for educational purposes only.
 */

// TODO: Replace the "<NAMESPACE-NAME>" and "<QUEUE-NAME>" placeholders.
const string namespaceConnectionString = "<CONNECTION-STRING>";
const string queueName = "<QUEUE-NAME>";

// The client that owns the connection and can be used to create senders and receivers
ServiceBusClient client;

var clientOptions = new ServiceBusClientOptions
{ 
    TransportType = ServiceBusTransportType.AmqpWebSockets,
    ConnectionIdleTimeout = TimeSpan.FromMilliseconds(5000)
};

client = new ServiceBusClient(namespaceConnectionString, clientOptions);

// The sender used to publish messages to the queue
ServiceBusSender sender = client.CreateSender(queueName);

// The receiver used to receive the messages from the queue
ServiceBusReceiver receiver = client.CreateReceiver(queueName);

while (true)
{
    ShowMenu();
}

void ShowMenu() {
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

async void SendMessage()
{
    Console.WriteLine("Enter the message to send:");
    string? message = Console.ReadLine();

    try
    {
        ServiceBusSender sender = client.CreateSender(queueName);
        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(message);

        await sender.SendMessageAsync(serviceBusMessage);
        Console.WriteLine($"Message sent: {message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sending message: {ex.Message}");
    }
}

async void ReceiveMessage()
{
    try
    {
        ServiceBusReceiver receiver = client.CreateReceiver(queueName);
        ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

        if (receivedMessage != null)
        {
            Console.WriteLine($"Received message: {receivedMessage.Body}");
            await receiver.CompleteMessageAsync(receivedMessage);
        }
        else
        {
            Console.WriteLine("No messages available in the queue.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error receiving message: {ex.Message}");
    }
}
