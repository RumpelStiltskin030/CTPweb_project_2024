// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', () => {
    const menuButton = document.getElementById('menu-button');
    const menu = document.getElementById('menu');

    menuButton.addEventListener('click', (event) => {
        event.stopPropagation(); // 防止點擊事件傳遞到 document
        menu.classList.toggle('show');
    });

    // 點擊選單外部時隱藏選單
    document.addEventListener('click', (event) => {
        if (!menuButton.contains(event.target) && !menu.contains(event.target)) {
            menu.classList.remove('show');
        }
    });
});
