function updateScroll() {
    var element = document.getElementsByClassName("messages")[0];
    element.scrollTop = element.scrollHeight;
}

setTitle = (title) => { document.title = title; };

window.jsfunction = { focusElement: function (id) { const element = document.getElementById(id); element.focus(); } }
