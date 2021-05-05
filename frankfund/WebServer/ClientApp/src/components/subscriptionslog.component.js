import React, { Component, useState, useEffect } from "react";
import { withRouter } from 'react-router-dom';
import Subscriptions from './subscriptions.component';
import Swal from 'sweetalert2'

class SubscriptionsLog extends Component {
    constructor() {
        super()
        this.state = {
            user: "",
            userID: -1,
            subscriptions: [],
            dataFetched: false
        };
        this.getGoals = this.getGoals.bind(this);
        this.handleRefresh = this.handleRefresh.bind(this);

        // Used only if User has no SavingsGoals
        this.emptySubscriptions = [{
            SID: "", AccountID: "", RID: "", PurchaseDate: "No subscriptions",
            Notes: "", Amount: "", RenewFrequency: ""
        }]
    }

    async getGoals() {
        console.log("got goals")
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/account/user=${user.AccountUsername}/Subscriptions&apikey=${apikey}`;

        // Retrieve all SavingsGoals for the user
        await (
            fetch(url)
                .then((data) => data.json())
                .then((subscriptionsData) => {
                    this.setState({ user: user.AccountUsername, userID: user.AccountID, subscriptions: subscriptionsData.Subscriptions, dataFetched: true })
                    console.log("goals set")
                })
        )
            .catch((err) => {
                console.log(err)
                this.setState({ user: user.AccountUsername, userID: user.AccountID, subscriptions: this.emptySubscriptions, dataFetched: true })
            });
    };

    // Fetch and re-render updated goals
    async handleRefresh() {
        this.setState({ user: this.state.user, userID: this.state.userID, subscriptions: this.state.subscriptions, dataFetched: false });
        await (this.getGoals());
    }

    // Update retrieved goals
    componentWillMount() {
        this.getGoals();
    }

    render() {
        // UI prompt for creating a goal by date
        async function createByDate(userID) {
            let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
            let url = `/api/Subscription&apikey=${apikey}`;

            const { value: formValues } = await Swal.fire({
                title: "Create a Subscription",
                html:

                    '<h5>Purchase Date</h5>' +
                    '<input required id="swal-input1" class="swal2-input" placeholder="Enter date" type="date" style="font-size: 16pt; height: 40px; width:280px;">' +

                    '<h5>Notes / Name: </h5>' +
                    '<input required id="swal-input2" class="swal2-input" placeholder="Enter a note" style="font-size: 16pt; height: 40px; width:280px;">' +

                    '<h5>Goal Amount</h5>' +
                    '<input required id="swal-input3" class="swal2-input" placeholder="$ Enter amount" type="number" style="height: 40px">' +

                    '<h5>Renew Frequency</h5>'
                    + `<label for="periods"></label>
                            <select name="periods" class="swal2-input" id="swal-input4" style="height: 40px; width:280px;">
                                <option value="Weekly">Weekly</option>
                                <option value="Monthly">Monthly</option>
                                <option value="everyThreeMonths">every three months</option>
                                <option value="everySixMonths">every six months</option>
                                <option value="Yearly">Yearly</option>
                                <option value="NotSpecified">Not Specified</option>
                            </select><br></br>`,


                focusConfirm: false,
                showCancelButton: true,
                showCloseButton: true,
                preConfirm: () => {
                    return [
                        document.getElementById("swal-input1").value,
                        document.getElementById("swal-input2").value,
                        document.getElementById("swal-input3").value,
                        document.getElementById("swal-input4").value
                    ];
                }
            });

            // Create goal if user entered all all fields
            if (formValues) {
                console.log("Formvalues", formValues);
                let loading = true;
                while (loading) {
                    Swal.fire({
                        title: 'Creating Subscription',
                        html: `<p>Adding new subscription to log <b></b> ...</p>`,
                        allowOutSideClick: false,
                        onBeforeOpen: () => { Swal.showLoading() }
                    });
                    let params = {
                        method: "POST",
                        headers: { "Content-type": "application/json" },
                        body: JSON.stringify({
                            "AccountID": userID,
                            "RID":0,
                            "PurchaseDate": formValues[0],
                            "Notes": formValues[1],
                            "Amount": formValues[2],
                            "RenewFrequency": formValues[3]
                        })
                    }
                    await (fetch(url, params))
                        .then(response => {
                            if (response.ok) {
                                // Display success message
                                Swal.fire({
                                    title: formValues[1],
                                    icon: "success",
                                    html: `<p>Subscription has successfully created! Refreshing ...</p>`,
                                    showCloseButton: true
                                })
                                // Wait 1.5 seconds before reloading the page
                                setTimeout(function () { window.location.reload(false) }, 1500);
                            }
                            else {
                                Swal.fire({
                                    title: formValues[0],
                                    icon: "error",
                                    html: `Something went wrong, failed to create subscription.</p>`,
                                    showCloseButton: true
                                })
                            }
                        })
                    //Exit loading loop
                    loading = false;
                }
            }
        }


        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2" >Subscriptions</h1>
                { // Pause loading if data has not been fetched yet
                    !this.state.dataFetched ? <> <a>Loading . . .</a></> :

                        // Otherwise return loaded data
                        <>

                            <h2 id="Hello">Hi {this.state.user} </h2>
                            <div style={{ "max-height": "500px", "overflow": "auto" }}>
                                <button onClick={() => createByDate(this.state.userID)} className="btn btn-dark btn-blk" style={{ float: "right" }}>Add New Subscription </button>
                                <table className="table" id="AllGoals">
                                    <thead>
                                        <tr>
                                            <th>Purchase Date</th>
                                            <th>Notes</th>
                                            <th>Amount</th>
                                            <th>Renew Frequency</th>
                                         
                                            <th>
                                                <input onClick={this.handleRefresh} type="image" width="30" height="30" style={{ float: "right" }} src="https://image.flaticon.com/icons/png/512/61/61444.png" />
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {<Subscriptions subscriptions={this.state.subscriptions} />}
                                        {this.state.dataFetched = false}
                                    </tbody>
                                </table>
                            </div>
                        </>
                }
            </div>
        );
    }
}

export default withRouter(SubscriptionsLog);