import React, {Component, useState, useEffect} from "react";
import { withRouter } from 'react-router-dom';
import Subscriptions from './subscriptions.component';
import Swal from 'sweetalert2'

class SubscriptionsLog extends Component{
    constructor(){
        super()
        this.state = {
            user: "",
            userID: -1,
            subscriptions: [],
            dataFetched : false
        };
        this.getSubscriptions = this.getSubscriptions.bind(this);
        this.handleRefresh = this.handleRefresh.bind(this);

        // Used only if User has no SavingsGoals
        this.emptySubscriptions = [{
            SID: "", AccountID: "", RID: "", PurchaseDate: "No subscriptions to show", 
            Notes: "", Amount: "", RenewFrequency: ""
        }]
    }

    async getSubscriptions(){
        console.log("got subscriptions")
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/account/user=${user.AccountUsername}/Subscriptions&apikey=${apikey}`;

        // Retrieve all SavingsGoals for the user
        await(
            fetch(url)
            .then((data) => data.json())
            .then((subscriptionsData) => {
                this.setState({ user: user.AccountUsername, userID: user.AccountID, subscriptions: subscriptionsData.Subscriptions, dataFetched: true })
                console.log("subscriptions set")
            })
        )
        .catch((err) => { 
            console.log(err)
            this.setState({ user: user.AccountUsername, userID: user.AccountID, subscriptions: this.emptySubscriptions, dataFetched: true })
        });
    };

    // Fetch and re-render updated goals
    async handleRefresh(){
        this.setState({ user: this.state.user, userID: this.state.userID, subscriptions: this.state.subscriptions, dataFetched: false });
        await (this.getSubscriptions());
    }

    // Update retrieved goals
    componentWillMount() {
        this.getSubscriptions();
    }

    render(){
        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2" >Subscriptions</h1>
                { // Pause loading if data has not been fetched yet
                !this.state.dataFetched ? <> <a>Loading . . .</a></> : 

                // Otherwise return loaded data
                <>
                    <h2 id="Hello">Hi {this.state.user} </h2>
                    <table className="table" id="AllSubscriptions">
                        <thead>
                            <tr>
                                <th>PurchaseDate</th>
                                <th>Notes</th>
                                <th>Amount</th>
                                <th>Renewal Frequency</th>
                                <th>
                                    <input onClick={ this.handleRefresh } type="image" width="30" height="30" style={{float: "right"}} src="https://image.flaticon.com/icons/png/512/61/61444.png" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {<Subscriptions subscriptions={this.state.subscriptions} />}
                            {this.state.dataFetched = false}
                        </tbody>
                    </table>
                </>
                }  
            </div>
        );
    }
}

export default withRouter(SubscriptionsLog);