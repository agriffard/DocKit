window.highlightCode = function () {
    document.querySelectorAll('pre code').forEach(function (block) {
        hljs.highlightElement(block);
    });
};
