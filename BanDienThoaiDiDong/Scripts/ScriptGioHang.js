// get all the quantity forms on the page
const quantityForms = document.querySelectorAll('.quantity');


// loop through each form and attach a click event listener to the buttons
quantityForms.forEach(form => {
    const idPro = form.querySelector('#idPro').value;
    const formId = form.getAttribute('data-form-id');
    const minusBtn = form.querySelector('.minus');
    const plusBtn = form.querySelector('.plus');
    const qty = form.querySelector('.qty')

    minusBtn.addEventListener('click', function (event) {
        console.log('Minus button clicked for product ID:', idPro);
        let quantity = parseInt(this.nextElementSibling.value); // get the current quantity value
        quantity--; // decrement the quantity by 1
        if (quantity < 0) { // make sure the quantity does not go below zero
            quantity = 0;

        }
        this.nextElementSibling.value = quantity.toString(); // update the input field with the new quantity value
        // add code to update the quantity of the product with the given ID
        Update_Cart(idPro, quantity);
    });

    plusBtn.addEventListener('click', function (event) {
        console.log('Plus button clicked for product ID:', idPro);
        let quantity = parseInt(this.previousElementSibling.value); // get the current quantity value
        quantity++; // increment the quantity by 1
        this.previousElementSibling.value = quantity.toString(); // update the input field with the new quantity value
        // add code to update the quantity of the product with the given ID
        Update_Cart(idPro, quantity);
    });
});
function Update_Cart(idPro, quant) {
    //let idPro = value;
    //let quant = $(".qty").val();
    console.log("" + idPro + "| " + quant)
    $.ajax({
        type: "POST",
        url: "/ShoppingCart/Update_Cart_Quantity",
        data: { idPro: idPro, quant: quant },
        success: function (response) {
            if (response == "success") {
                window.location.href = "/ShoppingCart/ShowCart"
            }
        }
    });
}
//const selectedOption = document.querySelector('input[name="value-radio"]:checked').value;
//console.log(selectedOption); // This will log the value of the selected radio button q

