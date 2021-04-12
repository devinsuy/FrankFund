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

    onSubmit(e, form) {
        //e.preventDefault();
        // Simple POST request with a JSON body using fetch
        //const requestOptions = {
        //    method: 'POST',
        //    headers: { 'Content-Type': 'application/json' },
        //    body: JSON.stringify({ title: 'React POST Request Example' })
        //};
        //fetch('api/account/create&apikey={apiKey}', requestOptions)
        //    .then(response => response.json())
        //    .then(data => this.setState({ AccountUsername: data.username, EmailAddress: data.email, PasswordHash: data.password }));

        fetch(form.action, { method: 'post', body: new FormData(form) });

        console.log('We send post asynchronously (AJAX)');
        e.preventDefault();
    }


    render() {
        return (
            <form
                id="create-user-account"
                method="POST" action="api/account/create&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0" onsubmit="send(event,this)">
                <h3>Create New User Account</h3>

                <div className="form-group">
                    <label>Username</label>
                    <input type="text" name="AccountUsername" className="form-control" placeholder="Username" />
                </div>

                <div className="form-group">
                    <label>Email</label>
                    <input type="email" name="EmailAddress" className="form-control" placeholder="Enter email" />
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input type="password" name="Password" className="form-control" placeholder="Enter password" />
                </div>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Register</button>
                <p className="forgot-password text-right">
                    Already registered <a href="#">log in?</a>
                </p>
            </form>
        );
    }
}