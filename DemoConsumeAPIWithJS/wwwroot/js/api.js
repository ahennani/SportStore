let url = "https://localhost:44364/products";

let productsList = document.getElementById("products-list");
if (productsList) {
    fetch(url)
        .then(response => response.json())
        .then(data => displayPdoructs(data))
        .catch(ex => {
            alert("Something Went Wrong ...");
            console.log(ex);
        })
}


const displayPdoructs = (products) => {
    console.log(products);

    products.forEach(product => {
        let li = document.createElement("li");
        li.classList.add("list-group-item");
        let text = `Name: ${product.name} == Price: ${product.price}`
        li.appendChild(document.createTextNode(text));
        productsList.appendChild(li);
    });

}