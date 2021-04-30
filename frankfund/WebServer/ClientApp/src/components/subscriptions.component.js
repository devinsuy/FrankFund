import Swal from 'sweetalert2'
import React, { useState } from "react";


export default function Subscriptions({ subscriptions }) {
    return (
        <>
            { subscriptions.map((subscription) => (<Subscription key={subscription.SID} subscription={subscription} />))}
        </>
    )
}


const Subscription = ({ subscription }) => {
    // Build api url for this particular goal
    let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    let url = `/api/Subscription/SID=${subscription.SID}&apikey=${apikey}`;
    
    return (
        <>
            <tr id={`Subscription${subscription.SID}`} key={subscription.SID}>
                <td id={`PurchaseDate${subscription.SID}`}> {subscription.PurchaseDate}</td>
                <td id={`Notes${subscription.SID}`}> {subscription.Notes}</td>
                <td id={`Amount${subscription.SID}`}> {subscription.Amount}</td>
                <td id={`RenewFrequency${subscription.SID}`}> {subscription.RenewFrequency}</td>
                {
                    subscription.SID == "" ? <></> :
                    <td>
                        <button onClick={viewAlert} className="btn btn-outline-success btn-sm">View</button>
                        <button onClick={editAlert} className="btn btn-outline-success btn-sm">Edit</button>
                        <button onClick={deleteAlert} className="btn btn-outline-success btn-sm">Delete</button>
                    </td>
                }
            </tr>
        </>
    )

    //Button functionality
    // Edit button, display form to modify the subscription
    function editAlert() {
        Swal.fire({
            title: subscription.Notes,
            icon: 'question',
            showConfirmButton: false,
            showCloseButton: true,
            html:
                `<p>Select the subscription attribute to edit.</p>
                <div>
                    <button class="btn btn-primary" id="EditNotes${subscription.SID}">Notes</button>
                    <button class="btn btn-primary" id="EditAmount${subscription.SID}">Amount</button>
                    <button class="btn btn-primary" id="EditDate${subscription.SID}">Date</button>
                    <button class="btn btn-primary" id="EditRenewFrequency${subscription.SID}">Renew Frequency</button>
                </div>`

        })
        // Button event handlers
        document.getElementById(`EditNotes${subscription.SID}`).addEventListener("click", function () {
            editSubscriptionNotes()
        });
        document.getElementById(`EditAmount${subscription.SID}`).addEventListener("click", function () {
            editSubscriptionAmount()
        });
        document.getElementById(`EditDate${subscription.SID}`).addEventListener("click", function () {
            editSubscriptionDate()
        });
        document.getElementById(`EditRenewFrequency${subscription.SID}`).addEventListener("click", function () {
            editSubscriptionFrequency()
        });
    }


    async function editSubscriptionNotes() {
        const { value: newNote } = await Swal.fire({
            title: subscription.Notes,
            showCloseButton: true,
            icon: "question",
            input: 'text',
            html: '<p>Please enter the new note for the subscription</p>',
            inputPlaceholder: 'Enter a note',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Note cannot be left blank. Please enter a note.'
                }
            }
        })
        // Make PATCH request and update the name of the subscription
        if (newNote) {
            let loading = true;
            while (loading) {
                // Show loading message
                Swal.fire({
                    title: 'Updating Subscription Note',
                    html: `<p>Updating note to <b>${newNote}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                // Async await is blocking operation
                let params = {
                    method: "PATCH",
                    headers: { "Content-type": "application/json" },
                    body: JSON.stringify({ "Notes": newNote })
                }
                await (fetch(url, params))
                    .then(response => {
                        if (response.ok) {
                            document.getElementById(`Notes${subscription.SID}`).innerHTML = newNote;

                            // Display success message
                            Swal.fire({
                                title: newNote,
                                icon: "success",
                                html: `<p>Subscription name has successfully been updated from <b>${subscription.Notes}</b> to <b>${newNote}</b>!</p>`,
                                showCloseButton: true
                            })
                            subscription.Notes = newNote;
                        }
                        else {
                            Swal.fire({
                                title: subscription.Notes,
                                icon: "error",
                                html: `Something went wrong, failed to update the subscription note.</p>`,
                                showCloseButton: true
                            })
                        }
                    })
                //Exit loading loop
                loading = false;
            }
        }
    }

    async function editSubscriptionAmount() {
        const { value: newAmount } = await Swal.fire({
            title: subscription.Notes,
            showCloseButton: true,
            icon: "question",
            input: 'number',
            html: `<p>Enter the new amount for <b>${subscription.Notes}</b>:</p>`,
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
                    title: 'Updating Subscription amount',
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
                            document.getElementById(`Amount${subscription.SID}`).innerHTML = newAmount;

                            // Display success message
                            Swal.fire({
                                title: subscription.Notes,
                                icon: "success",
                                html: `<p>Subscription name has successfully been updated to <b>${newAmount}</b>!</p>`,
                                showCloseButton: true
                            })
                            subscription.Amount = newAmount;
                        }
                        else {
                            Swal.fire({
                                title: subscription.Notes,
                                icon: "error",
                                html: `Something went wrong, failed to update Subscription amount.</p>`,
                                showCloseButton: true
                            })
                        }
                    })
                //Exit loading loop
                loading = false;
            }
        }
    }

    async function editSubscriptionFrequency() {
        // Prompt user with dropdown menu for subscription type selection
        const { value: newFrequency } = await Swal.fire({
            title: subscription.Notes,
            text: "Select the new renewal frequency",
            input: 'select',
            icon: "question",
            showCloseButton: true,
            inputOptions: {
                Weekly: "Weekly",
                Monthly: "Monthly",
                everyThreeMonths: "Every 3 Months",
                everySixMonths: "Every 6 Months",
                Yearly: "Yearly",
                NotSpecified: "Unspecified",
            },

            inputPlaceholder: 'Select an option',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please select an renewal frequency.'
                }
                return new Promise((resolve) => { resolve() })
            }
        })
        // Make PATCH request and update the name of the subscription
        if (newFrequency) {
            let loading = true;
            while (loading) {
                // Show loading message
                Swal.fire({
                    title: 'Updating Subscription frequency',
                    html: `<p>Updating frequency to <b>${newFrequency}</b></p>`,
                    allowOutsideClick: false,
                    onBeforeOpen: () => { Swal.showLoading() }
                });
                // Async await is blocking operation
                let params = {
                    method: "PATCH",
                    headers: { "Content-type": "application/json" },
                    body: JSON.stringify({ "RenewFrequency": newFrequency })
                }
                await (fetch(url, params))
                    .then(response => {
                        if (response.ok) {
                            document.getElementById(`RenewFrequency${subscription.SID}`).innerHTML = newFrequency;

                            // Display success message
                            Swal.fire({
                                title: subscription.Notes,
                                icon: "success",
                                html: `<p>Subscription renewal frequency has successfully been updated to <b>${newFrequency}</b>!</p>`,
                                showCloseButton: true
                            })
                            subscription.RenewFrequency = newFrequency;
                        }
                        else {
                            Swal.fire({
                                title: subscription.Notes,
                                icon: "error",
                                html: `<p>Something went wrong, failed to update subscription renewal frequency.</p>`,
                                showCloseButton: true
                            })
                        }
                    })
                //Exit loading loop
                loading = false;
            }
        }
    }

    function formatDateForJSON(dateString) {
        const dateArray = dateString.split('/')
        let newDate = ''
        for (let i = dateArray.length - 1; i >= 0; i--) {
            if (i > 0) {
                newDate += dateArray[i] + '-'
            } else {
                newDate += dateArray[i]
            }
        }
        console.log(newDate)
        return newDate;
    }

    async function editSubscriptionDate() {
        Swal.fire({
            title: subscription.Notes,
            showCloseButton: true,
            showCancelButton: true,
            icon: "question",
            html:
                `<p>Select a date when the subscription was made below</p>` +
                `<input id="DateBox${subscription.SID}" class="swal2-input" type="date" value=${subscription.PurchaseDate} required>`,
        });

        document.getElementsByClassName("swal2-confirm swal2-styled")[0].addEventListener("click", async function () {
            // Update the date string for UTC conversion
            let newDateRaw = document.getElementById(`DateBox${subscription.SID}`).value.replace(/-/g, '\/');
            const newDate = new Date(newDateRaw).toDateString();
            const prevDate = new Date(subscription.PurchaseDate.replace(/-/g,'\/')).toDateString();
            const formattedNewDate = newDateRaw.replaceAll("/", "-");
            //console.log(new Date(newDateRaw).toLocaleDateString());
           
            console.log(formattedNewDate, "FORMATTED NEW DATE");
            console.log(newDateRaw.replaceAll("/", "-"), "NEW DATE RAW");
            console.log(newDateRaw.replace('\/', /-/g), "NEW DATE RAW WITH REGEX");
            console.log(newDate, "NEW DAAATE");

            //User did not enter a date or entered the same date already set
            if (!newDateRaw || newDate == prevDate) {
                let message = !newDateRaw ? "You did not enter a valid date." : `${subscription.Notes} date made is already set to ${newDate}.`;
                Swal.fire({
                    title: subscription.Notes,
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
                    body: JSON.stringify({ "PurchaseDate": formattedNewDate })
                }
                await (fetch(url, params))
                    .then(response => {
                        if (response.ok) {
                            document.getElementById(`PurchaseDate${subscription.SID}`).innerHTML = newDate

                            //Display success message
                            Swal.fire({
                                title: subscription.Notes,
                                icon: "success",
                                html: `<p>The date made has successfully been updated to <b>${newDate}</b></p>`,
                                showCloseButton: true
                            })
                            subscription.PurchaseDate = newDate;
                        }
                        else {
                            Swal.fire({
                                title: subscription.Notes,
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

    async function deleteSubscription() {
        let loading = true;
        while (loading) {
            Swal.fire({
                title: 'Updating',
                html: `<p>Deleting <b>${subscription.Notes}</b> from the system.</p>`,
                allowOutsideClick: false,
                onBeforeOpen: () => { Swal.showLoading() }
            });
            let params = { method: "DELETE" }
            await (fetch(url, params))
                .then(response => {
                    if (response.ok) {
                        document.getElementById(`Subscription${subscription.SID}`).remove();

                        // Display success message
                        Swal.fire({
                            title: subscription.Notes,
                            icon: "success",
                            html: `<p>Subscription <b>${subscription.Notes}</b> was successfuly removed!</p>`,
                            showCloseButton: true
                        })
                    }
                    else {
                        Swal.fire({
                            title: subscription.Notes,
                            icon: "error",
                            html: `<p>Something went wrong, failed to remove ${subscription.Notes} subscription.`,
                            showCloseButton: true
                        })
                    }
                })
            loading = false;
        }
    }

    // Delete button, display confirmation screen to delete the transaction
    function deleteAlert() {
        Swal.fire({
            title: subscription.Notes,
            html: `<p><h4>WARNING</h4>You will not be able to undo this action!<br><br>Are you sure you want to delete this subscription?</p>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm',
            showCloseButton: true
        })
        document.getElementsByClassName(`swal2-confirm swal2-styled`)[0].addEventListener("click", function () {
            deleteSubscription()
        });
    }

    function viewAlert() {
      
        let dateMade = subscription.PurchaseDate != "" ? new Date(subscription.PurchaseDate.replace(/-/g, '\/')).toDateString() : "";
        //let dateEntered = transaction.DateTransactionEntered != "" ? new Date(transaction.DateTransactionEntered.replace(/-/g, '\/')).toDateString() : "";

        Swal.fire({
            title: subscription.Notes,
            icon: 'info',
            showCloseButton: true,
            html:
                `<p>This subscription has an amount of <b>$${subscription.Amount}</b> `
                + `<br>This subscription renews <b>${subscription.RenewFrequency}</b>. `
                + `<br><br>The subscription was made on <b>${dateMade}</b>` + `</br> with a note of ${subscription.Notes}</b> `
        })
    }
}


