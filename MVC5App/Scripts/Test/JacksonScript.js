
function PrintHello() {
    var message = "hello world?";
    document.write(message);
}

function PrintDate() {
    var dateStr = "2017 - 07 - 09 11:07:00 UTC";
    var dateArr = dateStr.split(" ");
    var fullDate = new Date(dateArr[0], dateArr[2] + 1, dateArr[4]);

    document.write("<p>", fullDate, "</p>");

    document.write("Year: ", dateArr[0], "<br />");
    document.write("Month: ", dateArr[2], "<br />");
    document.write("Day: ", dateArr[4], "<br />");    
}

function PrintDateFromStr(inputStr) {
    document.write(inputStr);
}
