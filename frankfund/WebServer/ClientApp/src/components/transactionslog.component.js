import React, { Component, useState, useEffect } from "react";
import { withRouter } from 'react-router';
import Transactions from './transactions.component';

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
        // TODO: Validation, if /goals/{user} != the logged in user return unauthorized (other users shouldn't be able to access data that isn't theirs)
        let user = this.props.match.params.user;                                // Select user from route /goals/{user}
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
        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Transactions Log</h1>

                { // Pause loading if data has not been fetched yet
                    !this.state.dataFetched ? <> <a>Loading . . .</a></> :

                        // Otherwise return loaded data
                        <>
                            <b>Hi {this.state.user}</b>
                            <table className="table">
                                <thead>
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