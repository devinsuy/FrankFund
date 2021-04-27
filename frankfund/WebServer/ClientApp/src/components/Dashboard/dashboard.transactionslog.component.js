import React, { Component, useState, useEffect } from "react";
import Transactions from './dashboard.transactions.component';
import Typography from '@material-ui/core/Typography';
import Link from '@material-ui/core/Link';
import { makeStyles } from '@material-ui/core/styles';
import { withStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    seeMore: {
        marginTop: theme.spacing(3),
    },
}));

class TransactionsLog extends Component {
    constructor() {
        super()
        this.state = {
            user: "",
            userID: -1,
            transactions: [],
            dataFetched: false
        };
        this.getTransactions = this.getTransactions.bind(this);

        // Used only if User has no Transactions
        this.emptyTransactions = [{
            TID: "", AccountID: "", SGID: "", TransactionName: "No transactions to display",
            Amount: "", DateTransactionMade: "", DateTransactionEntered: "", IsExpense: null, TransactionCategory: ""
        }]
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
                    // Display only 5 most recent transactions on dashboard
                    let recentTransactions = transactionsData.Transactions.slice(0, 5);
                    this.setState({ user: user.AccountUsername, userID: user.AccountID, transactions: recentTransactions, dataFetched: true })
                })
        )
            .catch((err) => {
                console.log(err)
                this.setState({ user: user.AccountUsername, userID : user.AccountID, transactions: this.emptyTransactions, dataFetched: true })
            });
    };


    // Update retrieved Transactions
    componentWillMount() {
        this.getTransactions()
    }

    render() {
        const { classes } = this.props;

        return (
            <div className="container">
                <Typography component="h2" variant="h6" color="primary" gutterBottom>
                    Recent Transactions
                </Typography>
                { // Pause loading if data has not been fetched yet
                    !this.state.dataFetched ? <> <a>Loading . . .</a></> :

                        // Otherwise return loaded data
                        <>
                            <table className="table">
                                <thead>
                                    <tr>
                                        <th>Name</th>
                                        <th>Amount</th>
                                        <th>Date Made</th>
                                        <th>Date Entered</th>
                                        <th>Type</th>
                                        <th>Category</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {<Transactions transactions={this.state.transactions} />}
                                    {this.state.dataFetched = false}
                                </tbody>
                            </table>
                            <div className={classes.seeMore}>
                                <Link color="primary" href="/transactions">
                                    See more transactions
                                </Link>
                            </div>
                        </>
                }
            </div>
        );
    }
}

export default withStyles(useStyles)(TransactionsLog)