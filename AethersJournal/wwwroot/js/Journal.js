window.Journal = {
  showSaveMessage: function (element, message, duration = 1000) {
    element.innerHTML = message;
    element.classList.add("visible");

    setTimeout(() => {
        console.log("hidden");
        element.classList.remove("visible");
    }, duration);
  },
};
