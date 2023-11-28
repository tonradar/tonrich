"use strict";
var App = /** @class */ (function () {
    function App() {
    }
    App.setCookie = function (name, value, seconds, remeber) {
        var date = new Date();
        date.setSeconds(date.getSeconds() + seconds);
        var cookie = "".concat(name, "=").concat(value, ";path=/;samesite=strict;");
        if (remeber == true) {
            cookie = cookie += "expires=".concat(date.toUTCString(), ";");
        }
        document.cookie = cookie;
    };
    App.getCookie = function (name) {
        var _a;
        // https://stackoverflow.com/a/25490531/2720104
        return ((_a = document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)')) === null || _a === void 0 ? void 0 : _a.pop()) || null;
    };
    App.removeCookie = function (name) {
        document.cookie = "".concat(name, "=; Max-Age=0");
    };
    App.goBack = function () {
        window.history.back();
    };
    /**
     * To disable the scrollbar of the body when showing the modal, so the modal can be always shown in the viewport without being scrolled out.
     **/
    App.setBodyOverflow = function (hidden) {
        document.body.style.overflow = hidden ? "hidden" : "auto";
    };
    App.applyBodyElementClasses = function (cssClasses, cssVariables) {
        cssClasses === null || cssClasses === void 0 ? void 0 : cssClasses.forEach(function (c) { return document.body.classList.add(c); });
        Object.keys(cssVariables).forEach(function (key) { return document.body.style.setProperty(key, cssVariables[key]); });
    };
    App.copy = function (text) {
        navigator.clipboard.writeText(text).then(function () {
            console.log(text, " copied to clipboard!");
        })
            .catch(function (error) {
            console.error(error);
        });
    };
    App.setLocalStorageItem = function (key, value) {
        localStorage.setItem(key, value);
    };
    App.getLocalStorageItem = function (key) {
        return localStorage.getItem(key);
    };
    return App;
}());
;
BitTheme.init({
    system: true,
    onChange: function (newTheme, oldThem) {
        //if (newTheme === 'dark') {
        //    document.body.classList.add('theme-dark');
        //    document.body.classList.remove('theme-light');
        //    document.querySelector("meta[name=theme-color]")!.setAttribute('content', '#0d1117');
        //} else {
        //    document.body.classList.add('theme-light');
        //    document.body.classList.remove('theme-dark');
        //    document.querySelector("meta[name=theme-color]")!.setAttribute('content', '#ffffff');
        //}
    }
});
