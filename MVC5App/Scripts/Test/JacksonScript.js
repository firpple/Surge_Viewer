//=============================================================================
// PrintHello()
// Hello world used for learning JS
//=============================================================================
function PrintHello() {
    var message = "hello world?";
    document.write(message);
}

//=============================================================================
// PrintDateFromStr(inputStr)
// Prints date crom string in the format of: ""yyyy-mm-dd hh-mm-ss GMT"
//=============================================================================
function PrintDateFromStr(inputStr) {
    var dateArr = inputStr.split(" ")[0].split("-");
    var fullDate = new Date();

    fullDate.setFullYear(dateArr[0], dateArr[1] - 1, dateArr[2]);
    document.write(fullDate);
}

//=============================================================================
// ParseDate()
// inputStr: string date in the format of: "yyyy-mm-dd hh-mm-ss GMT"
//=============================================================================
function ParseDate(inputStr) {
    var dateArr = inputStr.split(" ")[0].split("-");
    var fullDate = new Date();

    fullDate.setFullYear(dateArr[0], dateArr[1] - 1, dateArr[2]);

    return fullDate;
}