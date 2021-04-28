import Swal from 'sweetalert2'
import React, { useState } from "react";


export default function Subscriptions({ subscriptions }) {
    return (
        <>
            { subscriptions.map((subscription) => (<Subscriptions key={subscription.SID} subscription={subscription} />))}
        </>
    )
}

const Subscription = ({ subscription }) => {
    
    let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
    let url = `api/Subscription/SID=${subscription.SID}&apikey=${apikey}`;
    //let endDate = goal.EndDate != "" ? new Date(goal.EndDate.replace(/-/g, '\/')).toDateString() : "";
    let today = new Date().toDateString();
    return (
        <>
            <tr id={`Subscription${subscription.SID}`} key={subscription.SID}>
                <td id={`Account${subscription.AccountID}`}> {subscription.AccountID}</td>
                <td id={`Amount${subscription.SID}`}> {subscription.PurchaseAmount != "" ? "$" + subscription.PurchaseAmount : ""}</td>
                <td id={`Notes${subscription.SID}`}> {subscription.Notes != "" ? + subscription.Notes : ""}</td>
                <td id={`RenewFrequency${subscription.SID}`}> {subscription.Renewfrequency}</td>
                <td id={`PurchaseDate${subscription.SID}`}> {subscription.PurchaseDate}</td>

            { // Display buttons only if user has any subscriptions at all
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
}

// ------------------------------ Button functionality ------------------------------

