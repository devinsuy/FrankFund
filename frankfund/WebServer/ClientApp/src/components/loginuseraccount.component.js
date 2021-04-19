// JavaScript source code for User Account Login React Component

import React, { Component } from "react";
import axios from 'axios';
import swal from 'sweetalert';

export default class LoginUserAccount extends Component {

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
            method: "patch",
            url: "/api/account/accID=5&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0",
            data: json,
            headers: {
                'accept': 'application/json',
                'content-type': 'application/json'
            }
        })
            .then((res) => {
                console.log(res);
                swal("Success!", "Account has successfully been updated!", "success");
            })
            .catch((err) => {
                swal("Error!", "An error has occured.", "error");
                throw err;
            });
    }

    render() {
        return (
            <form
                id="login"
                method='post'
                onSubmit={this.onSubmit}>
                <h3>Login</h3>

                <div className="form-group">
                    <label>Username or Email</label>
                    <input type="text" name="UsernameEmail" className="form-control" placeholder="Need to fill in with previous username" />
                </div>


                <div className="form-group">
                    <label>Password</label>
                    <input type="password" name="Password" className="form-control" placeholder="Enter new password" />
                </div>

                <button type="submit" className="btn btn-dark btn-lg btn-block">Login</button>
            </form>
        );
    }
}