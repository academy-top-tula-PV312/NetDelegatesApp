using NetDelegatesApp;

// covariant
MessageBuilder messageBuilder = WriteMessage;
Message message = messageBuilder("Hello world");
message.Print();
Console.WriteLine();

messageBuilder = WriteEmailMessage; // Covariant
message = messageBuilder("Hello people");
message.Print();
Console.WriteLine();

// contrvariant
EmailReceiver emailBox = ReceiveEmailMessage;
emailBox(new EmailMessage("Good by world"));
Console.WriteLine();

emailBox = ReceiveMessage;
emailBox(new EmailMessage("Good by people"));
Console.WriteLine();


// methods for covariant
Message WriteMessage(string text) => new Message(text);
EmailMessage WriteEmailMessage(string text) => new EmailMessage(text);

// methods for contrvariant
void ReceiveEmailMessage(EmailMessage message) => message.Print();
void ReceiveSmsMessage(SmsMessage message) => message.Print();
void ReceiveMessage(Message message) => message.Print();


// method for generic covariant
MessageBuilderT<SmsMessage> SmsMessageSender = (string text) => new SmsMessage(text);
MessageBuilderT<Message> MessageSender = SmsMessageSender;
Message messageSms = MessageSender("Hello Generic World");
messageSms.Print();
Console.WriteLine();

// method for generic contrvariant
MessageReceiverT<Message> messageReceiver = (Message message) => message.Print();
MessageReceiverT<SmsMessage> messageSmsReceiver = messageReceiver;

messageSmsReceiver(new SmsMessage("Good by Generic people"));
// messageSmsReceiver(new Message("Good by Generic people")); // error

messageReceiver(new Message("Good by Generic Base Message"));
messageReceiver(new EmailMessage("Good by Generic Email Message"));
messageReceiver(new SmsMessage("Good by Generic Sms Message"));


// both
MessageConvert<Message, EmailMessage> toEmailConverter = (Message message) => new EmailMessage(message.Text);
MessageConvert<SmsMessage, Message> converter = toEmailConverter;

Message messageBoth = converter(new SmsMessage("Both Sms Message"));
messageBoth.Print();



// non generic delegates
delegate Message MessageBuilder(string text); // delegate for covariant
delegate void EmailReceiver(EmailMessage message); // delegate for contrvariant


// generic delegates
delegate T MessageBuilderT<out T>(string text); // generic delegate for covariant
delegate void MessageReceiverT<in T>(T message); // generic delegate for contrvariant
delegate TOut MessageConvert<in TIn, out TOut>(TIn message); // both



class Message
{
    public string Text { set; get; }

    public Message(string text) => Text = text;

    public virtual void Print() => Console.WriteLine(Text);
}

class EmailMessage : Message
{
    public EmailMessage(string text) : base(text) { }

    public override void Print() => Console.WriteLine($"Email: {Text}");
}

class SmsMessage : Message
{
    public SmsMessage(string text) : base(text) { }

    public override void Print() => Console.WriteLine($"Sms: {Text}");
}
