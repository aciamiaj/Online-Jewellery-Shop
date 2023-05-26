// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function changeColor(element, color) {
    var svg = element.querySelector("svg");
    var path = svg.querySelector("path");
    path.setAttribute("fill", color);
}