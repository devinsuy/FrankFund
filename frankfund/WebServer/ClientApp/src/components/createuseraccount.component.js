// JavaScript source code for Create New User Account React Component

import React, { Component } from "react";

export default class CreateUserAccount extends Component {

    constructor(props) {
        super(props);

        this.state = { username: null, email: null, password: null };
    }

    //onChange(e) {
    //    this.setState({
    //        [e.target.name]: e.target.value
    //    });
    //}

    //onSubmit(e) {
    //    e.preventDefault();

    //    fetch('/api/account/create', {
    //        method: 'POST',
    //        headers: {
    //            'Accept': 'application/json',
    //            'Content-Type': 'application/json'
    //        },
    //        body: JSON.stringify({ description: this.state.description })
    //    });

    //    this.setState({ description: '' });
    //}

    onSubmit(e) {
        e.preventDefault();
        // Simple POST request with a JSON body using fetch
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title: 'React POST Request Example' })
        };
        fetch('api/account/create', requestOptions)
            .then(response => response.json())
            .then(data => this.setState({ AccountUsername: data.username, EmailAddress: data.email, PasswordHash: data.password }));
    }


    render() {
        return (
            <form
                id="create-user-account"
                action='api/account/create'
                method= 'post'
                onSubmit={this.onSubmit}>
                <h3>Create New User Account</h3>

                <div className="form-group">
                    <label>Username</label>
                    <input type="AccountUsername" className="form-control" placeholder="Username" />
                </div>

                <div className="form-group">
                    <label>Email</label>
                    <input type="EmailAddress" className="form-control" placeholder="Enter email" />
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input type="PasswordHash" className="form-control" placeholder="Enter password" />
                </div>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Register</button>
                <p className="forgot-password text-right">
                    Already registered <a href="#">log in?</a>
                </p>
            </form>
        );
    }
}