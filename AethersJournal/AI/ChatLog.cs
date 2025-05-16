enum ChatProfile {
    User,
    AI
}

class ChatLog {
    private ChatProfile _profile;
    private string _message;

    public ChatLog(ChatProfile ChatProfile, string Message) {
        _profile = ChatProfile;
        _message = Message;
    }

    public override string ToString()
    {
        return $"${_profile}: {_message}";
    }
}