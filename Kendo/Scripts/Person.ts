
class Person {

    Id: number;
    Name: string;

    constructor()
    {
    }


    public GetPersons(): string
    {
        var lstPersons = new Array<Person>();
        var jsonTxt;
        var http = new XMLHttpRequest();
        http.onreadystatechange = function () {

            if (this.readyState == 4 && this.status == 200) {
                jsonTxt = this.responseText;
            }
        }
        
        http.open("GET", "/Home/GetPersons",true);
        http.send();
        return jsonTxt;
    }

}

function showPersons() {
    var p = new Person();
    var persons = p.GetPersons();

}