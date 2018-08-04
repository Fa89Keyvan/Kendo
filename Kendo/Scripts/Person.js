var Person = /** @class */ (function () {
    function Person() {
    }
    Person.prototype.GetPersons = function () {
        var lstPersons = new Array();
        var jsonTxt;
        var http = new XMLHttpRequest();
        http.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                jsonTxt = this.responseText;
            }
        };
        http.open("GET", "/Home/GetPersons", true);
        http.send();
        return jsonTxt;
    };
    return Person;
}());
function showPersons() {
    var p = new Person();
    var persons = p.GetPersons();
}
//# sourceMappingURL=Person.js.map