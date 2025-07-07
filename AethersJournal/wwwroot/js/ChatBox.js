window.ChatBox = {
    openOrCloseChatbox: function (event) {
        const dataTarget = event.currentTarget.getAttribute("data-target");
        
        const chatboxBody = document.querySelector(dataTarget);
        chatboxBody.classList.toggle("visible");
    },
    
    addMessage: function (chatboxDiv, chatMessage, isHuman) {
        const messageDiv = document.createElement('div');
        messageDiv.className = isHuman ? 'user-message' : 'ai-message';
        messageDiv.textContent = chatMessage;
        
        chatboxDiv.appendChild(messageDiv);
        
        // Scroll to the bottom to show the new message
        messageDiv.scrollIntoView({behavior: "smooth"});

        // Remove chatboxDiv inner text
        const input = document.getElementById("chatbox-input");
        input.value = "";
    }
}