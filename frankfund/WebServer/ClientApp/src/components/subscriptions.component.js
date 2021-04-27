import Swal from 'sweetalert2'
import React, { useState } from "react";


export default function Subscriptions({subscriptions}) {
    return (
        <>
            { subscriptions.map((subscription) => ( <Subscription key={subscription.SID} subscription={subscription} /> ))}
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
                <td id={`RenewalFrequency${subscription.SID}`}> {subscription.RenewalFrequency}</td>
                <td>
                    <button className="btn btn-outline-success btn-sm">View</button>
                    <button className="btn btn-outline-success btn-sm">Edit</button>
                    <button className="btn btn-outline-success btn-sm">Delete</button>
                </td>
            </tr>
        </>
    )
}


