window.ChatBox = {
    openOrCloseChatbox: function (event) {
        const dataTarget = event.currentTarget.getAttribute("data-target");
        
        const chatboxBody = document.querySelector(dataTarget);
        chatboxBody.classList.toggle("visible");
    },
    
    addMessage: function () {

    }
}