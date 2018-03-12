
function PrintHello() {
    var message = "hello world?";
    document.write(message);
}

function PrintDateFromStr(inputStr) {
    var dateArr = inputStr.split(" ");
    var fullDate = new Date();

    fullDate.setFullYear(dateArr[0], dateArr[2] - 1, dateArr[4]);
    document.write(fullDate, "<br />");
}