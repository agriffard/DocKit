window.highlightCode = function () {
    document.querySelectorAll('pre code').forEach(function (block) {
        hljs.highlightElement(block);
    });
};

window.scrollToHeading = function (id) {
    var el = document.getElementById(id);
    if (el) {
        el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        history.replaceState(null, '', location.pathname + location.search + '#' + id);
    }
};
