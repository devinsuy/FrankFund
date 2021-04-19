// JavaScript source code for Transaction Log React Component

import React, {Component, useState, useEffect} from "react";
import { withRouter } from 'react-router';
import Transactions from './transactions.component';

class TransactionLog extends Component {
    constructor(){
        super()
        this.state = {
            user: "",
            transactions: [],
            dataFetched : false
        };
        // Placeholder for user with no transactions
        this.emptyTransaction = [{
            TID: "", accountID: "", SGID: "", transactionName: "No Transactions to Display", amount: "", isExpense: "",
            category: "", dateTransactionMade: "", dateTransactionEntered: ""
        }]
    }

    async getTransactions() {
        let user = this.props.match.params.user;
        let apikey = "c55f8d138f6ccfd43612b15c98706943e1f4bea3";
        let url = `/api/account/user=${user}/Transaction&apikey=${apikey}`;

        // Retrieve all transactions for the user
        await(
            fetch(url)
            .then((data) => data.json())
            .then((transactionsData) => {
                this.setState({ user: user, transactions: transactionsData.transactions, dataFetched: true})
            })
        )
        .catch((err) => {
            console.log(err)
            this.setState({ user: user, transactions: this.emptyTransaction, dataFetched: true })
        });
    };

    // Update retrieved transactions
    componentWillMount() {
        this.getTransactions();
    }

    render() {
        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Transaction Log</h1>
                
                
                { // Pause loading if data has notbeen fetched yet
                !this.state.dataFetched ? <> <a>Loading all transactions . . .</a></> :
                
                // Otherwise return loaded data
                <>
                    <b>Hi {this.state.user}</b>
                    <hr></hr>
                    <a href="/create-transaction"><button className="btn btn-dark btn-lg">New Transaction</button></a>
                    <table className="table">
                        <thead>
                            <tr>
                                <th>Transaction Name</th>
                                <th>Amount</th>
                                <th>Type</th>
                                <th>Category</th>
                                <th>Date Made</th>
                                <th>Date Entered</th>
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

export default withRouter(TransactionLog);