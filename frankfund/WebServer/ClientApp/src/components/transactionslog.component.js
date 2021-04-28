import React, { Component, useState, useEffect } from "react";
import { withRouter } from 'react-router-dom';
import Transactions from './transactions.component';
import Goals from './savingsgoals.component';
import Swal from 'sweetalert2'

class TransactionsLog extends Component {
    constructor() {
        super()
        this.state = {
            user: "",
            userID: -1,
            transactions: [],
            goals: [],
            dataFetched: false
        };
        this.getTransactions = this.getTransactions.bind(this);
        this.handleRefresh = this.handleRefresh.bind(this);

        // Used only if User has no Transactions
        this.emptyTransactions = [{
            TID: "", AccountID: "", SGID: "", TransactionName: "No transactions to display",
            Amount: "", DateTransactionMade: "", DateTransactionEntered: "", IsExpense: null, TransactionCategory: ""
        }]
    }

    async getGoals() {

    }
    async getTransactions() {
        let user = JSON.parse(localStorage.getItem("user"));
        let apikey = "bd0eecf7cf275751a421a6101272f559b0391fa0";
        let url = `/api/account/user=${user.AccountUsername}/Transactions&apikey=${apikey}`;

        // Retrieve all SavingsGoals for the user
        await (
            fetch(url)
                .then((data) => data.json())
                .then((transactionsData) => {
                    this.setState({ user: user.AccountUsername, userID: user.AccountID, transactions: transactionsData.Transactions, dataFetched: true })
                })
        )
            .catch((err) => {
                console.log(err)
                this.setState({ user: user.AccountUsername, userID : user.AccountID, transactions: this.emptyTransactions, dataFetched: true })
            });
    };

    // Fetch and re-render updated Transactions
    async handleRefresh(){
        this.setState({ user: this.state.user, userID: this.state.userID, transactions: this.state.transactions, dataFetched: false });
        await(this.getTransactions());
    }

    // Update retrieved Transactions
    componentWillMount() {
        this.getTransactions()
    }


