// JavaScript source code for Transaction Log React Component

import React, {Component} from "react";

export default class TransactionLog extends Component {
    render() {
        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Transaction Log</h1>
                <hr></hr>
                <a href="/create-transaction"><button className="btn btn-dark btn-lg">New Transaction</button></a>
                
                <form>
                    <p>
                    <select>
                        <option>Past Day</option>
                        <option>Past Week</option>
                        <option>Past Month</option>
                        <option>Past 3 Months</option>
                        <option>Past 6 Months</option>
                        <option>Past Year</option>
                        <input type="submit" value="sort"/>
                    </select>
                    <a href="/create-transaction"><button className="btn btn-dark btn-sm">Sort</button></a>
                    </p>
                </form>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Transaction Name</th>
                            <th>Amount</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Example Trans1</td>
                            <td>$50.00</td>
                            <td><a href="/transactions-detail"><button className="btn btn-outline-success btn-sm">View</button></a>
                            <a href="/modify-transaction"><button className="btn btn-outline-success btn-sm">Edit</button></a>
                            <a href="/delete-transaction"><button className="btn btn-outline-success btn-sm">Delete</button></a></td>
                        </tr>
                        <tr>
                            <td>Example Trans2</td>
                            <td>$50.00</td>
                            <td><a href="#"><button className="btn btn-outline-success btn-sm">View</button></a>
                            <a href="#"><button className="btn btn-outline-success btn-sm">Edit</button></a>
                            <a href="#"><button className="btn btn-outline-success btn-sm">Delete</button></a></td>
                        </tr>
                    </tbody>
                </table>
            </div>

        )
    }
}