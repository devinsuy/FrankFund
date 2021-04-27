/* global varName */
/* eslint-env jquery */
import Swal from 'sweetalert2'

export default function Transactions({transactions}) {
    return (
        <>
            { transactions.map((transaction) => (<Transaction key={transaction.TID} transaction={transaction} />))}
        </>
    )
}

const Transaction = ({ transaction }) => {
    // Convert date to readable format 
    let dateMade = transaction.DateTransactionMade != "" ? new Date(transaction.DateTransactionMade.replace(/-/g, '\/')).toDateString() : "";
    let dateEntered = transaction.DateTransactionEntered != "" ? new Date(transaction.DateTransactionEntered.replace(/-/g, '\/')).toDateString() : "";

    // Build api url for this particular goal
    let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    let url = `/api/Transaction/TID=${transaction.TID}&apikey=${apikey}`;

    return (
        <>
            <tr id={`Transaction${transaction.TID}`} key={transaction.TID}>
                <td id={`TransactionName${transaction.TID}`}> {transaction.TransactionName}</td>
                <td id={`Amount${transaction.TID}`}> {transaction.Amount != "" ? "$" + transaction.Amount : ""}</td>
                <td id={`DateTransactionMade${transaction.TID}`}> {dateMade}</td>
                <td id={`DateTransactionEntered${transaction.TID}`}> {dateEntered}</td>
                <td id={`IsExpense${transaction.TID}`}> {transaction.IsExpense == true ? "Expense" : transaction.IsExpense == false ? "Income" : ""}</td>
                <td id={`TransactionCategory${transaction.TID}`}> {transaction.TransactionCategory}</td>
                
                { // Display buttons only if user has any transactions at all
                    transaction.IsExpense == null ? <></> :
                    <td>
                        <button onClick={viewAlert} className="btn btn-outline-success btn-sm">View</button>
                        <button onClick={editAlert} className="btn btn-outline-success btn-sm">Edit</button>
                        <button onClick={deleteAlert} className="btn btn-outline-success btn-sm">Delete</button>
                    </td>
                }

            </tr>
        </>
    )

    // ------------------------------ Button functionality ------------------------------

    // View button, display popup for additional information about transaction
    function viewAlert() {
        let type = transaction.IsExpense == true ? "Expense" : transaction.IsExpense == false ? "Income" : "";
        let dateMade = transaction.DateTransactionMade != "" ? new Date(transaction.DateTransactionMade.replace(/-/g, '\/')).toDateString() : "";
        let dateEntered = transaction.DateTransactionEntered != "" ? new Date(transaction.DateTransactionEntered.replace(/-/g, '\/')).toDateString() : "";

        Swal.fire({
            title: transaction.TransactionName,
            icon: 'info',
            showCloseButton: true,
            html:
                `<p>This transaction has an amount of <b>$${transaction.Amount}</b> `
                + `and is an <b>${type}</b>.<br><br>This transaction is categorized as <b>${transaction.TransactionCategory}</b>. `
                + `<br><br>The transaction was made on <b>${dateMade}</b> and was entered into the system on <b>${dateEntered}</b>`
        })
    }

    // Edit button, display form to modify the transaction
    function editAlert() {
        Swal.fire({
            title: transaction.TransactionName,
            icon: 'question',
            showConfirmButton: false,
            showCloseButton: true,
            html:
                `<p>Select the transaction attribute to edit.</p>
                <div>
                    <button class="btn btn-primary" id="EditName${transaction.TID}">Name</button>
                    <button class="btn btn-primary" id="EditAmount${transaction.TID}">Amount</button>
                    <button class="btn btn-primary" id="EditType${transaction.TID}">Type</button>
                    <button class="btn btn-primary" id="EditCategory${transaction.TID}">Category</button>
                    <button class="btn btn-primary" id="EditDateMade${transaction.TID}">Date Made</button>
                </div>`

        })
        // Button event handlers
        document.getElementById(`EditName${transaction.TID}`).addEventListener("click", function () {
            editTransactionName()
        });
        document.getElementById(`EditAmount${transaction.TID}`).addEventListener("click", function () {
            editTransactionAmount()
        });
        document.getElementById(`EditType${transaction.TID}`).addEventListener("click", function () {
            editTransactionType()
        });
        document.getElementById(`EditCategory${transaction.TID}`).addEventListener("click", function () {
            editTransactionCategory()
        });
        document.getElementById(`EditDateMade${transaction.TID}`).addEventListener("click", function () {
            editTransactionDateMade()
        });
    }

    // Delete button, display confirmation screen to delete the transaction
    function deleteAlert() {
        Swal.fire({
            title: transaction.TransactionName,
            html: `<p><h4>WARNING</h4>You will not be able to undo this action!<br><br>Are you sure you want to delete this transaction?</p>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showCloseButton: true
        })
        document.getElementsByClassName(`swal2-confirm swal2-styled`)[0].addEventListener("click", function () {
            deleteTransaction()
        });
    }

    async function deleteTransaction() {
        let loading = true;
        while (loading) {
            Swal.fire({
                title: 'Updating',
                html: `<p>Deleting <b>${transaction.TransactionName}</b> from the system.</p>`,
                allowOutsideClick: false,
                onBeforeOpen: () => { Swal.showLoading() }
            });
            let params = { method: "DELETE" }
            await (fetch(url, params))
                .then(response => {
                    if (response.ok) {
                        document.getElementById(`Transaction${transaction.TID}`).remove();

                        // Display success message
                        Swal.fire({
                            title: transaction.TransactionName,
                            icon: "success",
                            html: `<p>Transaction <b>${transaction.TransactionName}</b> was successfuly removed!</p>`,
                            showCloseButton: true
                        })
                    }
                    else {
                        Swal.fire({
                            title: transaction.transactionName,
                            icon: "error",
                            html: `<p>Something went wrong, failed to remove ${transaction.TransactionName} transaction.`,
                            showCloseButton: true
                        })
                    }
                })
            loading = false;
        }
    }

    async function editTransactionName() {
        const { value: newName } = await Swal.fire({
            title: transaction.TransactionName,
            showCloseButton: true,
            icon: "question",
            input: 'text',
            html: '<p>Please enter the new name for the transaction</p>',
            inputPlaceholder: 'Enter a name',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Name cannot be left blank. Please enter a name.'
                }
            }
        })
        // Make PATCH request and update the name of the transaction
        if (newName) {
            let loading = true;
            while (loading) {
                // Show loading message
                Swal.fire({
                    title: 'Updating Transaction Name',
                    html: `<p>Updating name to <b>${newName}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                // Async await is blocking operation
                let params = {
                    method: "PATCH",
                    headers: { "Content-type": "application/json" },
                    body: JSON.stringify({ "TransactionName": newName })
                }
                await (fetch(url, params))
                    .then(response => {
                        if (response.ok) {
                            document.getElementById(`TransactionName${transaction.TID}`).innerHTML = newName;

                            // Display success message
                            Swal.fire({
                                title: newName,
                                icon: "success",
                                html: `<p>Transaction name has successfully been updated from <b>${transaction.TransactionName}</b> to <b>${newName}</b>!</p>`,
                                showCloseButton: true
                            })
                            transaction.TransactionName = newName;
                        }
                        else {
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "error",
                                html: `Something went wrong, failed to update transaction name.</p>`,
                                showCloseButton: true
                            })
                        }
                    })
                //Exit loading loop
                loading = false;
            }
        }
    }

    async function editTransactionAmount() {
        const { value: newAmount } = await Swal.fire({
            title: transaction.TransactionName,
            showCloseButton: true,
            icon: "question",
            input: 'number',
            html: `<p>Enter the new amount for <b>${transaction.TransactionName}</b>:</p>`,
            inputPlaceholder: 'Enter an amount in USD',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Amount cannot be blank. Please enter an amount.'
                }
            }
        })
        // Make PATCH request and update the name of the transaction
        if (newAmount) {
            let loading = true;
            while (loading) {
                // Show loading message
                Swal.fire({
                    title: 'Updating Transaction amount',
                    html: `<p>Updating amount to <b>$${newAmount}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                // Async await is blocking operation
                let params = {
                    method: "PATCH",
                    headers: { "Content-type": "application/json" },
                    body: JSON.stringify({ "Amount": newAmount })
                }
                await (fetch(url, params))
                    .then(response => {
                        if (response.ok) {
                            document.getElementById(`Amount${transaction.TID}`).innerHTML = newAmount;

                            // Display success message
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "success",
                                html: `<p>Transaction name has successfully been updated to <b>${newAmount}</b>!</p>`,
                                showCloseButton: true
                            })
                            transaction.Amount = newAmount;
                        }
                        else {
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "error",
                                html: `Something went wrong, failed to update transaction amount.</p>`,
                                showCloseButton: true
                            })
                        }
                    })
                //Exit loading loop
                loading = false;
            }
        }
    }

    async function editTransactionType() {
        // Prompt user with dropdown menu for transaction type selection
        const { value: newType } = await Swal.fire({
            title: transaction.TransactionName,
            text: "Select the new transaction type",
            input: 'select',
            icon: "question",
            showCloseButton: true,
            inputOptions: {
                true: "Expense",
                false: "Income"
            },
            inputPlaceholder: 'Select an option',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please select an transaction type.'
                }
                return new Promise((resolve) => { resolve() })
            }
        })

        //NEED TO FIX
        // Process input only if user presses submit
        if (newType) {
            let newTypeToString = "";
            if (newType == 'true') {
                newTypeToString = "Expense";
            }
            else {
                newTypeToString = "Income";
            } 
            // Display message if selected type is the same as the old one
            if (newType == transaction.IsExpense) {
                Swal.fire({
                    title: transaction.TransactionName,
                    icon: "warning",
                    html: `<p>The type is already the same as selection. No changes were made.</p>`,
                    showCloseButton: true
                })
            }
            // Make PATCH request and update type
            else {
                let loading = true;
                while (loading) {
                    // show loading message
                    Swal.fire({
                        title: 'Updating transaction type',
                        html: `<p>Updating the transaction type to <b>${newTypeToString}</b></p>`,
                        allowOutsideClick: false,
                        onBeforeOpen: () => { Swal.showLoading() }
                    });
                    // Async await is blocking operation
                    let params = {
                        method: "PATCH",
                        headers: { "Content-type": "application/json" },
                        body: JSON.stringify({ "IsExpense": newType })
                    }
                    await (fetch(url, params))
                            .then((response) => response.json())
                            .then((transactionData) => {
                                transaction = transactionData

                                // Update component without full re-render
                                document.getElementById(`IsExpense${transaction.TID}`).innerHTML = newTypeToString;

                                // Display success message
                                Swal.fetch({
                                    title: transaction.TransactionName,
                                    icon: "success",
                                    html: `<p>NoopeThe transaction type was successfully updated to <b>${newTypeToString}</b>!</p>`,
                                    showCloseButton: true
                                })
                                transaction.IsExpense = newType;
                            })
                        .catch((err) => {
                            console.log(err);
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "success",
                                html: `<p>HelloThe transaction type was successfully updated to <b>${newTypeToString}</b>!</p>`,
                                showCloseButton: true
                            })
                        });
                    //window.location.reload(false);
                    // exit loading loop
                    loading = false;
                }
            }
        }
    }

    async function editTransactionCategory() {
        // Prompt user with dropdown menu for transaction type selection
        const { value: newCategory } = await Swal.fire({
            title: transaction.transactionName,
            text: "Select the new transaction category",
            input: 'select',
            icon: "question",
            showCloseButton: true,
            inputOptions: {
                Entertainment: "Entertainment",
                Restaurants: "Restaurants",
                Transportation: "Transportation",
                HomeAndUtilities: "Home And Utilities",
                Education: "Education",
                Insurance: "Insurance",
                Health: "Health",
                Deposits: "Deposits",
                Shopping: "Shopping",
                Groceries: "Groceries",
                Uncategorized: "Uncategorized"

            },
            inputPlaceholder: 'Select an option',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please select an transaction type.'
                }
                return new Promise((resolve) => { resolve() })
            }
        })
        // Make PATCH request and update the name of the transaction
        if (newCategory) {
            let loading = true;
            while (loading) {
                // Show loading message
                Swal.fire({
                    title: 'Updating Transaction Category',
                    html: `<p>Updating category to <b>${newCategory}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                // Async await is blocking operation
                let params = {
                    method: "PATCH",
                    headers: { "Content-type": "application/json" },
                    body: JSON.stringify({ "TransactionCategory": newCategory })
                }
                await (fetch(url, params))
                    .then(response => {
                        if (response.ok) {
                            document.getElementById(`TransactionCategory${transaction.TID}`).innerHTML = newCategory;

                            // Display success message
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "success",
                                html: `<p>Transaction category has successfully been updated to <b>${newCategory}</b>!</p>`,
                                showCloseButton: true
                            })
                            transaction.TransactionCategory = newCategory;
                        }
                        else {
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "error",
                                html: `<p>Something went wrong, failed to update transaction category.</p>`,
                                showCloseButton: true
                            })
                        }
                    })
                //Exit loading loop
                loading = false;
            }
        }
    }

    async function editTransactionDateMade() {
        Swal.fire({
            title: transaction.TransactionName,
            showCloseButton: true,
            showCancelButton: true,
            icon: "question",
            html:
                `<p>Select a date when the transaction was made below</p>` +
                `<input id="DateBox${transaction.TID}" class="swal2-input" type="date" value=${transaction.DateTransactionMade} required>`,
        });

        document.getElementsByClassName("swal2-confirm swal2-styled")[0].addEventListener("click", async function () {
            // Update the date string for UTC conversion
            let newDateRaw = document.getElementById(`DateBox${transaction.TID}`).value.replace(/-/g, '\/');
            const newDate = new Date(newDateRaw).toDateString();
            const prevDate = new Date(transaction.DateTransactionMade.replace(/-/g, '\/')).toDateString();
            console.log(new Date(newDateRaw).toLocaleDateString());

            //User did not enter a date or entered the same date already set
            if (!newDateRaw || newDate == prevDate) {
                let message = !newDateRaw ? "You did not enter a valid date." : `${transaction.TransactionName} date made is already set to ${newDate}.`;
                Swal.fire({
                    title: transaction.TransactionName,
                    icon: "warning",
                    html: `<p>${message} No changes were made.</p>`,
                    showCloseButton: true
                })
                return;
            }

            //Otherwise update the date
            let loading = true;
            while (loading) {
                //show loading message
                Swal.fire({
                    title: 'Updating',
                    html: `<p>Updating the date made from <b>${prevDate}</b> to <b>${newDate}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });

                let params = {
                    method: "PATCH",
                    headers: { "Content-type": "application/json" },
                    body: JSON.stringify({ "DateTransactionMade": new Date(newDateRaw).toLocaleDateString() })
                }
                await (fetch(url, params))
                    .then(response => {
                        if (response.ok) {
                            document.getElementById(`DateTransactionMade${transaction.TID}`).innerHTML = newDate

                            //Display success message
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "success",
                                html: `<p>The date made has successfully been updated to <b>${newDate}</b></p>`,
                                showCloseButton: true
                            })
                            transaction.DateTransactionMade = newDate;
                        }
                        else {
                            Swal.fire({
                                title: transaction.TransactionName,
                                icon: "error",
                                html: `<p>Something went wrong, failed to update the date made.</p>`,
                                showCloseButton: true
                            })
                        }
                    }
                    )
                //exit loading loop
                loading = false;
            }
        });
    }
}