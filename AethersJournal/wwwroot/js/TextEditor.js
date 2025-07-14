window.TextEditor = {
  currSelection: null,

  // Selection related functions
  saveSelection: function (event) {
    const sel = window.getSelection();
    if (sel.rangeCount > 0) {
      this.currSelection = sel.getRangeAt(0);
    }
  },

  restoreSelection: function () {
    const sel = window.getSelection();

    if (this.currSelection) {
      sel.removeAllRanges();
      sel.addRange(this.currSelection);
    }
  },

  handleFormat: function (event) {
    this.restoreSelection();
    const format = event.target.getAttribute("data-attribute");
    const element = this.currSelection.startContainer.parentElement;
    console.log(element);

    const span = document.createElement("span");

    if (element.nodeType === 1) {
      if (element.tagName.toLowerCase() === "span" & element.style.fontWeight === "bold") {
        console.log("he")
        span.textContent = element.textContent;
        element.remove();
        this.currSelection.insertNode(span);
        return;
      }
    } else {
      
    span.style.fontWeight = "bold";
    }

    element.remove();
    this.currSelection.insertNode(span);
  },

  checkContainer: function (event) {
    console.log(this.currSelection);
  },

  handlePaste: function (event) {
    event.preventDefault();
    const text = (event.clipboardData || window.clipboardData).getData(
      "text/plain"
    );

    const splitText = text.split(/\r?\n/);
    const selection = window.getSelection();

    if (!selection.rangeCount) return;

    const range = selection.getRangeAt(0);
    range.deleteContents();

    splitText.forEach((text) => {
      let node;

      if (text.trim() === "") {
        node = document.createElement("div");
        node.appendChild(document.createElement("br"));
      } else {
        node = document.createElement("div");
        node.textContent = text;
      }

      range.insertNode(node);

      range.setStartAfter(node);
      range.collapse(true);
    });
  },

  // For cooperation with C#
  // Insert Content
  setContent: function (element, journalContent) {
    element.innerHTML = journalContent;
  },

  setTitle: function (element, journalTitle) {
    element.value = journalTitle;
  },

  // Extract Content
  getContent: function (element) {
    return element.innerHTML;
  },

  getTitleContent: function(element) {
    return element.value;
  }
};

document.addEventListener("mouseup", () => TextEditor.saveSelection());