    render() {
        // ------------------------------ Button functionality ------------------------------
        async function handleAddTransaction(userID) {
            //Prompt user for the transaction type
            const isAnExpense = await (getIsAnExpense());

            if (isAnExpense == null) return;
            if (isAnExpense) createExpense(userID);
            else createIncome(userID);
        }

        // Prompt radio selection UI for whether to create an income or expense transaction
        // returns boolean or null if user cancelled
        async function getIsAnExpense() {
            const inputOptions = new Promise((resolve) => {
                resolve({
                    'true': "Expense",
                    'false': "Income"
                })
            })
            const { value: isAnExpense } = await Swal.fire({
                title: "Select the type of transaction to create",
                input: 'radio',
                html: `<p>Add a new transaction that is an <b>expense:</b></p> <p>Ex: Spent $20 on gas</p> 
                <p>Add a new transaction that is an <b>income:</b></p> <p>Ex: Deposited $300 toward a specific savings goal</p><br></br>`,
                inputOptions: inputOptions,
                showCancelButton: true,
                showCloseButton: true,
                inputValidator: (value) => {
                    if (!value) {
                        return 'Please select an option'
                    }
                }
            })
            return isAnExpense==null ? null : (isAnExpense == 'true')
        }

        // Creating transaction that is an expense
        async function createExpense(userID) {
            let apikey = "bd0eecf7cf275751a421a6101272f559b0391fa0";
            let url = `/api/Transaction&apikey=${apikey}`;
            const { value: formValues } = await Swal.fire({
                title: "Add a new Transaction",
                html:
                    '<h3>Name</h3>' +
                    '<input id="swal-input1" class="swal2-input" placeholder="Enter the name" required style="font-size: 16pt; height: 40px; width:280px;">' +

                    '<h3>Amount</h3>' +
                    '<input id="swal-input2" class="swal2-input" placeholder="Enter the amount in $ " type="number" required style="height: 40px">' +

                    '<h3>Date transaction was made</h3>' +
                    '<input id="swal-input3" class="swal2-input" placeholder="Choose a date" type="date" required style="height: 40px; width:280px;">' +

                    '<h3>Category</h3>' +
                    '<select id="swal-input4" class="swal2-input" placeholder="Select the category" required style="height: 40px; width:280px;">' +
                    '<option value="Entertainment">Entertainment</option>' +
                    '<option value="Restaurants">Restaurants</option>' +
                    '<option value="Transportation">Transportation</option>' +
                    '<option value="HomeAndUtilities">Home And Utilities</option>' +
                    '<option value="Education">Education</option>' +
                    '<option value="Insurance">Insurance</option>' +
                    '<option value="Health">Health</option>' +
                    '<option value="Deposits">Deposits</option>' +
                    '<option value="Shopping">Shopping</option>' +
                    '<option value="Groceries">Groceries</option>' +
                    '<option value="Uncategorized">Uncategorized</option>' +
                    '</select>',
                focusConfirm: false,
                preConfirm: () => {
                    return [
                        document.getElementById("swal-input1").value,
                        document.getElementById("swal-input2").value,
                        document.getElementById("swal-input3").value,
                        document.getElementById("swal-input4").value
                    ];
                }
            });

            if (formValues) {
                let loading = true;
                while (loading) {
                    Swal.fire({
                        title: 'Creating Transaction',
                        html: `<p>Adding new transaction into the transaction log...`,
                        allowOutSideClick: false,
                        onBeforeOpen: () => { Swal.showLoading() }
                    });
                    let params = {
                        method: "POST",
                        headers: { "Content-type": "application/json" },
                        body: JSON.stringify({
                            "SGID": -1,
                            "AccountID": userID,
                            "TransactionName": formValues[0],
                            "Amount": formValues[1],
                            "DateTransactionMade": formValues[2],
                            "IsExpense": true,
                            "TransactionCategory": formValues[3]
                        })
                    }
                    await (fetch(url, params))
                        .then(response => {
                            if (response.ok) {
                                // Display success message
                                Swal.fire({
                                    title: 'Created Transaction',
                                    icon: "success",
                                    html: `<p>Transaction has successfully added! Refreshing ...</p>`,
                                    showCloseButton: true
                                })
                                // Wait 1.5 seconds before reloading the page
                                setTimeout(function () { window.location.reload(false) }, 1500);
                            }
                            else {
                                Swal.fire({
                                    title: 'Error!',
                                    icon: "error",
                                    html: `Something went wrong, failed to add transaction.</p>`,
                                    showCloseButton: true
                                })
                            }
                        })
                    //Exit loading loop
                    loading = false;
                }
            }
        }

        //TODO: NEED TO ADD WAY TO DISPLAY ALL GOALS AS OPTIONS
        // Creating transaction that is an income
        async function createIncome(userID) {
            let apikey = "bd0eecf7cf275751a421a6101272f559b0391fa0";
            let url = `/api/Transaction&apikey=${apikey}`;
            const { value: formValues } = await Swal.fire({
                title: "Create a Transaction",
                html:
                    '<h3>Name</h3>' +
                    '<input id="swal-input1" class="swal2-input" placeholder="Enter the name" required style="font-size: 16pt; height: 40px; width:280px;">' +

                    '<h3>Amount</h3>' +
                    '<input id="swal-input2" class="swal2-input" placeholder="Enter the amount in $ " type="number" required style="height: 40px">' +

                    '<h3>Date transaction was made</h3>' +
                    '<input id="swal-input3" class="swal2-input" placeholder="Choose a date" type="date" required style="height: 40px; width:280px;">' +

                    '<h3>Category</h3>' +
                    '<select id="swal-input4" class="swal2-input" placeholder="Select the category" required style="height: 40px; width:280px;">' +
                    '<option value="Entertainment">Entertainment</option>' +
                    '<option value="Restaurants">Restaurants</option>' +
                    '<option value="Transportation">Transportation</option>' +
                    '<option value="HomeAndUtilities">Home And Utilities</option>' +
                    '<option value="Education">Education</option>' +
                    '<option value="Insurance">Insurance</option>' +
                    '<option value="Health">Health</option>' +
                    '<option value="Deposits">Deposits</option>' +
                    '<option value="Shopping">Shopping</option>' +
                    '<option value="Groceries">Groceries</option>' +
                    '<option value="Uncategorized">Uncategorized</option>' +
                    '</select>' +

                    '<h3>Select the goal income is going toward</h3>' +
                    '<select id="swal-input5" class="swal2-input" placeholder="Select the savings goal" required style="height: 40px; width:280px;">' +
                    '<option value="-1">No Savings Goal</option>' +
                    '</select>',
                focusConfirm: false,
                preConfirm: () => {
                    return [
                        document.getElementById("swal-input1").value,
                        document.getElementById("swal-input2").value,
                        document.getElementById("swal-input3").value,
                        document.getElementById("swal-input4").value,
                        document.getElementById("swal-input5").value
                    ];
                }
            });

            if (formValues) {
                let loading = true;
                while (loading) {
                    Swal.fire({
                        title: 'Creating Transaction',
                        html: `<p>Adding new transaction into the transaction log...`,
                        allowOutSideClick: false,
                        onBeforeOpen: () => { Swal.showLoading() }
                    });
                    let params = {
                        method: "POST",
                        headers: { "Content-type": "application/json" },
                        body: JSON.stringify({
                            "SGID": formValues[4],
                            "AccountID": userID,
                            "TransactionName": formValues[0],
                            "Amount": formValues[1],
                            "DateTransactionMade": formValues[2],
                            "IsExpense": false,
                            "TransactionCategory": formValues[3]
                        })
                    }
                    await (fetch(url, params))
                        .then(response => {
                            if (response.ok) {
                                // Display success message
                                Swal.fire({
                                    title: 'Created Transaction',
                                    icon: "success",
                                    html: `<p>Transaction has successfully added! Refreshing ...</p>`,
                                    showCloseButton: true
                                })
                                // Wait 1.5 seconds before reloading the page
                                setTimeout(function () { window.location.reload(false) }, 1500);
                            }
                            else {
                                Swal.fire({
                                    title: 'Error!',
                                    icon: "error",
                                    html: `Something went wrong, failed to add transaction.</p>`,
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
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Transactions Log</h1>

                { // Pause loading if data has not been fetched yet
                    !this.state.dataFetched ? <> <a>Loading . . .</a></> :

                        // Otherwise return loaded data
                        <>
                            <h2>Hi {this.state.user}</h2>
                            <div style={{ "max-height": "500px", "overflow": "auto" }}>
                                <button onClick={() => handleAddTransaction(this.state.userID)} className="btn btn-dark btn-blk" style={{ float: "right" }}>New Transaction </button>
                                <table className="table">
                                    <thead>
                                        <tr>
                                            <th>Name</th>
                                            <th>Amount</th>
                                            <th>Date Made</th>
                                            <th>Date Entered</th>
                                            <th>Type</th>
                                            <th>Category</th>
                                            <th>
                                                <input onClick={ this.handleRefresh } type="image" width="30" height="30" style={{float: "right"}} src="https://image.flaticon.com/icons/png/512/61/61444.png" />
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {<Transactions transactions={this.state.transactions} />}
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

export default withRouter(TransactionsLog);