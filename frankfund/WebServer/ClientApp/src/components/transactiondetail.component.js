// JavaScript source code for Create New User Account React Component

import React, { Component } from "react";

export default class TransactionDetail extends Component {
    render() {
        return (
            <div className="container">
                <h1 class="display-4 font-weight-bold white-text pt-5 mb-2">Transaction Detail</h1>
                <hr></hr>
                <dl class="row">
                    <dt class="col-sm-5">Transaction Name</dt>
                    <dd class="col-sm-8">Temporary Name</dd>
                    <dt class="col-sm-5">Amount</dt>
                    <dd class="col-sm-8">$420.42</dd>
                    <dt class="col-sm-5">Type</dt>
                    <dd class="col-sm-8">Expense</dd>
                    <dt class="col-sm-5">Category</dt>
                    <dd class="col-sm-8">Temporary Category</dd>
                    <dt class="col-sm-5">Transaction made on</dt>
                    <dd class="col-sm-8">12/12/1212</dd>
                    <dt class="col-sm-6">Transaction entered into the system on</dt>
                    <dd class="col-sm-8">00/00/0000</dd>
                </dl>
                <a href="\transactions-log"><button className="btn btn-dark">Return</button></a>
            </div>
        );
    }
}