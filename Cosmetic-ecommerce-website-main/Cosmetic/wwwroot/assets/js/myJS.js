


function showLoginPopup(modalType) {
    const modal = new bootstrap.Modal(document.getElementById('loginRequiredModal' + modalType));
    modal.show();
}

function redirectToLogin(extraParams = {}) {
    const currentUrl = window.location.pathname;
    const queryParams = new URLSearchParams(extraParams);

    const fullReturnUrl = currentUrl + (queryParams.toString() ? '?' + queryParams.toString() : '');

    window.location.href = `/Home/Login?returnUrl=${encodeURIComponent(fullReturnUrl)}`;
}



function notLoginInProductDetail() {
    const size = document.getElementById('size').value;
    const quantity = document.getElementById('quantity').value;
    redirectToLogin({
        size: size,
        quantity: quantity
    });
}

function showMessage(text) {
    var message = document.getElementById('messageContainer');

    if (message) {
        message.innerText = text;
        message.style.display = 'block';
        setTimeout(function () {
            message.style.display = 'none';
        }, 3000);
    }
}

function showWarning(text) {
    var warning = document.getElementById('warningContainer');
    if (warning) {
        warning.innerHTML = text;
        warning.style.display = 'block';
        setTimeout(() => {
            warning.style.display = 'none';
        }, 3000);
    }
}


