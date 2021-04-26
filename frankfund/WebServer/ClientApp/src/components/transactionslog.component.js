import React, { Component, useState, useEffect } from "react";
import { withRouter } from 'react-router-dom';
import Transactions from './transactions.component';
import Swal from 'sweetalert2'

class TransactionsLog extends Component {
    constructor() {
        super()
        this.state = {
            user: "",
            transactions: [],
            dataFetched: false
        };
        // Used only if User has no Transactions
        this.emptyTransactions = [{
            TID: "", AccountID: "", SGID: "", TransactionName: "",
            Amount: "", DateTransactionMade: "", DateTransactionEntered: "", IsExpense: "", TransactionCategory: ""
        }]
    }

    async getTransactions() {
        let user = JSON.parse(localStorage.getItem("user")).AccountUsername;
        let apikey = "bd0eecf7cf275751a421a6101272f559b0391fa0";
        let url = `/api/account/user=${user}/Transactions&apikey=${apikey}`;

        // Retrieve all SavingsGoals for the user
        await (
            fetch(url)
                .then((data) => data.json())
                .then((transactionsData) => {
                    this.setState({ user: user, transactions: transactionsData.Transactions, dataFetched: true })
                })
        )
            .catch((err) => {
                console.log(err)
                this.setState({ user: user, transactions: this.emptyTransactions, dataFetched: true })
            });
    };

    // Update retrieved goals
    componentWillMount() {
        this.getTransactions()
    }


    render() {
        // ------------------------------ Button functionality ------------------------------
        async function AddAlert() {
            //let user = this.props.match.params.user;
            let apikey = "bd0eecf7cf275751a421a6101272f559b0391fa0";
            let url = `/api/Transaction&apikey=${apikey}`;
            const { value: formValues } = await Swal.fire({
                title: "Create a Transaction",
                html:
                    '<h3>Name</h3>' +
                    '<input id="swal-input1" class="swal2-input" placeholder="Enter the name" required>' +

                    '<h3>Amount</h3>' +
                    '<input id="swal-input2" class="swal2-input" placeholder="Enter the amount in $ " type="number" required>' +

                    '<h3>Date transaction was made</h3>' +
                    '<input id="swal-input3" class="swal2-input" placeholder="Choose a date" type="date" required>' +

                    '<h3>Type</h3>' +
                    '<select id="swal-input4" class="swal2-input" placeholder="Select the type" required>' +
                    '<option value="true">Expense</option>' +
                    '<option value="false">Income</option>' +
                    '</select>' +

                    '<h3>Category</h3>' +
                    '<select id="swal-input5" class="swal2-input" placeholder="Select the category" required>' +
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
                            "SGID": 2,
                            "AccountID": 4,
                            "TransactionName": formValues[0],
                            "Amount": formValues[1],
                            "DateTransactionMade": formValues[2],
                            "IsExpense": formValues[3],
                            "TransactionCategory": formValues[4]})
                    }
                    await (fetch(url, params))
                        .then(response => {
                            if (response.ok) {
                                // Display success message
                                Swal.fire({
                                    title: 'Created Transaction',
                                    icon: "success",
                                    html: `<p>Transaction has successfully added!</p>`,
                                    showCloseButton: true
                                })
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
                    window.location.reload(false);
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
                            <button onClick={AddAlert} className="btn btn-dark btn-blk" style={{float: "right"}}>New Transaction </button>
                            <h2>Hi {this.state.user}</h2>
                            <table className="table">
                                <thead>
                                    {/* <tr>
                                        <button onClick={AddAlert} className="btn btn-dark btn-blk">New Transaction</button>
                                        <br></br>
                                    </tr> */}
                                    <tr>
                                        <th>Name</th>
                                        <th>Amount</th>
                                        <th>Date Made</th>
                                        <th>Date Entered</th>
                                        <th>Type</th>
                                        <th>Category</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {<Transactions transactions={this.state.transactions} />}
                                    {this.state.dataFetched = false}
                                </tbody>
                            </table>
                        </>
                }
            </div>
        );
    }
}

export default withRouter(TransactionsLog);