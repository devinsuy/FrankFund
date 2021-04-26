import Swal from 'sweetalert2'
import "react-datepicker/dist/react-datepicker.css";
import React, { useState } from "react";


export default function Subscriptions({ subscriptions }) {
    return (
        <>
            { subscriptions.map((subscription) => (<Subscriptions key={subscription.SID} subscription={subscription} />))}
        </>
    )
}

const Subscription = ({ subscription }) => {
    
    let apikey = "446cc7cf5ad5efab7a1a645cb8f3efbea08cb6b4a3";
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

            </tr>
        </>
    )
}

// ------------------------------ Button functionality ------------------------------

