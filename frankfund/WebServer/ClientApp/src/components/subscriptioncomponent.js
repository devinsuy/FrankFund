import React, { Component, useState, useEffect } from "react";
import { withRouter } from 'react-router-dom';
import Subscriptions from './subscriptions.functionality';

class SubscriptionLog extends Component {
    constructor() {
        super()
        this.state = { //intital state of the object
            SID: "", AccountID: "", RID: "", PurchaseDate: "", Notes: "No notes to display.",
            Amount: "", RenewFrequency: "",
            
            dataFetched: false
        };

        //Use this if user has no subscriptions 
        this.emptySubscription = [{
            SID: "", AccountID: "", RID: "", PurchaseDate: "", Notes: "No notes to display.", 
            Amount: "", RenewFrequency: ""
        }]

    }

    async getSubscriptions() {
        let user = JSON.parse(localStorage.getItem("user")).AccountID;
        let apikey = "446cc7cf5ad5efab7a1a645cb8f3efbea08cb6b4";
        let url = `/api/account/user=${user.AccountID}/Subscription&apikey=${apikey}`;


        // https://localhost:44310/api/account/user=$rachelpai/subscription&apikey=$446cc7cf5ad5efab7a1a645cb8f3efbea08cb6b4

        // Retrieve all Subscriptions for the user
        await (
            fetch(url)
                .then((data) => data.json())
                .then((subscriptionsData) => {
                    this.setState({ user: user, subscriptions: subscriptionsData.Subscriptions, dataFetched: true })
                })
        )
            .catch((err) => {
                console.log(err)
                this.setState({ user: user, subscriptions: this.emptySubscription, dataFetched: true })
            });
    };

    // Update retrieved goals
    componentWillMount() {
        this.getSubscriptions()
    }

    render() {
        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Subscriptions</h1>

                { // Pause loading if data has not been fetched yet
                    !this.state.dataFetched ? <> <a>Loading . . .</a></> :

                        // Otherwise return loaded data
                        <>
                            <b>Hi {this.state.user}</b>
                            <table className="table">
                                <thead>
                                    <tr>
                                        <th>Purchase Amount</th>
                                        <th>Notes</th>
                                        <th>Renew Frequency</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    
                                    {this.state.dataFetched = false}
                                </tbody>
                            </table>
                        </>
                }
            </div>
        );
    }
}

export default withRouter(SubscriptionLog);