function addToCart(productId) {
    const sizeElement = document.getElementById("size");
    const quantityElement = document.getElementById("quantity");

    const size = sizeElement ? sizeElement.value : null;
    const quantity = quantityElement ? parseInt(quantityElement.value) : 1;

    $.ajax({
        url: '/Carts/AddProductToCart',
        method: 'POST',
        data: { productId: productId, productSize: size, quantity: quantity },
        success: function (data) {
            if (data.success) {
                $.ajax({
                    url: '/Carts/ReloadMiniCart',
                    method: 'POST',
                    success: function (html) {
                        $("#miniCartContainer").html(html);
                    },
                    error: function (xhr, status, error) {
                        console.error('AJAX Error reloading MiniCart:', error);
                    }
                });

                showMessage(data.message);
            } else {
                showWarning(data.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', error);
        }
    });
}

function changeQuantityCartItem(cartItemId, productSize, newQuantity) {
    const inputElement = document.getElementById(`quantity-${cartItemId}`);
    const isChecked = document.getElementById(`cartItem-checkbox-${cartItemId}`).checked;
    if (newQuantity == null) {
        newQuantity = Math.min(100, Math.max(1, inputElement.value));
    }

    inputElement.value = Math.min(100, Math.max(1, newQuantity));

    $.ajax({
        url: "/Carts/ChangeQuantityCartItem",
        method: "POST",
        data: {
            cartItemId: cartItemId,
            newQuantity: newQuantity,
            productSize: productSize
        },
        success: function (data) {
            if (data.success) {
                if (isChecked) {
                    changeTotalPriceText(data.newFinalPrice, data.newTotalPrice);
                }
                changePriceTextInCartItem(cartItemId, data.finalPrice, data.totalPrice, data.productPriceAfterDiscount, data.productDiscount, data.productPrice);

            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error for change quantity cart item ", error);
        }
    });
}

function handleToggleCheckBoxCartItem(checkbox, productSize) {
    var isChecked = checkbox.checked;
    var cartItemId = checkbox.getAttribute("data-cartItemCheckBoxId");
    console.log(isChecked, " hehe");
    $.ajax({
        url: "/Carts/toggleCheckBoxCartItem",
        method: "POST",
        data: {
            cartItemId: cartItemId,
            isChecked: isChecked,
            productSize: productSize
        },
        success: function (data) {
            if (data.success) {

                changeTotalPriceText(data.newFinalPrice, data.newTotalPrice);

                changePriceTextInCartItem(cartItemId, data.finalPrice, data.totalPrice, data.productPriceAfterDiscount, data.productDiscount, data.productPrice);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error for change quantity cart item ", error);
        }
    });
}

function changePriceTextInCartItem(cartItemId, finalPrice, totalPrice, productPriceAfterDiscount, productDiscount, productPrice) {
    const finalPriceElement = document.querySelector(`#finalPrice-${cartItemId}`);
    const totalPriceElement = document.querySelector(`#totalPrice-${cartItemId}`);
    const productPriceAfterDiscountElement = document.querySelector(`#productPriceAfterDiscount-${cartItemId}`);
    const productPriceElement = document.querySelector(`#productPrice-${cartItemId}`);

    finalPriceElement.innerText = `$${finalPrice.toFixed(2)}`;
    productPriceAfterDiscountElement.innerText = `$${productPriceAfterDiscount.toFixed(2)}`
    if (totalPriceElement) {
        totalPriceElement.innerText = `$${totalPrice.toFixed(2)}`;
    }
    if (productPriceElement) {
        productPriceElement.innerText = `$${productPrice.toFixed(2)}`;
    }
    if (productDiscount == 0) {
        if (totalPriceElement) totalPriceElement.style.display = "none";
        if (productPriceElement) productPriceElement.style.display = "none";
    } else {
        if (totalPriceElement) totalPriceElement.style.display = "block";
        if (productPriceElement) productPriceElement.style.display = "block";
    }
}

function changeTotalPriceText(newFinalPrice, newTotalPrice) {
    const finalPriceFromAllCartItems = document.querySelector(`#finalPriceFromAllCartItems`);
    const totalPriceFromAllCartItems = document.querySelector(`#totalPriceFromAllCartItems`);
    const discountPriceFromAllCartItems = document.querySelector(`#discountPriceFromAllCartItems`);
    const rankDiscountFromAllCartItems = document.querySelector(`#rankDiscountFromAllCartItems`);
    const rankDiscount = document.getElementById('storeRankDiscount').getAttribute('data-rankDiscount');
    const tempFinalPriceElement = document.getElementById('storeTempFinalPrice');


    var prevFinalPriceAll = parseFloat(tempFinalPriceElement.innerText);
    var prevTotalPriceAll = parseFloat(totalPriceFromAllCartItems.innerText.replace('$', ''));
    var newFinalPriceAll = newFinalPrice + prevFinalPriceAll;
    var newTotalPriceAll = newTotalPrice + prevTotalPriceAll;
    tempFinalPriceElement.innerText = `${(newFinalPriceAll).toFixed(2)}`;
    totalPriceFromAllCartItems.innerText = `$${(newTotalPriceAll).toFixed(2)}`;
    discountPriceFromAllCartItems.innerText = `$${(newTotalPriceAll - newFinalPriceAll).toFixed(2)}`;
    finalPriceFromAllCartItems.innerText = `$${(newFinalPriceAll * ((100 - rankDiscount) / 100)).toFixed(2)}`;
    rankDiscountFromAllCartItems.innerText = `$${((newFinalPriceAll * rankDiscount) / 100).toFixed(2)}`;
}


function updatePriceInProductDetail(discount) {
    const selectedOption = document.getElementById('size').options[document.getElementById('size').selectedIndex];
    const quantity = document.getElementById("quantity").value;

    const price = parseFloat(selectedOption.getAttribute('data-price')) * quantity;
    if (discount != 0) {
        const originalPriceElement = document.getElementById('original-price');
        const discountPriceElement = document.getElementById('discount-price');

        const discountedPrice = price * ((100 - discount) / 100);

        originalPriceElement.textContent = '$' + price.toFixed(2);
        discountPriceElement.textContent = '$' + discountedPrice.toFixed(2);
    } else {
        document.getElementById('discount-price').textContent = '$' + price.toFixed(2);
    }
}



function submitFormData(formId, method) {
    var form = document.getElementById(formId);

    var formData = $(form).serialize();
    var actionUrl = form.getAttribute('action');
    $.ajax({
        url: actionUrl,
        type: method,
        data: formData,
        success: function (response) {
            $(".field-error").text('');
            if (response.success) {
                showMessage(response.message);


            } else {

                if (response.fieldErrors) {
                    for (var field in response.fieldErrors) {
                        var errorMessage = response.fieldErrors[field];
                        $("[data-valmsg-for='" + field + "']").text(errorMessage);
                    }
                } 
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });
}

function submitFormDataWithFile(formId, method) {
    var form = document.getElementById(formId);
    var formData = new FormData(form);

    var actionUrl = form.getAttribute('action');

    $.ajax({
        url: actionUrl,
        type: method,
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            $(".field-error").text('');

            if (response.success) {
                showMessage(response.message);

            } else {
                if (response.fieldErrors) {
                    console.log(response.fieldErrors);
                    for (var field in response.fieldErrors) {
                        var errorMessage = response.fieldErrors[field];
                        $("[data-valmsg-for='" + field + "']").text(errorMessage);
                    }
                }
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data", error);
        }
    });
}




function togglePassword(button) {
    const input = button.previousElementSibling;
    const icon = button.querySelector('i');

    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('fa-eye');
        icon.classList.add('fa-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.remove('fa-eye-slash');
        icon.classList.add('fa-eye');
    }
}


async function confirmDeleteAddressShipping() {
    const id = document.getElementById("deleteAddressId").value;
    const response = await fetch(`/Customer/DeleteAddressShipping?id=${id}`, {
        method: 'POST'
    });

    const result = await response.json();

    if (result.success) {

        const clickedCard = document.querySelector(`.shipping-address__card[data-id="${id}"]`);
        clickedCard.remove();

        var newDefaultId = result.newDefaultId;
        const newCard = document.querySelector(`.shipping-address__card[data-id="${newDefaultId}"]`);
        if (newCard) {
            newCard.classList.add('shipping-address__card--default');

            const cardBody = newCard.querySelector('.card-body');
            const badge = document.createElement('span');
            badge.className = 'badge bg-primary shipping-address__default-badge';
            badge.textContent = 'Default';
            cardBody.insertBefore(badge, cardBody.firstChild);

            const newBtn = newCard.querySelector('.shipping-address__set-default-btn');
            if (newBtn) {
                newBtn.disabled = true;
                newBtn.innerHTML = '<i class="fas fa-check me-1"></i>Default';
                newBtn.title = "Already Default";
            }
        }

        bootstrap.Modal.getInstance(document.getElementById("deleteAddressModal")).hide();



        showMessage(result.message);
    }
}

async function confirmDeleteCartItem() {
    const id = document.getElementById("deleteCartItemId").value;
    const productSize = document.getElementById("deleteCartItemProductSize").value;
    const response = await fetch(`/Carts/DeleteCartItemInShippingCart?id=${id}&productSize=${productSize}`, {
        method: 'POST'
    });

    const result = await response.json();

    if (result.success) {


        const cartItem = document.getElementById(`cartItemRow-${id}`);
        changeTotalPriceText(result.newFinalPrice, result.newTotalPrice);
        const checkbox = document.getElementById(`cartItem-checkbox-${id}`);
        checkbox.checked = false;
        cartItem.remove();

        bootstrap.Modal.getInstance(document.getElementById("deleteCartItemModal")).hide();

        showMessage(result.message);
    }
}

async function handleCheckOut() {
    const checkedCheckboxes = document.querySelectorAll(".cartItemCheckBox:checked");

    const cartItems = Array.from(checkedCheckboxes).map(checkbox => {
        const id = checkbox.getAttribute("data-cartItemCheckBoxId");
        const productSize = checkbox.getAttribute("onchange").match(/'([^']+)'/)[1];
        return { id: parseInt(id), productSize };
    });

    const finalPrice = parseFloat(document.getElementById("finalPriceFromAllCartItems").innerText.replace("$", ""));
    const totalPrice = parseFloat(document.getElementById("totalPriceFromAllCartItems")?.innerText.replace("$", "") || finalPrice);
    const discountPrice = parseFloat(document.getElementById("discountPriceFromAllCartItems").innerText.replace("$", ""));
    const rankDiscount = parseFloat(document.getElementById("rankDiscountFromAllCartItems").innerText.replace("$", ""));
    const totalDiscount = rankDiscount + discountPrice;
    const orderNote = document.getElementById("orderNote").value;

    const addressShippingElement = document.getElementById("addressShippingInfor");
    const addressId = addressShippingElement.getAttribute("data-id");

    const data = {
        CartItemsPlaceOrder: cartItems,
        FinalPrice: finalPrice,
        TotalPrice: totalPrice,
        TotalDiscount: totalDiscount,
        ProductDiscount: discountPrice,
        RankDiscount: rankDiscount,
        OrderNote: orderNote,
        AddressId: addressId
    };


    const response = await fetch('/Orders/PlaceOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });

    const result = await response.json();

    if (result.success) {
        showMessage(result.message);
        removeBadgeOfUnavailableCartItem(checkedCheckboxes)
    } else {
        showWarning(result.message);
        updateUnavailableCartItem(result.unavailableCartItems);
    }
}
function updateUnavailableCartItem(unavailableCartItems) {
    if (!unavailableCartItems || !unavailableCartItems.length) {
        return;
    }

    unavailableCartItems.forEach(eachCartItem => {
        const itemId = eachCartItem.id;
        const status = eachCartItem.status;
        const cartItemRow = document.getElementById(`cartItemRow-${itemId}`);
        const checkbox = document.getElementById(`cartItem-checkbox-${itemId}`);
        const quantityInput = document.getElementById(`quantity-${itemId}`);
        const minusBtn = document.getElementById(`plusQuantity-${itemId}`);
        const plusBtn = document.getElementById(`minusQuantity-${itemId}`);



        if (cartItemRow) {

            if (status === "Out of Stock") {
                cartItemRow.classList.add("opacity-50");
                changeTotalPriceText(-parseFloat(eachCartItem.finalPrice), -parseFloat(eachCartItem.totalPrice));
                if (quantityInput) {
                    quantityInput.disabled = true;
                    quantityInput.classList.add("bg-light", "text-muted");
                }

                if (minusBtn) {
                    minusBtn.classList.add("text-muted");
                    minusBtn.onclick = null;
                }

                if (plusBtn) {
                    plusBtn.classList.add("text-muted");
                    plusBtn.onclick = null;
                }

                if (checkbox) {
                    checkbox.checked = false;
                    checkbox.disabled = true;
                }
            }

            let badgeContainer = cartItemRow.querySelector(".position-absolute.top-0.start-0");
            if (!badgeContainer) {
                badgeContainer = document.createElement("div");
                badgeContainer.className = "position-absolute top-0 start-0";
                const productDiv = cartItemRow.querySelector(".shop-product");
                if (productDiv) {
                    productDiv.appendChild(badgeContainer);
                }
            } else {
                badgeContainer.innerHTML = "";
            }

            const badge = document.createElement("span");
            badge.className = "badge rounded-0 text-uppercase fs-14px px-5 py-4 ls-1 fw-semibold";

            if (status === "Out of Stock") {
                badge.classList.add("bg-danger", "text-white");
                badge.textContent = "out of stock";
            } else if (status === "Exceeds Stock") {
                badge.classList.add("bg-warning", "text-dark");
                badge.textContent = "exceeds stock";

                const quantityCell = cartItemRow.querySelector(".align-middle .shop-quantity").parentNode;
                let stockInfo = quantityCell.querySelector(".text-danger.mt-2");

                if (!stockInfo) {
                    stockInfo = document.createElement("div");
                    stockInfo.className = "text-danger mt-2 small";
                    quantityCell.appendChild(stockInfo);
                }

                stockInfo.textContent = `Only ${eachCartItem.remainQuantity} available`;
            }
            badgeContainer.appendChild(badge);
        }

    });
}

function removeBadgeOfUnavailableCartItem(checkedCheckboxes) {

    Array.from(checkedCheckboxes).forEach(eachCheckBox => {
        const id = eachCheckBox.getAttribute("data-cartItemCheckBoxId");
        const cartItemRow = document.getElementById(`cartItemRow-${id}`);
        if (cartItemRow) {
            const badge = cartItemRow.querySelector(".position-absolute.top-0.start-0");

            if (badge) {
                badge.remove();
            }
            const textDanger = cartItemRow.querySelector(".small.text-danger.mt-2");

            if (textDanger) {
                textDanger.remove();
            }
        }
    })
}


function debounce(func, delay) {
    let timer;
    return function (...args) {
        clearTimeout(timer);
        timer = setTimeout(() => {
            func.apply(this, args);
        }, delay);
    };
}


function handleChangeStatusOrder(modalId, status) {
    let orderId = null;

    if (status == "CANCELLED") {
        orderId = document.getElementById("cancelOrderId").value;
    } else if (status == "PENDING") {
        orderId = document.getElementById("repurchaseOrderId").value;
    }

    $.ajax({
        url: `/Orders/ChangeStatusOrder`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({ Id: orderId, Status: status }),
        success: function (response) {
            if (response.success) {

                showMessage(response.message);

                const orderCard = document.querySelector(`[data-cardOrderId="${orderId}"]`);
                if (orderCard) {
                    const statusElem = orderCard.querySelector(".order-status__myOrder");
                    statusElem.textContent = status.toUpperCase();
                    statusElem.className = `order-status__myOrder status-${status.toLowerCase()}__myOrder`;

                    if (status == "CANCELLED") {
                        const actionBtn = orderCard.querySelector(".cancel-order-btn__myOrder");
                        if (actionBtn) actionBtn.remove();
                        const repurchaseBtn = document.createElement("button");
                        repurchaseBtn.className = "rePurchase-order-btn__myOrder";
                        repurchaseBtn.setAttribute("data-bs-toggle", "modal");
                        repurchaseBtn.setAttribute("data-bs-target", "#warnRepurchaseOrderModal");
                        repurchaseBtn.setAttribute("data-order-id", orderId);

                        const icon = document.createElement("i");
                        icon.className = "fa-solid fa-check me-4";

                        repurchaseBtn.appendChild(icon);
                        repurchaseBtn.appendChild(document.createTextNode("Repurchase Order"));

                        orderCard.querySelector(".order-actions__myOrder").appendChild(repurchaseBtn);
                    } else if (status == "PENDING") {
                        const actionBtn = orderCard.querySelector(".rePurchase-order-btn__myOrder");
                        if (actionBtn) actionBtn.remove();
                        const cancelBtn = document.createElement("button");
                        cancelBtn.className = "cancel-order-btn__myOrder";
                        cancelBtn.setAttribute("data-bs-toggle", "modal");
                        cancelBtn.setAttribute("data-bs-target", "#warnCancelOrderModal");
                        cancelBtn.setAttribute("data-order-id", orderId);

                        const icon = document.createElement("i");
                        icon.className = "fas fa-times me-4";

                        cancelBtn.appendChild(icon);
                        cancelBtn.appendChild(document.createTextNode("Cancel Order"));

                        orderCard.querySelector(".order-actions__myOrder").appendChild(cancelBtn);

                    }
                }
                bootstrap.Modal.getInstance(document.getElementById(modalId)).hide();
            } else {
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });
}


function handleChangeStatusOrderByAdmin(modalId, status, orderId) {

    $.ajax({
        url: `/Orders/ChangeStatusOrder`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({ Id: orderId, Status: status }),
        success: function (response) {
            if (response.success) {

                showMessage(response.message);
                initialSelectInOrderDetailDashBoard(status);

                const oldSpan = document.getElementById("orderStatusDisplay");
                if (oldSpan) {
                    const newSpan = document.createElement("span");
                    newSpan.id = "orderStatusDisplay";
                    newSpan.className = `status-${status.toLowerCase()}__myOrder text-capitalize p-3 ms-4`;
                    newSpan.textContent = status;

                    oldSpan.replaceWith(newSpan);
                }

                bootstrap.Modal.getInstance(document.getElementById(modalId)).hide();
            } else {
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });
}


function initialSelectInOrderDetailDashBoard(orderStatus) {
    const selectElement = document.getElementById('selectChangeStatusOrder');
    selectElement.innerHTML = "";
    if (orderStatus == "PENDING") {
        selectElement.insertAdjacentHTML('beforeend',
            `
            <option value="PENDING" selected="true">PENDING</option>
            <option value="SHIPPED">SHIPPED</option>
            <option value="COMPLETED">COMPLETED</option>
            <option value="CANCELLED">CANCELLED</option>

            `
        );
    } else if (orderStatus == "SHIPPED") {
        selectElement.insertAdjacentHTML('beforeend',
            `
            <option value="SHIPPED" selected="true">SHIPPED</option>
            <option value="COMPLETED">COMPLETED</option>

            `)
    } else if (orderStatus == "COMPLETED") {
        selectElement.insertAdjacentHTML('beforeend',
            `
            <option value="COMPLETED" selected="true">COMPLETED</option>
            <option value="RETURN">RETURN</option>

            `)
    } else if (orderStatus == "RETURN") {
        selectElement.insertAdjacentHTML('beforeend',
            `
            <option value="RETURN" selected="true">RETURN</option>
            <option value="PENDING">PENDING</option>

            `)
    } else {
        selectElement.remove();
    }
}

function changeStatusCategory(id, isDelete, modalId) {

    $.ajax({
        url: `/Categories/ChangeStatusCategory`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({
            Id: id,
            isDelete: isDelete
        }),
        success: function (response) {
            if (response.success) {

                showMessage(response.message);

                const spanStatus = document.getElementById(`orderStatusClass-${id}`);
                if (spanStatus) {
                    spanStatus.className = `badge rounded-lg rounded-pill alert py-3 px-4 mb-0 border-0 text-capitalize fs-12 ${isDelete ? "alert-danger" : "alert-success"}`
                    spanStatus.textContent = `${isDelete ? "Unavailable" : "Available"}`
                }

                const btnToggle = document.getElementById(`btnToggleCategoryStatus-${id}`);

                if (btnToggle) {
                    btnToggle.innerHTML = !isDelete
                        ? `<i class="far fa-trash me-2"></i> Delete`
                        : `<i class="fas fa-undo me-2"></i> Restore`;

                    btnToggle.className = "btn btn-hover-text-light py-4 px-5 fs-13px btn-xs me-4";

                    if (!isDelete) {
                        btnToggle.classList.add("btn-outline-primary", "btn-hover-bg-danger", "btn-hover-border-danger");
                    } else {
                        btnToggle.classList.add("btn-outline-success", "btn-hover-bg-success", "btn-hover-border-success");
                    }

                    btnToggle.setAttribute("onclick", !isDelete
                        ? `confirmDelete(${id});`
                        : `confirmRestore(${id});`);
                }
                bootstrap.Modal.getInstance(document.getElementById(modalId)).hide();
            } else {
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });
}




function submitAddCategoryForm(formId) {
    var form = document.getElementById(formId);

    var formData = $(form).serialize();
    console.log(formData);

    $.ajax({
        url: `/Categories/CreateCategory`,
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success) {
                $(".field-error").text('');
                showMessage(response.message);
                var storeAllCategories = document.getElementById('storeAllCategories');
                var category = response.category;

                const tr = document.createElement('tr');
                tr.classList.add('categoryRow');
                tr.dataset.name = category.name;


                const tdId = document.createElement('td');
                tdId.textContent = category.id;
                tr.appendChild(tdId);

                const tdName = document.createElement('td');
                tdName.className = 'text-body-emphasis';
                const nameLink = document.createElement('a');
                nameLink.href = `/Products/Edit/${category.id}`;
                const namePara = document.createElement('p');
                namePara.className = 'fw-semibold text-body-emphasis mb-0';
                namePara.textContent = category.name;
                nameLink.appendChild(namePara);
                tdName.appendChild(nameLink);
                tr.appendChild(tdName);

                const tdDesc = document.createElement('td');
                tdDesc.className = 'text-body-emphasis';
                tdDesc.textContent = category.description;
                tr.appendChild(tdDesc);

                const tdStatus = document.createElement('td');
                const spanStatus = document.createElement('span');
                spanStatus.id = 'orderStatusClass';
                spanStatus.className = `badge rounded-lg rounded-pill alert py-3 px-4 mb-0 border-0 text-capitalize fs-12 ${category.status ? "alert-success" : "alert-danger"}`;
                spanStatus.textContent = category.status ? "Available" : "Unavailable";
                tdStatus.appendChild(spanStatus);
                tr.appendChild(tdStatus);

                const tdActions = document.createElement('td');
                tdActions.className = 'text-center';

                const actionWrapper = document.createElement('div');
                actionWrapper.className = 'd-flex flex-nowrap justify-content-center';

                const editLink = document.createElement('a');
                editLink.href = `/Categories/Edit/${category.id}`;
                editLink.className = 'btn btn-primary py-4 px-5 btn-xs fs-13px me-4';
                editLink.innerHTML = '<i class="far fa-pen me-2"></i> Edit';
                actionWrapper.appendChild(editLink);

                const toggleLink = document.createElement('a');
                toggleLink.id = `btnToggleCategoryStatus-${category.id}`;
                toggleLink.href = 'javascript:;';
                toggleLink.className = category.status
                    ? 'btn btn-outline-primary btn-hover-bg-danger btn-hover-border-danger btn-hover-text-light py-4 px-5 fs-13px btn-xs me-4'
                    : 'btn btn-outline-success btn-hover-bg-success btn-hover-border-success btn-hover-text-light py-4 px-5 fs-13px btn-xs me-4';
                toggleLink.setAttribute('onclick', category.status
                    ? `confirmDelete('${category.id}');`
                    : `confirmRestore('${category.id}');`);
                toggleLink.innerHTML = category.status
                    ? '<i class="far fa-trash me-2"></i> Delete'
                    : '<i class="fas fa-undo me-2"></i> Restore';

                actionWrapper.appendChild(toggleLink);
                tdActions.appendChild(actionWrapper);
                tr.appendChild(tdActions);

                storeAllCategories.appendChild(tr);

            } else {
                if (response.fieldErrors) {
                    $(".field-error").text('');

                    for (var field in response.fieldErrors) {
                        var errorMessage = response.fieldErrors[field];
                        $("[data-valmsg-for='NewCategory." + field + "']").text(errorMessage);
                    }

                }
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });

}


function submitEditCategoryForm(formId) {
    var form = document.getElementById(formId);

    var formData = $(form).serialize();
    $.ajax({
        url: `/Categories/EditCategory`,
        type: 'PUT',
        data: formData,
        success: function (response) {
            if (response.success) {
                $(".field-error").text('');
                showMessage(response.message);
            } else {
                if (response.fieldErrors) {
                    $(".field-error").text('');
                    for (var field in response.fieldErrors) {
                        var errorMessage = response.fieldErrors[field];
                        $("[data-valmsg-for='" + field + "']").text(errorMessage);
                    }
                }
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });
}

function changeStatusProduct(id, isDelete, modalId) {
    $.ajax({
        url: `/Products/ChangeProductStatus`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({
            Id: id,
            isDelete: isDelete
        }),
        success: function (response) {
            if (response.success) {

                showMessage(response.message);

                const spanStatus = document.getElementById(`productStatusClass-${id}`);
                if (spanStatus) {
                    spanStatus.className = `badge rounded-lg rounded-pill alert py-3 px-4 mb-0 border-0 text-capitalize fs-12 ${isDelete ? "alert-danger" : "alert-success"}`
                    spanStatus.textContent = `${isDelete ? "Unavailable" : "Available"}`
                }

                const btnToggle = document.getElementById(`btnToggleProductStatus-${id}`);

                if (btnToggle) {
                    btnToggle.innerHTML = !isDelete
                        ? `<i class="far fa-trash me-2"></i> Delete`
                        : `<i class="fas fa-undo me-2"></i> Restore`;

                    btnToggle.className = "btn btn-hover-text-light py-4 px-5 fs-13px btn-xs me-4";

                    if (!isDelete) {
                        btnToggle.classList.add("btn-outline-primary", "btn-hover-bg-danger", "btn-hover-border-danger");
                    } else {
                        btnToggle.classList.add("btn-outline-success", "btn-hover-bg-success", "btn-hover-border-success");
                    }

                    btnToggle.setAttribute("onclick", !isDelete
                        ? `confirmDelete(${id});`
                        : `confirmRestore(${id});`);
                }
                bootstrap.Modal.getInstance(document.getElementById(modalId)).hide();
            } else {
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });
}

function submitEditProductVariantForm(formId) {
    var form = document.getElementById(formId);

    var formData = $(form).serialize();
    $.ajax({
        url: `/Products/EditProductVariant`,
        type: 'PUT',
        data: formData,
        success: function (response) {
            if (response.success) {
                $(".field-error").text('');
                showMessage(response.message);
            } else {
                if (response.fieldErrors) {
                    $(".field-error").text('');
                    for (var field in response.fieldErrors) {
                        var errorMessage = response.fieldErrors[field];
                        $("[data-valmsg-for='" + field + "']").text(errorMessage);
                    }
                }
                showWarning(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error in Submit Data ", error);
        }
    });
}

function SelectProductOnChange() {
    const select = document.getElementById('selectProduct');
    const selectedOption = select.options[select.selectedIndex];

    const productId = selectedOption.value;
    const productType = selectedOption.dataset.type;

    const inputProductIdElement = document.getElementById('inputProductId');
    const inputProductTypeElement = document.getElementById('inputProductType');

    inputProductIdElement.value = productId;
    inputProductTypeElement.value = productType;


    inputProductIdElement.innerText = productId;
    inputProductTypeElement.innerText = productType;

    const storeProductVariantNameElement = document.getElementById('storeProductVariantName');
    storeProductVariantNameElement.innerHTML = '';



    const inputElement = document.createElement('input');
    const labelProductVariantNameElement = document.getElementById('labelProductVariantName');
    inputElement.className = 'form-control';
    inputElement.placeholder = 'Product Variant Name';
    inputElement.id = 'inputProductVariantName';
    inputElement.name = "Name";


    if (productType == "VolumeBased") {
        labelProductVariantNameElement.innerText = 'Name ( ml )';
        storeProductVariantNameElement.appendChild(inputElement);
        inputElement.type = 'number';
    } else if (productType == "WeightBased") {
        labelProductVariantNameElement.innerText = 'Name ( g )';
        storeProductVariantNameElement.appendChild(inputElement);
        inputElement.type = 'number';
    } else if (productType == 'SizeBased') {
        labelProductVariantNameElement.innerText = 'Name ( S, M, L )';
        const selectElement = document.createElement('select');
        selectElement.name = 'Name';
        selectElement.className = 'form-select';
        selectElement.id = 'inputProductVariantName';

        ['Small', 'Medium', 'Large'].forEach(size => {
            const option = document.createElement('option');
            option.value = size;
            option.text = size;
            selectElement.appendChild(option);
        });

        storeProductVariantNameElement.appendChild(selectElement);
    } else {
        labelProductVariantNameElement.innerText = 'Name ( Standard )';
        inputElement.value = 'Standard';
        inputElement.readOnly = true;

        storeProductVariantNameElement.appendChild(inputElement);
    }

    const validationSpan = document.createElement('span');
    validationSpan.className = 'text-danger field-error';
    validationSpan.setAttribute('data-valmsg-for', 'Name');
    validationSpan.setAttribute('data-valmsg-replace', 'true');
    storeProductVariantNameElement.appendChild(validationSpan);
}
