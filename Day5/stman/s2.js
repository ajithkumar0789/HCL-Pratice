// Array to store names
let l = [];

// Function called directly by ADD button
function addStudent() {
    let input = document.getElementById("st");
    let name = input.value.trim();

    if (name === "") {
        alert("Please enter a name!");
        return;
    }

    l.push(name);
    console.log("Updated Array:", l);

    // Clear the input box
    input.value = "";
}

// Function called directly by Display button
function displayStudents() {
    let outputDiv = document.getElementById("output");

    if (l.length === 0) {
        outputDiv.innerHTML = "<p>No names in list.</p>";
        return;
    }

    let listHtml = "<ul>";
    for (let i = 0; i < l.length; i++) {
        listHtml += `<li>${l[i]}</li>`;
    }
    listHtml += "</ul>";

    outputDiv.innerHTML = listHtml;
}