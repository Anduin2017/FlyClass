// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var initDarkTheme = function () {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        // dark mode
        $('.navbar-light').addClass('navbar-dark');
        $('.navbar-light').removeClass('navbar-light');
        $('body').addClass('bg-dark');
        $('body').css('color', 'white');
        $('.modal-content').addClass('bg-dark');
        $('.modal-content').css('color', 'white');
        $('.container-fluid').addClass('bg-dark');
        $('.container-fluid').css('color', 'white');
        $('.list-group-item').addClass('bg-dark');
        $('.list-group-item').css('color', 'white');
        $('.content-wrapper').addClass('bg-dark');
        $('.card').addClass('bg-dark');
        $('.bg-light').addClass('bg-dark');
        $('.bg-light').removeClass('bg-light');
        $('.bg-white').addClass('bg-dark');
        $('.bg-white').removeClass('bg-white');
        $('.bd-footer').addClass('bg-dark');
        $('table').addClass('table-dark');
    }
}
initDarkTheme();

var mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
mediaQuery.addEventListener('change', initDarkTheme);
