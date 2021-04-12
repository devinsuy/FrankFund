// JavaScript source code for Create New User Account React Component

import React, { Component } from "react";
import axios from 'axios';
import $ from 'jquery'; 

export default class CreateUserAccount extends Component {

    //constructor(props) {
    //    super(props);

    //    this.state = { username: null, email: null, password: null };
    //}

    //state = {
    //    AccountUsername: '',
    //    EmailAddress: '',
    //    Password: ''
    //}

    // ----- Testing with static JSON input -----
    //onSubmit(e) {
    //    e.preventDefault();

    //    axios.post('/api/account/create&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0', {
    //        AccountUsername: 'testing',
    //        EmailAddress: 'testing@gmail.com',
    //        Password: 'Password1'
    //    })
    //        .then((response) => {
    //            console.log(response);
    //        }, (error) => {
    //            console.log(error);
    //        });
    //}

    onSubmit(e) {
        e.preventDefault();
        const form = document.querySelector("form");
        let data = new FormData(form);
        var object = {};
        data.forEach(function (value, key) {
            object[key] = value;
        });
        var json = JSON.stringify(object);
        console.log(json);
        axios({
            method: "post",
            url: "/api/account/create&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
            data: json,
            headers: {
                'accept': 'application/json',
                'content-type': 'application/json'
            }
        })
            .then((res) => {
                console.log(res);
            })
            .catch((err) => {
                throw err;
            });
    }

    render() {
        return (
            <form action="" method="post" onSubmit={this.onSubmit}>
                <h3>Create New User Account</h3>

                <div className="form-group">
                    <label>Username</label>
                    <input type="text" name="AccountUsername" className="form-control" placeholder="Username" />
                </div>

                <div className="form-group">
                    <label>Email</label>
                    <input type="email" name="EmailAddress" className="form-control" placeholder="Enter email"/>
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input type="password" name="Password" className="form-control" placeholder="Enter password"/>
                </div>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Register</button>
                <p className="forgot-password text-right">
                    Already registered <a href="#">log in?</a>
                </p>
            </form>
        );
    }
}