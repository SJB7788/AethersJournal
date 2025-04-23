// event to handle paste
window.handlePaste = function (event) {
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
};
